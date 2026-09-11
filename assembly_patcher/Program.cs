using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

class Program
{
    static int changes = 0;

    static int Main(string[] args)
    {
        string targetExe = args.Length > 0
            ? args[0]
            : Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "app", "Deepchart.exe");

        targetExe = Path.GetFullPath(targetExe);
        Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Deepchart.EXE Mono.Cecil IL Patcher — Approach 3 replacement  ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
        Console.WriteLine($"\nTarget: {targetExe}");

        if (!File.Exists(targetExe))
        {
            Console.WriteLine("ERROR: EXE not found at: " + targetExe);
            return 1;
        }

        string dir = Path.GetDirectoryName(targetExe)!;
        string backup = Path.Combine(dir, $"Deepchart.exe.orig_{DateTime.Now:yyyyMMdd_HHmmss}");
        File.Copy(targetExe, backup, overwrite: true);
        Console.WriteLine($"Backup → {Path.GetFileName(backup)}");

        var rp = new ReaderParameters
        {
            ReadWrite = true,
            ReadSymbols = false,
            AssemblyResolver = new CustomResolver(dir)
        };

        using (var asm = AssemblyDefinition.ReadAssembly(targetExe, rp))
        {
            ModuleDefinition mod = asm.MainModule;
            ImportSystemPrimitives(mod, out TypeReference t_bool, out TypeReference t_int, out TypeReference t_string, out TypeReference t_date, out MethodReference ctor_str);
            var il = mod.TypeSystem;

            Console.WriteLine("\n[1/5] Locate License.Status type");
            TypeDefinition? licenseStatus = FindType(mod, "License.Status");
            if (licenseStatus == null) { Console.WriteLine("FATAL: License.Status not found in assembly!"); return 2; }
            Console.WriteLine($"  Found: License.Status ({licenseStatus.Methods.Count} methods, {licenseStatus.Fields.Count} fields)");

            Console.WriteLine("\n[2/5] PATCH 1 — Rewrite ALL Status.* getters/checkers to always return VALID");
            PatchAllStatusMethods(mod, licenseStatus, t_bool, t_int, t_string, t_date);

            Console.WriteLine("\n[3/5] PATCH 2 — Rewrite SeparatedCommandEncryptor.GetLicense() to return valid RequesterConnectionField (NEVER null)");
            TypeDefinition? sepEnc = FindType(mod, "VolAnalysis.Cryptography.SeparatedCommandEncryptor");
            if (sepEnc == null) { Console.WriteLine("WARNING: SeparatedCommandEncryptor not found — skipping GetLicense patch"); }
            else PatchGetLicense(mod, sepEnc, ctor_str);

            Console.WriteLine("\n[4/5] PATCH 2b — Rewrite RequesterConnectionField.LicenseKey getter to return permanent string (NEVER null)");
            TypeDefinition? reqConn = FindType(mod, "VolAnalysis.Cryptography.RequesterConnectionField");
            if (reqConn == null) { Console.WriteLine("WARNING: RequesterConnectionField not found — skipping LicenseKey patch"); }
            else PatchLicenseKeyProperty(mod, reqConn, t_string);

            Console.WriteLine("\n[5/5] PATCH 3 — REMOVE [LicenseProvider(...)] custom attribute from ALL types");
            StripLicenseProviderAttributes(mod);

            Console.WriteLine("\n✍️ Writing patched assembly to disk...");
            asm.Write();
            long newLen = new FileInfo(targetExe).Length;
            Console.WriteLine($"Done. Patched EXE size = {newLen:N0} bytes. Total changes applied = {changes}");
        }

        Console.WriteLine("\nPatching complete. Run 'wine64 Deepchart.exe' from the app/ folder to launch.");
        return 0;
    }

    static TypeDefinition? FindType(ModuleDefinition mod, string fullName)
    {
        foreach (var t in mod.Types)
        {
            if (t.FullName == fullName) return t;
            foreach (var n in t.NestedTypes) if (n.FullName == fullName) return n;
        }
        return null;
    }

    static void ImportSystemPrimitives(ModuleDefinition m, out TypeReference t_bool, out TypeReference t_int, out TypeReference t_string, out TypeReference t_date, out MethodReference ctor_arg2_str)
    {
        t_bool = m.TypeSystem.Boolean;
        t_int = m.TypeSystem.Int32;
        t_string = m.TypeSystem.String;
        t_date = m.ImportReference(typeof(DateTime));
        var objType = m.TypeSystem.Object;
        ctor_arg2_str = null!; // will be set inside PatchGetLicense when we locate the RequesterConnectionField .ctor
    }

    // ==============================================================
    // PATCH 1: Rewrite ALL Status methods. Every bool returns true,
    // every int returns int.MaxValue, every string returns permanent,
    // date returns DateTime.MaxValue.AddYears(-1), Clear() preserves
    // valid state (does NOT deactivate).
    // ==============================================================
    static void PatchAllStatusMethods(ModuleDefinition mod, TypeDefinition status,
        TypeReference t_bool, TypeReference t_int, TypeReference t_string, TypeReference t_date)
    {
        foreach (var m in status.Methods)
        {
            if (m.IsConstructor || !m.HasBody) continue;

            var proc = m.Body.GetILProcessor();
            proc.Body.Instructions.Clear();
            proc.Body.Variables.Clear();

            // Static ctor .cctor: set all static fields to valid values
            if (m.Name == ".cctor")
            {
                MethodReference? wr = FindStaticMethod(status, "WrapperStreamEditor.AnalyzeRecord", mod);
                if (wr != null) proc.Emit(OpCodes.Call, wr);

                foreach (var f in status.Fields)
                {
                    if (!f.IsStatic) continue;
                    proc.Emit(OpCodes.Ldsflda, f);
                    switch (f.FieldType.FullName)
                    {
                        case "System.Boolean":
                            // for tester predicate let it stay false; the rest all true
                            if (f.Name == "isTesterPredicate") proc.Emit(OpCodes.Ldc_I4_0);
                            else if (f.Name == "singletonConfigFieldFlag") proc.Emit(OpCodes.Ldc_I4_1);
                            else proc.Emit(OpCodes.Ldc_I4_1);
                            break;
                        case "System.Int32":
                            if (f.Name == "singletonConfigFieldFlag") proc.Emit(OpCodes.Ldc_I4_1);
                            else proc.Emit(OpCodes.Ldc_I4, int.MaxValue);
                            break;
                        case "System.String":
                            if (f.Name == "detachedBuilderTitle") proc.Emit(OpCodes.Ldstr, "PERMANENT");
                            else if (f.Name == "transmitterProjectText") proc.Emit(OpCodes.Ldstr, "FULL-LICENSE");
                            else proc.Emit(OpCodes.Ldstr, "PERMANENT");
                            break;
                        case "System.DateTime":
                            {
                                MethodReference dtCtor = mod.ImportReference(typeof(DateTime).GetConstructor(new[] { typeof(long) })!);
                                MethodReference utcNow = mod.ImportReference(typeof(DateTime).GetProperty("UtcNow")!.GetGetMethod()!);
                                MethodReference addYears = mod.ImportReference(typeof(DateTime).GetMethod("AddYears", new[] { typeof(int) })!);
                                proc.Emit(OpCodes.Call, utcNow);
                                proc.Emit(OpCodes.Ldc_I4, 100);
                                proc.Emit(OpCodes.Callvirt, addYears);
                                proc.Emit(OpCodes.Ldobj, f.FieldType);
                                break;
                            }
                        default:
                            // For enum (SqlRetentionUnit) / reference types (SortedList / byte[]): default
                            proc.Emit(OpCodes.Initobj, f.FieldType);
                            break;
                    }
                    proc.Emit(OpCodes.Stobj, f.FieldType);
                }
                proc.Emit(OpCodes.Ret);
                MarkChange($".cctor static field init ({status.Fields.Count(f=>f.IsStatic)} statics)");
                continue;
            }

            // Instance ctor .ctor: just return
            if (m.Name == ".ctor")
            {
                proc.Emit(OpCodes.Ldarg_0);
                var objCtor = mod.ImportReference(typeof(object).GetConstructor(Type.EmptyTypes)!);
                proc.Emit(OpCodes.Call, objCtor);
                proc.Emit(OpCodes.Ret);
                MarkChange(".ctor simplified");
                continue;
            }

            TypeReference ret = m.ReturnType;

            // Clear() -> keep all flags in activated state
            if (m.Name == "Clear")
            {
                foreach (var f in status.Fields)
                {
                    if (!f.IsStatic) continue;
                    switch (f.FieldType.FullName)
                    {
                        case "System.Boolean":
                            if (f.Name == "isTesterPredicate") proc.Emit(OpCodes.Ldc_I4_0);
                            else if (f.Name == "singletonConfigFieldFlag") proc.Emit(OpCodes.Ldc_I4_1);
                            else proc.Emit(OpCodes.Ldc_I4_1);
                            proc.Emit(OpCodes.Stsfld, f);
                            break;
                        case "System.Int32":
                            if (f.Name == "singletonConfigFieldFlag") proc.Emit(OpCodes.Ldc_I4_1);
                            else proc.Emit(OpCodes.Ldc_I4, int.MaxValue);
                            proc.Emit(OpCodes.Stsfld, f);
                            break;
                    }
                }
                proc.Emit(OpCodes.Ret);
                MarkChange("Clear() preserves activation (sets bools=true, ints=MaxValue)");
                continue;
            }

            // Void methods: nop + ret
            if (ret.FullName == "System.Void")
            {
                proc.Emit(OpCodes.Ret);
                MarkChange($"void method {m.Name}() → empty");
                continue;
            }

            // EnsureConfigurableMap(bool,bool,bool,bool) -> long permanent string
            if (m.Name == "EnsureConfigurableMap" && ret.FullName == "System.String")
            {
                proc.Emit(OpCodes.Ldstr, "ACTIVATED-PERMANENT-FULL-FEATURES");
                proc.Emit(OpCodes.Ret);
                MarkChange("EnsureConfigurableMap() → ACTIVATED-PERMANENT-FULL-FEATURES");
                continue;
            }
            if (m.Name == "EnsureInternalSummarizer" && ret.FullName == "System.String")
            {
                proc.Emit(OpCodes.Ldstr, "PERMANENT-LICENSE-ACTIVATED-000000000000");
                proc.Emit(OpCodes.Ret);
                MarkChange("EnsureInternalSummarizer() → PERMANENT-LICENSE-ACTIVATED");
                continue;
            }
            if (m.Name == "MergeWorker" && ret.FullName == "System.String")
            {
                proc.Emit(OpCodes.Ldstr, "PERMANENT");
                proc.Emit(OpCodes.Ret);
                MarkChange("MergeWorker() → PERMANENT");
                continue;
            }
            if (m.Name == "ListenSeparatedRecommender" && ret.FullName == "System.String")
            {
                proc.Emit(OpCodes.Ldstr, "FULLY-ACTIVATED");
                proc.Emit(OpCodes.Ret);
                MarkChange("ListenSeparatedRecommender() → FULLY-ACTIVATED");
                continue;
            }

            // Any other string getter → permanent
            if (ret.FullName == "System.String")
            {
                proc.Emit(OpCodes.Ldstr, "PERMANENT");
                proc.Emit(OpCodes.Ret);
                MarkChange($"{m.Name}() string → PERMANENT");
                continue;
            }

            // Boolean checkers — all return true
            if (ret.FullName == "System.Boolean")
            {
                proc.Emit(OpCodes.Ldc_I4_1);
                proc.Emit(OpCodes.Ret);
                MarkChange($"{m.Name}() bool → true");
                continue;
            }

            // Int returners — all return int.MaxValue (unlimited)
            if (ret.FullName == "System.Int32")
            {
                proc.Emit(OpCodes.Ldc_I4, int.MaxValue);
                proc.Emit(OpCodes.Ret);
                MarkChange($"{m.Name}() int → int.MaxValue");
                continue;
            }

            // DateTime returners — 9998-12-31
            if (ret.FullName == "System.DateTime")
            {
                MethodInfo dtMaxProp = typeof(DateTime).GetProperty("MaxValue")!.GetGetMethod()!;
                MethodInfo addYears = typeof(DateTime).GetMethod("AddYears", new[] { typeof(int) })!;
                proc.Emit(OpCodes.Call, mod.ImportReference(dtMaxProp));
                proc.Emit(OpCodes.Ldc_I4_M1);
                proc.Emit(OpCodes.Call, mod.ImportReference(addYears));
                proc.Emit(OpCodes.Ret);
                MarkChange($"{m.Name}() DateTime → MaxValue.AddYears(-1)");
                continue;
            }

            // Byte array / SortedList / enum returners — leave default but return default non-null reference for array
            if (ret.IsArray)
            {
                proc.Emit(OpCodes.Ldc_I4_0);
                proc.Emit(OpCodes.Newarr, ret.GetElementType()!);
                proc.Emit(OpCodes.Ret);
                MarkChange($"{m.Name}() array → new empty array (not null)");
                continue;
            }
            if (ret.IsValueType)
            {
                var lv = new VariableDefinition(ret);
                proc.Body.Variables.Add(lv);
                proc.Emit(OpCodes.Ldloca_S, lv);
                proc.Emit(OpCodes.Initobj, ret);
                proc.Emit(OpCodes.Ldloc_0);
                proc.Emit(OpCodes.Ret);
                MarkChange($"{m.Name}() valuetype → default()");
                continue;
            }

            // Unknown ref type — return null (fallback, shouldn't hit for the methods we care about)
            proc.Emit(OpCodes.Ldnull);
            proc.Emit(OpCodes.Ret);
            MarkChange($"{m.Name}() object → null (fallback)");
        }
    }

    static MethodReference? FindStaticMethod(TypeDefinition container, string dottedPath, ModuleDefinition mod)
    {
        var parts = dottedPath.Split('.');
        string typeName = string.Join(".", parts.Take(parts.Length - 1));
        string methodName = parts.Last();
        TypeDefinition? t = FindType(mod, typeName);
        if (t == null) return null;
        foreach (var m in t.Methods) if (m.Name == methodName && m.IsStatic) return m;
        return null;
    }

    // ==============================================================
    // PATCH 2: GetLicense() — was `return null;` → return a concrete
    // new RequesterConnectionField(null, "PERMANENT-FULL-LICENSE-ACTIVATED")
    // ==============================================================
    static void PatchGetLicense(ModuleDefinition mod, TypeDefinition sepEnc, MethodReference _ignore)
    {
        MethodDefinition? getLic = sepEnc.Methods.FirstOrDefault(m => m.Name == "GetLicense");
        if (getLic == null) { Console.WriteLine("  WARNING: GetLicense method not found"); return; }
        TypeReference retType = getLic.ReturnType; // should be RequesterConnectionField or License?
        TypeDefinition? retDef = retType.Resolve();
        if (retDef == null) { Console.WriteLine($"  WARNING: cannot resolve return type {retType.FullName}"); return; }

        // Find .ctor taking (something, string) - look for best match: ctor with last param string
        MethodDefinition? ctor = retDef.Methods
            .Where(m => m.IsConstructor && m.Parameters.Count >= 1)
            .OrderByDescending(m => m.Parameters.Count(p => p.ParameterType.FullName == "System.String"))
            .FirstOrDefault();
        if (ctor == null) { Console.WriteLine("  WARNING: no RequesterConnectionField constructor found"); return; }

        var proc = getLic.Body.GetILProcessor();
        proc.Body.Instructions.Clear();
        proc.Body.Variables.Clear();

        // Build arguments: for each parameter, pass default(T) + a permanent string for last string param
        bool usedString = false;
        for (int i = ctor.Parameters.Count - 1; i >= 0; i--)
        {
            var p = ctor.Parameters[i];
            if (p.ParameterType.FullName == "System.String" && !usedString)
            {
                proc.Emit(OpCodes.Ldstr, "PERMANENT-FULL-LICENSE-ACTIVATED");
                usedString = true;
            }
            else if (p.ParameterType.IsValueType)
            {
                var lv = new VariableDefinition(p.ParameterType);
                proc.Body.Variables.Add(lv);
                proc.Emit(OpCodes.Ldloca_S, lv);
                proc.Emit(OpCodes.Initobj, p.ParameterType);
                proc.Emit(OpCodes.Ldloc_0);
            }
            else
            {
                proc.Emit(OpCodes.Ldnull);
            }
        }
        // Reverse-emitted since we walked params last-to-first to pick string-last; actually correct order from constructor perspective:
        // We need to REVERSE above emits because we walked i=last→first. Simplest: rebuild cleanly below.

        proc.Body.Instructions.Clear();
        proc.Body.Variables.Clear();
        usedString = false;
        // emit params first → last
        foreach (var p in ctor.Parameters)
        {
            bool isLast = (p == ctor.Parameters.Last());
            if (p.ParameterType.FullName == "System.String" && (isLast || !usedString))
            {
                proc.Emit(OpCodes.Ldstr, "PERMANENT-FULL-LICENSE-ACTIVATED");
                usedString = true;
            }
            else if (p.ParameterType.IsValueType)
            {
                var lv = new VariableDefinition(p.ParameterType);
                proc.Body.Variables.Add(lv);
                proc.Emit(OpCodes.Ldloca_S, lv);
                proc.Emit(OpCodes.Initobj, p.ParameterType);
                proc.Emit(OpCodes.Ldloc, lv);
            }
            else
            {
                proc.Emit(OpCodes.Ldnull);
            }
        }
        proc.Emit(OpCodes.Newobj, ctor);
        proc.Emit(OpCodes.Ret);
        MarkChange($"SeparatedCommandEncryptor.GetLicense() → new {retDef.Name}({ctor.Parameters.Count})(\"PERMANENT-FULL-LICENSE-ACTIVATED\")");
    }

    // ==============================================================
    // PATCH 2b: RequesterConnectionField.LicenseKey getter → permanent string
    // ==============================================================
    static void PatchLicenseKeyProperty(ModuleDefinition mod, TypeDefinition rcf, TypeReference t_string)
    {
        foreach (var prop in rcf.Properties)
        {
            if (prop.Name == "LicenseKey" && prop.PropertyType.FullName == "System.String")
            {
                var getter = prop.GetMethod;
                if (getter == null) continue;
                var proc = getter.Body.GetILProcessor();
                proc.Body.Instructions.Clear();
                proc.Body.Variables.Clear();
                proc.Emit(OpCodes.Ldstr, "PERMANENT-FULL-LICENSE-KEY-0000000000000000");
                proc.Emit(OpCodes.Ret);
                MarkChange("RequesterConnectionField.LicenseKey → permanent key string");
                return;
            }
        }
        // fallback: any method named get_LicenseKey
        foreach (var m in rcf.Methods)
            if (m.Name == "get_LicenseKey" && m.ReturnType.FullName == "System.String")
            {
                var proc = m.Body.GetILProcessor();
                proc.Body.Instructions.Clear();
                proc.Emit(OpCodes.Ldstr, "PERMANENT-FULL-LICENSE-KEY-0000000000000000");
                proc.Emit(OpCodes.Ret);
                MarkChange("get_LicenseKey() → permanent key string");
            }
    }

    // ==============================================================
    // PATCH 3: Strip [LicenseProvider(...)] custom attributes from every type
    // Also strip them from any Assembly-level attributes
    // ==============================================================
    static void StripLicenseProviderAttributes(ModuleDefinition mod)
    {
        int stripped = 0;
        bool IsLic(CustomAttribute c)
            => c.AttributeType.FullName.Contains("LicenseProviderAttribute") ||
               c.AttributeType.FullName.Contains("LicenseProvider");

        // Assembly-level
        for (int i = mod.Assembly.CustomAttributes.Count - 1; i >= 0; i--)
            if (IsLic(mod.Assembly.CustomAttributes[i])) { mod.Assembly.CustomAttributes.RemoveAt(i); stripped++; }
        // Module-level
        for (int i = mod.CustomAttributes.Count - 1; i >= 0; i--)
            if (IsLic(mod.CustomAttributes[i])) { mod.CustomAttributes.RemoveAt(i); stripped++; }
        // Every type (+ nested types)
        void CheckType(TypeDefinition t)
        {
            for (int i = t.CustomAttributes.Count - 1; i >= 0; i--)
                if (IsLic(t.CustomAttributes[i])) { t.CustomAttributes.RemoveAt(i); stripped++; }
            foreach (var m in t.Methods)
                for (int i = m.CustomAttributes.Count - 1; i >= 0; i--)
                    if (IsLic(m.CustomAttributes[i])) { m.CustomAttributes.RemoveAt(i); stripped++; }
            foreach (var p in t.Properties)
                for (int i = p.CustomAttributes.Count - 1; i >= 0; i--)
                    if (IsLic(p.CustomAttributes[i])) { p.CustomAttributes.RemoveAt(i); stripped++; }
            foreach (var f in t.Fields)
                for (int i = f.CustomAttributes.Count - 1; i >= 0; i--)
                    if (IsLic(f.CustomAttributes[i])) { f.CustomAttributes.RemoveAt(i); stripped++; }
            foreach (var n in t.NestedTypes) CheckType(n);
        }
        foreach (var t in mod.Types) CheckType(t);

        if (stripped > 0)
        {
            changes += stripped;
            Console.WriteLine($"  Stripped {stripped} [LicenseProvider(...)] attributes from assembly/types/methods.");
        }
        else
        {
            Console.WriteLine("  No [LicenseProvider] attributes found in assembly — already clean.");
        }
    }

    static void MarkChange(string msg)
    {
        changes++;
        Console.WriteLine($"  ✔ {msg}");
    }
}

// Assembly resolver: also looks next to target EXE for DLLs
class CustomResolver : IAssemblyResolver
{
    readonly string dir;
    readonly DefaultAssemblyResolver fall = new();

    public CustomResolver(string dir) { this.dir = dir; }

    public AssemblyDefinition Resolve(AssemblyNameReference name)
    {
        string p = Path.Combine(dir, name.Name + ".dll");
        if (File.Exists(p))
        {
            try { return AssemblyDefinition.ReadAssembly(p, new ReaderParameters { AssemblyResolver = this, ReadSymbols = false }); }
            catch { }
        }
        // fallback to asm_*.dll naming (Deepchart renames DLLs)
        foreach (var f in Directory.EnumerateFiles(dir, "*.dll"))
        {
            try
            {
                var ad = AssemblyDefinition.ReadAssembly(f, new ReaderParameters { AssemblyResolver = this, ReadSymbols = false });
                if (ad.Name.Name == name.Name) return ad;
            }
            catch { }
        }
        return fall.Resolve(name);
    }
    public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters) => Resolve(name);
    public void Dispose() { fall.Dispose(); }
}

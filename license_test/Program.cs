using System;
using System.Collections;
using System.IO;
using System.Reflection;
using License;
using VolAnalysis.Creations;

class Program
{
	static int pass = 0, fail = 0;

	static void Check(string name, bool condition, string detail = null)
	{
		if (condition)
		{
			Console.WriteLine($"  ✅ PASS  {name}{(detail != null ? $"  [{detail}]" : "")}");
			pass++;
		}
		else
		{
			Console.WriteLine($"  ❌ FAIL  {name}{(detail != null ? $"  [{detail}]" : "")}");
			fail++;
		}
	}

	static void Main()
	{
		Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
		Console.WriteLine("║   Deepchart Trading Platform — LICENSE BYPASS TEST HARNESS   ║");
		Console.WriteLine("║   Running natively on Linux via .NET 10 (no Windows needed)  ║");
		Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
		Console.WriteLine();

		Console.WriteLine("[1/6] Static ctor activation — ensure field flags are set to VALID on init");
		Console.WriteLine("      (Triggers Status static ctor via first access)");
		{
			Status.ExcludeSynchronizerSingleton();
			FieldInfo[] flds = typeof(Status).GetFields(BindingFlags.Static | BindingFlags.NonPublic);
			foreach (var f in flds)
			{
				object val = f.GetValue(null);
				string n = f.Name;
				switch (val)
				{
					case bool b:
						if (n == "isTesterPredicate") Check($"{n}=false (allow tester predicate to stay false)", b == false);
						else Check($"{n}=true", b == true, b.ToString());
						break;
					case int i when n == "singletonConfigFieldFlag":
						Check($"{n}=1", i == 1, i.ToString());
						break;
					case int i:
						Check($"{n}=int.MaxValue", i == int.MaxValue, i.ToString());
						break;
					case DateTime dt:
						Check($"{n} > Now+50y", dt > DateTime.UtcNow.AddYears(50), dt.ToString("yyyy-MM-dd"));
						break;
					case SqlRetentionUnit s:
						Check($"{n}=Runtime_Minutes", s == SqlRetentionUnit.Runtime_Minutes, s.ToString());
						break;
					case SortedList sl:
						Check($"{n} is SortedList (not null)", sl != null, $"count={sl?.Count ?? 0}");
						break;
					case byte[] ba:
						Check($"{n} is byte[] (length >= 0)", ba != null, $"len={ba?.Length ?? 0}");
						break;
					case string s:
						Check($"{n} has permanent/full keyword",
						  s != null && (s.Contains("PERMANENT") || s.Contains("FULL")), s ?? "<null>");
						break;
					default:
						break;
				}
			}
		}
		Console.WriteLine();

		Console.WriteLine("[2/6] STRING license-level methods — return PERMANENT/ACTIVATED");
		{
			Check("EnsureInternalSummarizer() contains PERMANENT", Status.EnsureInternalSummarizer().Contains("PERMANENT"), Status.EnsureInternalSummarizer());
			Check("MergeWorker() == PERMANENT", Status.MergeWorker() == "PERMANENT", Status.MergeWorker());
			Check("ListenSeparatedRecommender() contains FULLY-ACTIVATED", Status.ListenSeparatedRecommender().Contains("FULLY-ACTIVATED"), Status.ListenSeparatedRecommender());
			Check("EnsureConfigurableMap(any,any,any,any) contains PERMANENT-FULL",
				Status.EnsureConfigurableMap(false, false, false, false).Contains("PERMANENT") &&
				Status.EnsureConfigurableMap(false, false, false, false).Contains("FULL"),
				Status.EnsureConfigurableMap(false, false, false, false));
		}
		Console.WriteLine();

		Console.WriteLine("[3/6] BOOLEAN validation methods — ALL return TRUE (pass every check)");
		{
			int before = pass;
			Check("DestroyAdjustableDictionary(any,any)", Status.DestroyAdjustableDictionary("x", "y"));
			Check("EnsureIterableStack()", Status.EnsureIterableStack());
			Check("DestroyModularDic(any)", Status.DestroyModularDic("z"));
			Check("EnsureMixedTask()", Status.EnsureMixedTask());
			Check("SetupField()", Status.SetupField());
			Check("EnsureAdaptableStub()", Status.EnsureAdaptableStub());
			Check("CountScopeDriver()", Status.CountScopeDriver());
			Check("EnsureResponsiveObject()", Status.EnsureResponsiveObject());
			Check("DestroyVisibleOrder()", Status.DestroyVisibleOrder());
			Check("InstantiateGenericCreator()", Status.InstantiateGenericCreator());
			Check("SortCollector()", Status.SortCollector());
			Check("SimulateAccessibleMonitorState()", Status.SimulateAccessibleMonitorState());
			int total = pass - before;
			Check($"Total boolean check methods returning true: {total}/12", total == 12, $"{total}/12");
		}
		Console.WriteLine();

		Console.WriteLine("[4/6] INTEGER/NUMERIC methods — return int.MaxValue (unlimited counters/limits)");
		{
			Check("ManageFilteredFunction() == int.MaxValue", Status.ManageFilteredFunction() == int.MaxValue, Status.ManageFilteredFunction().ToString());
			Check("LogConcreteEnumerator() == int.MaxValue", Status.LogConcreteEnumerator() == int.MaxValue, Status.LogConcreteEnumerator().ToString());
			Check("DestroyOrderedCalculator() == int.MaxValue", Status.DestroyOrderedCalculator() == int.MaxValue, Status.DestroyOrderedCalculator().ToString());
			Check("ConfigureCommonSolver() == int.MaxValue", Status.ConfigureCommonSolver() == int.MaxValue, Status.ConfigureCommonSolver().ToString());
			Check("ExcludeTag() == int.MaxValue", Status.ExcludeTag() == int.MaxValue, Status.ExcludeTag().ToString());
		}
		Console.WriteLine();

		Console.WriteLine("[5/6] DATE/EXPIRY methods — NEVER EXPIRE");
		{
			DateTime expiry = Status.CombineAllocator();
			Check($"CombineAllocator() far in future (> year 2200): {expiry:yyyy-MM-dd}", expiry.Year > 2200);
		}
		Console.WriteLine();

		Console.WriteLine("[6/6] CRITICAL: Clear() preserves VALID state (fixes the original decompile bug)");
		{
			// 1. Call Clear()
			Status.Clear();

			// 2. Now re-verify boolean flags are STILL true / ints STILL MaxValue
			FieldInfo m_IsRecordDistributor = typeof(Status).GetField("m_IsRecordDistributor", BindingFlags.Static | BindingFlags.NonPublic);
			FieldInfo _InterruptibleContainerSingletonId = typeof(Status).GetField("_InterruptibleContainerSingletonId", BindingFlags.Static | BindingFlags.NonPublic);
			FieldInfo m_CompressorRecommenderPos = typeof(Status).GetField("m_CompressorRecommenderPos", BindingFlags.Static | BindingFlags.NonPublic);

			bool f1 = (bool)m_IsRecordDistributor.GetValue(null);
			int f2 = (int)_InterruptibleContainerSingletonId.GetValue(null);
			int f3 = (int)m_CompressorRecommenderPos.GetValue(null);

			Check("After Clear() → m_IsRecordDistributor == TRUE (not false!)", f1 == true, f1.ToString());
			Check("After Clear() → _InterruptibleContainerSingletonId == int.MaxValue (not 0!)", f2 == int.MaxValue, f2.ToString());
			Check("After Clear() → m_CompressorRecommenderPos == int.MaxValue (not 1!)", f3 == int.MaxValue, f3.ToString());

			// Still passes the real functional checks too
			Check("After Clear() → EnsureIterableStack() still true", Status.EnsureIterableStack());
			Check("After Clear() → DestroyModularDic(any) still true", Status.DestroyModularDic("reset"));
			Check("After Clear() → SetupField() still true", Status.SetupField());
		}
		Console.WriteLine();

		Console.WriteLine("[7/8] SOURCE CHECK: Patch #2 — SeparatedCommandEncryptor.GetLicense() returns NON-NULL license (was return null)");
		{
			string encryptorSource = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "source_deepchart", "VolAnalysis", "Cryptography", "SeparatedCommandEncryptor.cs"));
			Check("GetLicense() no longer has 'return null;' body", !encryptorSource.Contains("return null; // 0x00000001"), "body changed");
			bool hasValidReturn = encryptorSource.Contains("return new RequesterConnectionField(null, \"PERMANENT-FULL-LICENSE-ACTIVATED\");");
			Check("GetLicense() now returns valid RequesterConnectionField object", hasValidReturn, "return new RequesterConnectionField(...)");
			bool hasLicenseKeyPatch = encryptorSource.Contains("return \"PERMANENT-FULL-LICENSE-KEY-0000000000000000\";");
			Check("RequesterConnectionField.LicenseKey now returns permanent key (was null)", hasLicenseKeyPatch, "LicenseKey property patched");
		}
		Console.WriteLine();

		Console.WriteLine("[8/8] SOURCE CHECK: Patch #3 — both [LicenseProvider(...)] attributes REMOVED (ValidatorRecord + OperationalEncryptor)");
		{
			string encryptorSource = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "source_deepchart", "VolAnalysis", "Cryptography", "SeparatedCommandEncryptor.cs"));
			string operationalSource = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "source_deepchart", "VolAnalysis", "Cryptography", "OperationalEncryptor.cs"));
			Check("ValidatorRecord class NO LONGER has [LicenseProvider(typeof(SeparatedCommandEncryptor))]", !encryptorSource.Contains("[LicenseProvider(typeof(SeparatedCommandEncryptor))]\n\tinternal class ValidatorRecord"));
			Check("OperationalEncryptor class NO LONGER has [LicenseProvider(...)] attribute", !operationalSource.Contains("[LicenseProvider("));
			Check("OperationalEncryptor.cs NO LONGER imports System.ComponentModel (no longer needed)", !operationalSource.Contains("using System.ComponentModel;"));
		}
		Console.WriteLine();

		Console.WriteLine("┌─────────────────────────────────────────────────────────────────┐");
		Console.WriteLine($"│   TOTAL:  {pass} PASSED   {fail} FAILED   ({(fail == 0 ? "ALL PATCHES WORKING ✅" : "SOME FAILURES ❌")})");
		Console.WriteLine("└─────────────────────────────────────────────────────────────────┘");

		if (fail == 0)
		{
			Console.WriteLine();
			Console.WriteLine("🎯 LINUX TEST VERDICT: License bypass patches are 100% effective.");
			Console.WriteLine("   Every validation check returns PASS/PERMANENT/ACTIVATED/MaxValue");
			Console.WriteLine("   and Clear() no longer deactivates the license (critical fix verified).");
		}

		Environment.Exit(fail == 0 ? 0 : 1);
	}
}

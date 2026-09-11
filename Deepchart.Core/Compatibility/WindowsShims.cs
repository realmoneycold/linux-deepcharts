using System;
using System.Linq;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
    public static class ProtectedData
    {
        private static readonly byte[] s_machineKey = new byte[] { 0xDE,0xAD,0xBE,0xEF,0x00,0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88,0x99,0xAA,0xBB };
        private static readonly byte[] s_defaultEntropy = new byte[] { 0x5F,0x4C,0x3B,0x2A };

        public static DataProtectionScope CurrentUser => DataProtectionScope.CurrentUser;
        public static byte[] Protect(byte[] userData, byte[]? optionalEntropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            byte[] entropy = optionalEntropy ?? s_defaultEntropy;
            using var aes = Aes.Create();
            aes.Key = DeriveKey(s_machineKey, entropy, 32);
            aes.IV = DeriveKey(s_machineKey, entropy.Reverse().Concat(entropy).ToArray(), 16);
            using var ms = new System.IO.MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)) { cs.Write(userData, 0, userData.Length); }
            return ms.ToArray();
        }
        public static byte[] Unprotect(byte[] encryptedData, byte[]? optionalEntropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            byte[] entropy = optionalEntropy ?? s_defaultEntropy;
            using var aes = Aes.Create();
            aes.Key = DeriveKey(s_machineKey, entropy, 32);
            aes.IV = DeriveKey(s_machineKey, entropy.Reverse().Concat(entropy).ToArray(), 16);
            using var ms = new System.IO.MemoryStream(encryptedData);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var result = new System.IO.MemoryStream();
            cs.CopyTo(result);
            return result.ToArray();
        }
        private static byte[] DeriveKey(byte[] master, byte[] salt, int bytes)
        {
            using var rfc = new Rfc2898DeriveBytes(master, salt, 1000, HashAlgorithmName.SHA256);
            return rfc.GetBytes(bytes);
        }
    }
    public enum DataProtectionScope { CurrentUser = 0, LocalMachine = 1 }
}

namespace System.Diagnostics
{
    public class PerformanceCounter : IDisposable
    {
        public string CategoryName { get; set; } = "";
        public string CounterName { get; set; } = "";
        public string InstanceName { get; set; } = "";
        public string MachineName { get; set; } = ".";
        public float RawValue { get; set; } = 0;
        public bool ReadOnly { get; set; } = true;
        public PerformanceCounter() {}
        public PerformanceCounter(string cat, string counter) { CategoryName=cat; CounterName=counter; }
        public PerformanceCounter(string cat, string counter, string inst) { CategoryName=cat; CounterName=counter; InstanceName=inst; }
        public PerformanceCounter(string cat, string counter, string inst, bool ro) { CategoryName=cat; CounterName=counter; InstanceName=inst; ReadOnly=ro; }
        public float NextValue() => RawValue;
        public CounterSample NextSample() => default;
        public void Close() {}
        public void Dispose() {}
    }
    public struct CounterSample { public long RawValue; public long BaseValue; public long CounterFrequency; public long SystemFrequency; public long TimeStamp; public long TimeStamp100nSec; public int CounterType; public int CounterTimeStamp; public static float Calculate(CounterSample a) => 0; public static float Calculate(CounterSample a, CounterSample b) => 0; }
    public class PerformanceCounterCategory
    {
        public PerformanceCounterCategory() {}
        public PerformanceCounterCategory(string name) { CategoryName=name; }
        public string CategoryName { get; set; } = "";
        public string MachineName { get; set; } = ".";
        public bool Exists(string name) => false;
        public static PerformanceCounterCategory[] GetCategories() => Array.Empty<PerformanceCounterCategory>();
        public static PerformanceCounterCategory[] GetCategories(string machine) => Array.Empty<PerformanceCounterCategory>();
    }
}

namespace System.Management
{
    public class ManagementObject : IDisposable
    {
        public ManagementObject() {}
        public ManagementObject(string path) {}
        public ManagementObject(ManagementPath path) {}
        public object this[string propertyName] { get => GetPropertyValue(propertyName); set {} }
        public object GetPropertyValue(string p) => null;
        public void Dispose() {}
    }
    public class ManagementPath
    {
        public ManagementPath() {}
        public ManagementPath(string path) { Path = path; }
        public string Path { get; set; } = "";
        public string Server { get; set; } = "";
        public string NamespacePath { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string RelativePath { get; set; } = "";
    }
    public class ManagementClass : IDisposable
    {
        public ManagementClass() {}
        public ManagementClass(string path) {}
        public ManagementObjectCollection GetInstances() => new ManagementObjectCollection();
        public void Dispose() {}
    }
    public class ManagementObjectCollection : IDisposable, System.Collections.IEnumerable
    {
        public System.Collections.IEnumerator GetEnumerator() { yield break; }
        public void Dispose() {}
    }
    public class ManagementObjectSearcher : IDisposable
    {
        public ManagementObjectSearcher() {}
        public ManagementObjectSearcher(string query) { Query = new ObjectQuery(query); }
        public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query) { }
        public ObjectQuery Query { get; set; } = new ObjectQuery();
        public ManagementScope Scope { get; set; } = new ManagementScope();
        public ManagementObjectCollection Get() => new ManagementObjectCollection();
        public void Dispose() {}
    }
    public class ObjectQuery
    {
        public ObjectQuery() {}
        public ObjectQuery(string query) { QueryString = query; }
        public string QueryString { get; set; } = "";
    }
    public class ManagementScope
    {
        public ManagementScope() {}
        public ManagementScope(string path) { Path = new ManagementPath(path); }
        public ManagementPath Path { get; set; } = new ManagementPath();
        public ConnectionOptions Options { get; set; } = new ConnectionOptions();
        public void Connect() {}
    }
    public class ConnectionOptions
    {
        public ConnectionOptions() {}
        public string Username { get; set; }
        public string Password { get; set; }
        public string Authority { get; set; }
        public AuthenticationLevel Authentication { get; set; } = AuthenticationLevel.Default;
        public ImpersonationLevel Impersonation { get; set; } = ImpersonationLevel.Impersonate;
        public int Timeout { get; set; } = (int)TimeSpan.FromSeconds(30).TotalSeconds;
    }
    public enum AuthenticationLevel { Unchanged=-1, Default=0, None=1, Connect=2, Call=3, Packet=4, PacketIntegrity=5, PacketPrivacy=6 }
    public enum ImpersonationLevel { Default=0, Anonymous=1, Identify=2, Impersonate=3, Delegate=4 }
    public class ManagementBaseObject : IDisposable { public virtual void Dispose() {} public object this[string p] { get => null; set {} } }
    public class PropertyDataCollection { public int Count => 0; }
    public class QualifierDataCollection { public int Count => 0; }
    public class PropertyData { public string Name { get; set; } = ""; public object Value { get; set; } public bool IsLocal { get; set; } }
}

namespace System.Drawing
{
    using System.Globalization;
    [StructLayout(LayoutKind.Sequential)]
    public struct Color : IEquatable<Color>
    {
        private long _value;
        public byte A => (byte)((_value >> 24) & 0xFF);
        public byte R => (byte)((_value >> 16) & 0xFF);
        public byte G => (byte)((_value >> 8) & 0xFF);
        public byte B => (byte)(_value & 0xFF);
        public bool IsEmpty => _value == 0;
        public bool IsKnownColor => false;
        public bool IsNamedColor => false;
        public bool IsSystemColor => false;
        public string Name => ToString();
        public static Color Empty => default;
        public static Color FromArgb(int a, int r, int g, int b) { Color c = new Color(); c._value = (uint)(a << 24 | r << 16 | g << 8 | b); return c; }
        public static Color FromArgb(int r, int g, int b) { return FromArgb(255, r, g, b); }
        public static Color FromArgb(int a, Color c) { return FromArgb(a, c.R, c.G, c.B); }
        public static Color FromKnownColor(KnownColor k) { return FromArgb(0,0,0,0); }
        public static Color FromName(string name) { return FromArgb(0,0,0,0); }
        public float GetBrightness() => 0f;
        public float GetHue() => 0f;
        public float GetSaturation() => 0f;
        public int ToArgb() => (int)_value;
        public bool Equals(Color other) => _value == other._value;
        public override bool Equals(object obj) => obj is Color c && Equals(c);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => $"Color [A={A}, R={R}, G={G}, B={B}]";
        public static bool operator ==(Color a, Color b) => a.Equals(b);
        public static bool operator !=(Color a, Color b) => !a.Equals(b);
    }
    public enum KnownColor { ActiveBorder=1, ActiveCaption=2, AppWorkspace=3, Control=4, ControlDark=5, ControlDarkDark=6, ControlLight=7, ControlLightLight=8, ControlText=9, Desktop=10, GrayText=11, Highlight=12, HighlightText=13, HotTrack=14, InactiveBorder=15, InactiveCaption=16, InactiveCaptionText=17, Info=18, InfoText=19, Menu=20, MenuText=21, ScrollBar=22, Window=23, WindowFrame=24, WindowText=25, Transparent=27, AliceBlue=28, AntiqueWhite=29, Aqua=30, Aquamarine=31, Azure=32, Beige=33, Bisque=34, Black=35, BlanchedAlmond=36, Blue=37, BlueViolet=38, Brown=39, BurlyWood=40, CadetBlue=41, Chartreuse=42, Chocolate=43, Coral=44, CornflowerBlue=45, Cornsilk=46, Crimson=47, Cyan=48, DarkBlue=49, DarkCyan=50, DarkGoldenrod=51, DarkGray=52, DarkGreen=53, DarkKhaki=54, DarkMagenta=55, DarkOliveGreen=56, DarkOrange=57, DarkOrchid=58, DarkRed=59, DarkSalmon=60, DarkSeaGreen=61, DarkSlateBlue=62, DarkSlateGray=63, DarkTurquoise=64, DarkViolet=65, DeepPink=66, DeepSkyBlue=67, DimGray=68, DodgerBlue=69, Firebrick=70, FloralWhite=71, ForestGreen=72, Fuchsia=73, Gainsboro=74, GhostWhite=75, Gold=76, Goldenrod=77, Gray=78, Green=79, GreenYellow=80, Honeydew=81, HotPink=82, IndianRed=83, Indigo=84, Ivory=85, Khaki=86, Lavender=87, LavenderBlush=88, LawnGreen=89, LemonChiffon=90, LightBlue=91, LightCoral=92, LightCyan=93, LightGoldenrodYellow=94, LightGray=95, LightGreen=96, LightPink=97, LightSalmon=98, LightSeaGreen=99, LightSkyBlue=100, LightSlateGray=101, LightSteelBlue=102, LightYellow=103, Lime=104, LimeGreen=105, Linen=106, Magenta=107, Maroon=108, MediumAquamarine=109, MediumBlue=110, MediumOrchid=111, MediumPurple=112, MediumSeaGreen=113, MediumSlateBlue=114, MediumSpringGreen=115, MediumTurquoise=116, MediumVioletRed=117, MidnightBlue=118, MintCream=119, MistyRose=120, Moccasin=121, NavajoWhite=122, Navy=123, OldLace=124, Olive=125, OliveDrab=126, Orange=127, OrangeRed=128, Orchid=129, PaleGoldenrod=130, PaleGreen=131, PaleTurquoise=132, PaleVioletRed=133, PapayaWhip=134, PeachPuff=135, Peru=136, Pink=137, Plum=138, PowderBlue=139, Purple=140, Red=141, RosyBrown=142, RoyalBlue=143, SaddleBrown=144, Salmon=145, SandyBrown=146, SeaGreen=147, SeaShell=148, Sienna=149, Silver=150, SkyBlue=151, SlateBlue=152, SlateGray=153, Snow=154, SpringGreen=155, SteelBlue=156, Tan=157, Teal=158, Thistle=159, Tomato=160, Turquoise=161, Violet=162, Wheat=163, White=164, WhiteSmoke=165, Yellow=166, YellowGreen=167 }
    public struct Point : IEquatable<Point>
    {
        public int X, Y;
        public Point(int x, int y) { X=x; Y=y; }
        public Point(Size sz) { X=sz.Width; Y=sz.Height; }
        public bool IsEmpty => X==0 && Y==0;
        public static readonly Point Empty = default;
        public static Point operator +(Point p, Size s) => new Point(p.X+s.Width, p.Y+s.Height);
        public static Point operator -(Point p, Size s) => new Point(p.X-s.Width, p.Y-s.Height);
        public static implicit operator PointF(Point p) => new PointF(p.X, p.Y);
        public bool Equals(Point other) => X==other.X && Y==other.Y;
        public override bool Equals(object o) => o is Point p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(X,Y);
    }
    public struct PointF : IEquatable<PointF>
    {
        public float X, Y;
        public PointF(float x, float y) { X=x; Y=y; }
        public bool IsEmpty => X==0f && Y==0f;
        public static readonly PointF Empty = default;
        public static PointF operator +(PointF p, SizeF s) => new PointF(p.X+s.Width, p.Y+s.Height);
        public static PointF operator -(PointF p, SizeF s) => new PointF(p.X-s.Width, p.Y-s.Height);
        public bool Equals(PointF other) => X==other.X && Y==other.Y;
        public override bool Equals(object o) => o is PointF p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(X,Y);
    }
    public struct Size : IEquatable<Size>
    {
        public int Width, Height;
        public Size(int w, int h) { Width=w; Height=h; }
        public Size(Point p) { Width=p.X; Height=p.Y; }
        public bool IsEmpty => Width==0 && Height==0;
        public static readonly Size Empty = default;
        public static Size operator +(Size a, Size b) => new Size(a.Width+b.Width, a.Height+b.Height);
        public static Size operator -(Size a, Size b) => new Size(a.Width-b.Width, a.Height-b.Height);
        public static explicit operator Point(Size s) => new Point(s.Width, s.Height);
        public bool Equals(Size other) => Width==other.Width && Height==other.Height;
        public override bool Equals(object o) => o is Size s && Equals(s);
        public override int GetHashCode() => HashCode.Combine(Width,Height);
    }
    public struct SizeF : IEquatable<SizeF>
    {
        public float Width, Height;
        public SizeF(float w, float h) { Width=w; Height=h; }
        public SizeF(PointF p) { Width=p.X; Height=p.Y; }
        public bool IsEmpty => Width==0f && Height==0f;
        public static readonly SizeF Empty = default;
        public static SizeF operator +(SizeF a, SizeF b) => new SizeF(a.Width+b.Width, a.Height+b.Height);
        public static SizeF operator -(SizeF a, SizeF b) => new SizeF(a.Width-b.Width, a.Height-b.Height);
        public bool Equals(SizeF other) => Width==other.Width && Height==other.Height;
        public override bool Equals(object o) => o is SizeF s && Equals(s);
        public override int GetHashCode() => HashCode.Combine(Width,Height);
    }
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X, Y, Width, Height;
        public Rectangle(int x, int y, int w, int h) { X=x; Y=y; Width=w; Height=h; }
        public Rectangle(Point loc, Size sz) { X=loc.X; Y=loc.Y; Width=sz.Width; Height=sz.Height; }
        public int Left => X;
        public int Top => Y;
        public int Right => X+Width;
        public int Bottom => Y+Height;
        public Point Location { get => new Point(X,Y); set { X=value.X; Y=value.Y; } }
        public Size Size { get => new Size(Width,Height); set { Width=value.Width; Height=value.Height; } }
        public bool IsEmpty => Width==0 && Height==0 && X==0 && Y==0;
        public static readonly Rectangle Empty = default;
        public void Inflate(int w, int h) { X-=w; Y-=h; Width+=2*w; Height+=2*h; }
        public static Rectangle Inflate(Rectangle r, int w, int h) { r.Inflate(w,h); return r; }
        public bool Contains(int x, int y) => x>=Left && x<Right && y>=Top && y<Bottom;
        public bool Contains(Point p) => Contains(p.X,p.Y);
        public bool Contains(Rectangle r) => r.Left>=Left && r.Right<=Right && r.Top>=Top && r.Bottom<=Bottom;
        public void Offset(int x, int y) { X+=x; Y+=y; }
        public static Rectangle FromLTRB(int l, int t, int r, int b) => new Rectangle(l,t,r-l,b-t);
        public static Rectangle Intersect(Rectangle a, Rectangle b) { int l=Math.Max(a.Left,b.Left); int t=Math.Max(a.Top,b.Top); int r=Math.Min(a.Right,b.Right); int b2=Math.Min(a.Bottom,b.Bottom); if(r>=l && b2>=t) return FromLTRB(l,t,r,b2); return Empty; }
        public static Rectangle Union(Rectangle a, Rectangle b) { int l=Math.Min(a.Left,b.Left); int t=Math.Min(a.Top,b.Top); int r=Math.Max(a.Right,b.Right); int b2=Math.Max(a.Bottom,b.Bottom); return FromLTRB(l,t,r,b2); }
        public bool Equals(Rectangle other) => X==other.X && Y==other.Y && Width==other.Width && Height==other.Height;
        public override bool Equals(object o) => o is Rectangle r && Equals(r);
        public override int GetHashCode() => HashCode.Combine(X,Y,Width,Height);
    }
    public struct RectangleF : IEquatable<RectangleF>
    {
        public float X, Y, Width, Height;
        public RectangleF(float x, float y, float w, float h) { X=x; Y=y; Width=w; Height=h; }
        public RectangleF(PointF loc, SizeF sz) { X=loc.X; Y=loc.Y; Width=sz.Width; Height=sz.Height; }
        public float Left => X;
        public float Top => Y;
        public float Right => X+Width;
        public float Bottom => Y+Height;
        public PointF Location { get => new PointF(X,Y); set { X=value.X; Y=value.Y; } }
        public SizeF Size { get => new SizeF(Width,Height); set { Width=value.Width; Height=value.Height; } }
        public bool IsEmpty => Width==0f && Height==0f && X==0f && Y==0f;
        public static readonly RectangleF Empty = default;
        public bool Contains(float x, float y) => x>=Left && x<Right && y>=Top && y<Bottom;
        public bool Contains(PointF p) => Contains(p.X,p.Y);
        public bool Contains(RectangleF r) => r.Left>=Left && r.Right<=Right && r.Top>=Top && r.Bottom<=Bottom;
        public static RectangleF FromLTRB(float l, float t, float r, float b) => new RectangleF(l,t,r-l,b-t);
        public bool Equals(RectangleF other) => X==other.X && Y==other.Y && Width==other.Width && Height==other.Height;
        public override bool Equals(object o) => o is RectangleF r && Equals(r);
        public override int GetHashCode() => HashCode.Combine(X,Y,Width,Height);
    }
    public static class SystemIcons { public static Icon Application => null; public static Icon Asterisk => null; public static Icon Error => null; public static Icon Exclamation => null; public static Icon Hand => null; public static Icon Information => null; public static Icon Question => null; public static Icon Shield => null; public static Icon Warning => null; public static Icon WinLogo => null; }
    public class Icon : IDisposable { public void Dispose() {} public int Height => 0; public int Width => 0; }
    public class Bitmap : IDisposable { public int Width => 0; public int Height => 0; public Bitmap(string fn) {} public Bitmap(System.IO.Stream s) {} public Bitmap(int w, int h) {} public Bitmap(System.Drawing.Image src) {} public void Dispose() {} public Color GetPixel(int x, int y) => Color.Empty; public void SetPixel(int x, int y, Color c) {} public void Save(string fn) {} public void Save(System.IO.Stream s, Imaging.ImageFormat f) {} }
    public class Image : IDisposable { public int Width => 0; public int Height => 0; public void Dispose() {} }
    namespace Imaging { public class ImageFormat { public static ImageFormat Bmp => new ImageFormat(); public static ImageFormat Gif => new ImageFormat(); public static ImageFormat Jpeg => new ImageFormat(); public static ImageFormat Png => new ImageFormat(); public static ImageFormat Tiff => new ImageFormat(); public static ImageFormat Wmf => new ImageFormat(); public static ImageFormat Emf => new ImageFormat(); public static ImageFormat Icon => new ImageFormat(); public static ImageFormat MemoryBmp => new ImageFormat(); } }
    public class Graphics : IDisposable
    {
        public static Graphics FromImage(Image img) => new Graphics();
        public void Dispose() {}
        public void Clear(Color c) {}
        public void DrawLine(Pen p, int x1, int y1, int x2, int y2) {}
        public void DrawRectangle(Pen p, int x, int y, int w, int h) {}
        public void FillRectangle(Brush b, int x, int y, int w, int h) {}
        public void DrawString(string s, Font font, Brush brush, float x, float y) {}
    }
    public class Pen : IDisposable { public Pen(Color c) {} public Pen(Color c, float w) {} public void Dispose() {} public float Width { get; set; } public Color Color { get; set; } }
    public class Brush : IDisposable { public void Dispose() {} }
    public class SolidBrush : Brush { public SolidBrush(Color c) { Color = c; } public Color Color { get; set; } }
    public class Font : IDisposable { public Font(string family, float size) { FontFamily = new FontFamily(family); Size = size; } public Font(FontFamily family, float size) { FontFamily = family; Size = size; } public void Dispose() {} public FontFamily FontFamily { get; } public float Size { get; } public FontStyle Style { get; set; } }
    public class FontFamily : IDisposable { public FontFamily(string name) { Name = name; } public void Dispose() {} public string Name { get; } }
    [Flags] public enum FontStyle { Regular=0, Bold=1, Italic=2, Underline=4, Strikeout=8 }
    public class StringFormat : IDisposable { public void Dispose() {} public StringAlignment Alignment { get; set; } public StringAlignment LineAlignment { get; set; } }
    public enum StringAlignment { Near=0, Center=1, Far=2 }
    public enum StringTrimming { None=0, Character=1, Word=2, EllipsisCharacter=3, EllipsisWord=4, EllipsisPath=5 }
    public enum SmoothingMode { Default=0, HighSpeed=1, HighQuality=2, None=3, AntiAlias=4 }
    public enum TextRenderingHint { SystemDefault=0, SingleBitPerPixelGridFit=1, SingleBitPerPixel=2, AntiAliasGridFit=3, AntiAlias=4, ClearTypeGridFit=5 }
    public enum InterpolationMode { Invalid=-1, Default=0, Low=1, High=2, Bilinear=3, Bicubic=4, NearestNeighbor=5, HighQualityBilinear=6, HighQualityBicubic=7 }
    public enum CompositingQuality { Invalid=-1, Default=0, HighSpeed=1, HighQuality=2, GammaCorrected=3, AssumeLinear=4 }
    public enum PixelFormat { DontCare=0, Undefined=0, Indexed=65536, Gdi=131072, Format16bppRgb555=135173, Format16bppRgb565=135174, Format24bppRgb=137224, Format32bppRgb=139273, Format1bppIndexed=196865, Format4bppIndexed=197634, Format8bppIndexed=198659, Alpha=262144, Format16bppArgb1555=397319, PAlpha=524288, Format32bppPArgb=925707, Format16bppGrayScale=1052676, Format48bppRgb=1060876, Format64bppPArgb=1851406, Canonical=2097152, Format32bppArgb=2498570, Format64bppArgb=3424269 }
    public abstract class ImageCodecInfo { public Guid Clsid; public Guid FormatID; public string CodecName; public string FormatDescription; public string FilenameExtension; public string MimeType; public ImageCodecFlags Flags; public int Version; public ImageCodecInfo[] Codecs => Array.Empty<ImageCodecInfo>(); public static ImageCodecInfo[] GetImageEncoders() => Array.Empty<ImageCodecInfo>(); public static ImageCodecInfo[] GetImageDecoders() => Array.Empty<ImageCodecInfo>(); }
    [Flags] public enum ImageCodecFlags { Encoder=1, Decoder=2, SupportBitmap=4, SupportVector=8, SeekableEncode=16, BlockDecode=32, Builtin=65536, System=131072 }
    public class EncoderParameters : IDisposable { public EncoderParameter[] Param; public EncoderParameters() {} public EncoderParameters(int count) { Param = new EncoderParameter[count]; } public void Dispose() {} }
    public class EncoderParameter : IDisposable { public Encoder Encoder; public int NumberOfValues; public Type ValueType; public byte[] Value; public EncoderParameter() {} public EncoderParameter(Encoder enc, long val) {} public void Dispose() {} }
    public sealed class Encoder { public Guid Guid; public Encoder(Guid g) { Guid = g; } public static Encoder Quality => new Encoder(Guid.Empty); public static Encoder Compression => new Encoder(Guid.Empty); public static Encoder ColorDepth => new Encoder(Guid.Empty); }
}

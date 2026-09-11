using System;

namespace SharpDX
{
    public struct Vector2
    {
        public float X; public float Y;
        public Vector2(float x, float y) { X = x; Y = y; }
        public static Vector2 Zero => new Vector2(0, 0);
    }

    public struct Size2F
    {
        public float Width; public float Height;
        public Size2F(float w, float h) { Width = w; Height = h; }
    }

    public struct Size2
    {
        public int Width; public int Height;
        public Size2(int w, int h) { Width = w; Height = h; }
    }

    public struct RectangleF
    {
        public float X; public float Y; public float Width; public float Height;
        public RectangleF(float x, float y, float w, float h) { X = x; Y = y; Width = w; Height = h; }
        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;
    }

    public struct Rectangle
    {
        public int X; public int Y; public int Width; public int Height;
        public Rectangle(int x, int y, int w, int h) { X = x; Y = y; Width = w; Height = h; }
    }

    public struct Point
    {
        public int X; public int Y;
        public Point(int x, int y) { X = x; Y = y; }
    }

    public struct PointF
    {
        public float X; public float Y;
        public PointF(float x, float y) { X = x; Y = y; }
    }

    public struct Color
    {
        public byte A; public byte R; public byte G; public byte B;
        public Color(byte r, byte g, byte b, byte a = 255) { A = a; R = r; G = g; B = b; }
        public Color(float r, float g, float b, float a = 1f) { A = (byte)(a * 255); R = (byte)(r * 255); G = (byte)(g * 255); B = (byte)(b * 255); }
        public static Color FromArgb(byte a, byte r, byte g, byte b) => new Color(r, g, b, a);
        public static Color FromRgba(byte r, byte g, byte b, byte a = 255) => new Color(r, g, b, a);
        public uint ToBgra() => (uint)((B << 24) | (G << 16) | (R << 8) | A);
        public uint ToRgba() => (uint)((R << 24) | (G << 16) | (B << 8) | A);
    }

    public struct Color4
    {
        public float A; public float R; public float G; public float B;
        public Color4(float r, float g, float b, float a = 1f) { A = a; R = r; G = g; B = b; }
        public Color4(Color c) { A = c.A / 255f; R = c.R / 255f; G = c.G / 255f; B = c.B / 255f; }
        public static implicit operator Color4(Color c) => new Color4(c);
    }

    public struct Matrix3x2
    {
        public float M11; public float M12;
        public float M21; public float M22;
        public float M31; public float M32;
        public static Matrix3x2 Identity => new Matrix3x2 { M11 = 1, M22 = 1 };
    }

    public abstract class Component : IDisposable
    {
        public virtual void Dispose() { }
        public virtual bool IsDisposed => false;
    }

    public class ComObject : Component
    {
    }

    public class SharpDXException : Exception
    {
        public SharpDXException() { }
        public SharpDXException(string message) : base(message) { }
    }
}

namespace SharpDX.Direct2D1
{
    using SharpDX;

    public enum AlphaMode : int { Premultiplied = 0, Straight = 1, Unknown = 2, Ignore = 3 }
    public enum RenderTargetType : int { Default = 0, Software = 1, Hardware = 2 }
    public enum FeatureLevel : int { Level_DEFAULT = 0, Level_9_1 = 37120, Level_9_2 = 37376, Level_9_3 = 37632, Level_10_0 = 40960, Level_10_1 = 41216 }
    [Flags] public enum RenderTargetUsage : int { None = 0, ForceBitmapRemoting = 1, CompatibleOnly = 2, GdiCompatible = 4 }
    public enum AntialiasMode : int { PerPrimitive = 0, Aliased = 1 }
    public enum TextAntialiasMode : int { Default = 0, Cleartype = 1, Grayscale = 2, Aliased = 3 }
    public enum DrawTextOptions : int { None = 0, NoSnap = 1, Clip = 2 }
    public enum DashStyle : int { Solid = 0, Dash = 1, Dot = 2, DashDot = 3, DashDotDot = 4, Custom = 5 }
    public enum CapStyle : int { Flat = 0, Square = 1, Round = 2, Triangle = 3 }
    public enum LineJoin : int { Miter = 0, Bevel = 1, Round = 2, MiterOrBevel = 3 }
    public enum FillMode : int { Alternate = 0, Winding = 1 }
    public enum SweepDirection : int { CounterClockwise = 0, Clockwise = 1 }
    public enum FigureBegin : int { Filled = 0, Hollow = 1 }
    public enum FigureEnd : int { Open = 0, Closed = 1 }
    public enum ArcSize : int { Small = 0, Large = 1 }
    public enum BitmapInterpolationMode : int { NearestNeighbor = 0, Linear = 1 }

    public struct RenderTargetProperties : IDisposable
    {
        public RenderTargetType Type; public PixelFormat PixelFormat; public float DpiX; public float DpiY; public RenderTargetUsage Usage; public FeatureLevel MinLevel;
        public RenderTargetProperties(RenderTargetType type, PixelFormat pf, float dpiX = 0, float dpiY = 0, RenderTargetUsage usage = 0, FeatureLevel minLevel = 0) { Type = type; PixelFormat = pf; DpiX = dpiX; DpiY = dpiY; Usage = usage; MinLevel = minLevel; }
        public void Dispose() { }
    }

    public struct PixelFormat
    {
        public int Format; public AlphaMode AlphaMode;
        public PixelFormat(int fmt, AlphaMode a = AlphaMode.Unknown) { Format = fmt; AlphaMode = a; }
    }

    public abstract class RenderTarget : IDisposable
    {
        public Size2F Size { get; set; }
        public Size2 PixelSize { get; set; }
        public Matrix3x2 Transform { get; set; }
        public AntialiasMode AntialiasMode { get; set; }
        public TextAntialiasMode TextAntialiasMode { get; set; }
        public float DpiX { get; set; }
        public float DpiY { get; set; }

        public virtual void BeginDraw() { }
        public virtual void EndDraw() { }
        public virtual void Clear(Color color) { }
        public virtual void Clear(Color4 color) { }
        public virtual void DrawLine(Vector2 p1, Vector2 p2, Brush brush, float strokeWidth = 1, StrokeStyle style = null) { }
        public virtual void DrawRectangle(RectangleF rect, Brush brush, float sw = 1, StrokeStyle ss = null) { }
        public virtual void FillRectangle(RectangleF rect, Brush brush) { }
        public virtual void DrawEllipse(Vector2 center, float rx, float ry, Brush brush, float sw = 1, StrokeStyle ss = null) { }
        public virtual void FillEllipse(Vector2 center, float rx, float ry, Brush brush) { }
        public virtual void DrawText(string text, SharpDX.DirectWrite.TextFormat format, RectangleF rect, Brush brush, DrawTextOptions o = 0) { }
        public virtual void DrawGeometry(Geometry geom, Brush brush, float sw = 1, StrokeStyle ss = null) { }
        public virtual void FillGeometry(Geometry geom, Brush brush) { }
        public virtual void DrawBitmap(Bitmap bmp, RectangleF? dest = null, float opacity = 1, BitmapInterpolationMode im = 0) { }
        public virtual void Flush() { }
        public virtual bool IsDisposed => false;
        public virtual void Dispose() { }
    }

    public abstract class Brush : IDisposable { public virtual void Dispose() { } public float Opacity { get; set; } public Matrix3x2 Transform { get; set; } }
    public class SolidColorBrush : Brush { public Color Color { get; set; } public SolidColorBrush() { } public SolidColorBrush(Color c) { Color = c; } }
    public abstract class Bitmap : IDisposable { public Size2F Size { get; set; } public Size2 PixelSize { get; set; } public float DpiX { get; set; } public float DpiY { get; set; } public PixelFormat PixelFormat { get; set; } public virtual void Dispose() { } }
    public abstract class Geometry : IDisposable { public virtual RectangleF GetBounds() => default; public virtual void Dispose() { } }
    public class PathGeometry : Geometry { public new virtual void Open() { } public virtual void BeginFigure(Vector2 p, FigureBegin b) { } public virtual void AddLine(Vector2 p) { } public virtual void AddLines(Vector2[] ps) { } public virtual void AddArc(Vector2 end, Size2F size, float rot, SweepDirection sd, ArcSize asz) { } public virtual void AddBezier(Vector2 p1, Vector2 p2, Vector2 p3) { } public virtual void EndFigure(FigureEnd e) { } public virtual void Close() { } }
    public class RectangleGeometry : Geometry { public RectangleF Rect { get; set; } }
    public class EllipseGeometry : Geometry { public Vector2 Center { get; set; } public float RadiusX { get; set; } public float RadiusY { get; set; } }
    public class StrokeStyle : IDisposable { public DashStyle DashStyle { get; set; } public CapStyle StartCap { get; set; } public CapStyle EndCap { get; set; } public LineJoin LineJoin { get; set; } public float DashOffset { get; set; } public float MiterLimit { get; set; } public float[] DashPattern { get; set; } public StrokeStyle() { } public virtual void Dispose() { } }
    public class Factory : IDisposable { public Factory() { } public Factory(FactoryType t = FactoryType.SingleThreaded) { } public virtual void Dispose() { } }
    public enum FactoryType { SingleThreaded = 0, MultiThreaded = 1 }
    public class DCRenderTarget : RenderTarget { }
    public class HwndRenderTargetProperties { public IntPtr Hwnd; public Size2 PixelSize; public PresentOptions PresentOptions; public HwndRenderTargetProperties() { Hwnd = IntPtr.Zero; PixelSize = default; PresentOptions = 0; } }
    public enum PresentOptions { None = 0, RetainContents = 1, Immediately = 2 }
    public class WindowRenderTarget : RenderTarget { }
    public class GradientStopCollection : IDisposable { public virtual void Dispose() { } }
    public class LinearGradientBrush : Brush { }
    public class RadialGradientBrush : Brush { }
    public struct GradientStop { public float Position; public Color Color; public GradientStop(float p, Color c) { Position = p; Color = c; } }
}

namespace SharpDX.DirectWrite
{
    using SharpDX;

    public enum FontWeight : int { Thin = 100, ExtraLight = 200, UltraLight = 200, Light = 300, SemiLight = 350, Normal = 400, Regular = 400, Medium = 500, DemiBold = 600, SemiBold = 600, Bold = 700, ExtraBold = 800, UltraBold = 800, Black = 900, Heavy = 900, ExtraBlack = 950, UltraBlack = 950 }
    public enum FontStyle : int { Normal = 0, Oblique = 1, Italic = 2 }
    public enum FontStretch : int { Undefined = 0, UltraCondensed = 1, ExtraCondensed = 2, Condensed = 3, SemiCondensed = 4, Normal = 5, Medium = 5, SemiExpanded = 6, Expanded = 7, ExtraExpanded = 8, UltraExpanded = 9 }
    public enum TextAlignment : int { Leading = 0, Trailing = 1, Center = 2, Justified = 3 }
    public enum ParagraphAlignment : int { Near = 0, Far = 1, Center = 2 }
    public enum WordWrapping : int { Wrap = 0, NoWrap = 1, EmergencyBreak = 2, WholeWord = 3, Character = 4 }
    public enum ReadingDirection : int { LeftToRight = 0, RightToLeft = 1, TopToBottom = 2, BottomToTop = 3 }
    public enum FlowDirection : int { TopToBottom = 0, BottomToTop = 1, LeftToRight = 2, RightToLeft = 3 }
    public enum LineSpacingMethod : int { Default = 0, Uniform = 1, Proportional = 2 }
    public enum TrimGranularity : int { None = 0, Character = 1, Word = 2 }

    public class Factory : IDisposable { public Factory() { } public Factory(FactoryType t = FactoryType.Shared) { } public virtual void Dispose() { } }
    public enum FactoryType { Shared = 0, Isolated = 1 }
    public class TextFormat : IDisposable
    {
        public string FontFamilyName { get; set; } = "Segoe UI";
        public float FontSize { get; set; } = 12;
        public FontWeight FontWeight { get; set; } = FontWeight.Normal;
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        public FontStretch FontStretch { get; set; } = FontStretch.Normal;
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Leading;
        public ParagraphAlignment ParagraphAlignment { get; set; } = ParagraphAlignment.Near;
        public WordWrapping WordWrapping { get; set; } = WordWrapping.Wrap;
        public ReadingDirection ReadingDirection { get; set; } = ReadingDirection.LeftToRight;
        public FlowDirection FlowDirection { get; set; } = FlowDirection.TopToBottom;
        public TextFormat() { }
        public TextFormat(Factory factory, string fontFamily, float size) { FontFamilyName = fontFamily; FontSize = size; }
        public TextFormat(Factory factory, string fontFamily, FontWeight weight, FontStyle style, FontStretch stretch, float size) { FontFamilyName = fontFamily; FontWeight = weight; FontStyle = style; FontStretch = stretch; FontSize = size; }
        public virtual void Dispose() { }
    }

    public class TextLayout : TextFormat
    {
        public string Text { get; set; } = "";
        public float MaxWidth { get; set; }
        public float MaxHeight { get; set; }
        public TextLayout() { }
        public TextLayout(Factory factory, string text, TextFormat format, float maxW, float maxH) { Text = text; MaxWidth = maxW; MaxHeight = maxH; }
    }

    public class TextRenderer : IDisposable { public virtual void Dispose() { } }
    public class FontFamily : IDisposable { public virtual void Dispose() { } }
    public class Font : IDisposable { public virtual void Dispose() { } }
    public class FontFace : IDisposable { public virtual void Dispose() { } }
    public class FontCollection : IDisposable { public virtual void Dispose() { } }
    public class FontList : FontCollection { }
    public struct FontMetrics { public short DesignUnitsPerEm; public short Ascent; public short Descent; public short LineGap; }
    public struct GlyphRun { public int GlyphCount; }
    public struct Trimming
    {
        public TrimGranularity Granularity; public int Delimiter; public int DelimiterCount;
        public Trimming(TrimGranularity g = 0, int d = 0, int dc = 0) { Granularity = g; Delimiter = d; DelimiterCount = dc; }
    }
}

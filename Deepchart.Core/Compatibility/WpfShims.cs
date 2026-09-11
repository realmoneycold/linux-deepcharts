using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows
{
    public enum Visibility : byte { Visible = 0, Hidden = 1, Collapsed = 2 }
    public enum HorizontalAlignment : byte { Left = 0, Center = 1, Right = 2, Stretch = 3 }
    public enum VerticalAlignment : byte { Top = 0, Center = 1, Bottom = 2, Stretch = 3 }
    public enum FlowDirection : byte { LeftToRight = 0, RightToLeft = 1 }
    public enum TextAlignment : byte { Left = 0, Right = 1, Center = 2, Justify = 3 }
    public enum TextWrapping : byte { NoWrap = 0, Wrap = 1, WrapWithOverflow = 2 }
    public enum TextTrimming : byte { None = 0, CharacterEllipsis = 1, WordEllipsis = 2 }
    public enum FontStyle : byte { Normal = 0, Oblique = 1, Italic = 2 }
    public enum FontStretch : byte { Undefined = 0, UltraCondensed = 1, ExtraCondensed = 2, Condensed = 3, SemiCondensed = 4, Normal = 5, Medium = 5, SemiExpanded = 6, Expanded = 7, ExtraExpanded = 8, UltraExpanded = 9 }
    public enum FontWeight : ushort { Thin = 100, ExtraLight = 200, UltraLight = 200, Light = 300, SemiLight = 350, Normal = 400, Regular = 400, Medium = 500, DemiBold = 600, SemiBold = 600, Bold = 700, ExtraBold = 800, UltraBold = 800, Black = 900, Heavy = 900, ExtraBlack = 950, UltraBlack = 950 }
    public enum ClickMode : byte { Release = 0, Press = 1, Hover = 2 }
    public enum Orientation : byte { Horizontal = 0, Vertical = 1 }
    public enum GridUnitType : byte { Auto = 0, Pixel = 1, Star = 2 }
    public enum DataGridSelectionMode : byte { FullRow = 0, CellOrRowHeader = 1 }

    [AttributeUsage(AttributeTargets.All)] public sealed class LocalizabilityAttribute : Attribute { public LocalizabilityAttribute(LocalizationCategory c) { Category = c; } public LocalizationCategory Category { get; set; } public bool Readable { get; set; } public bool Modifiability { get; set; } }
    public enum LocalizationCategory { None = 0, Text, Title, Label, Button, CheckBox, ComboBox, ListBox, Menu, RadioButton, ToolTip, Hyperlink, TextFlow, XmlData, Font, Inherit, Ignore, NeverLocalize, Comment }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)] public sealed class TypeConverterAttribute : Attribute { public TypeConverterAttribute() {} public TypeConverterAttribute(string typeName) { ConverterTypeName = typeName; } public TypeConverterAttribute(Type type) { ConverterTypeName = type.AssemblyQualifiedName; } public string ConverterTypeName { get; } }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)] public sealed class ValueSerializerAttribute : Attribute { public ValueSerializerAttribute(string typeName) { ValueSerializerTypeName = typeName; } public ValueSerializerAttribute(Type type) { ValueSerializerTypeName = type.AssemblyQualifiedName; } public string ValueSerializerTypeName { get; } }
    public class DependencyProperty { public string Name { get; } public Type PropertyType { get; } public Type OwnerType { get; } public object DefaultMetadata { get; } public static DependencyProperty Register(string name, Type propType, Type owner) => new DependencyProperty(name, propType, owner); public static DependencyProperty Register(string name, Type propType, Type owner, object metadata) => new DependencyProperty(name, propType, owner); private DependencyProperty(string n, Type pt, Type o) { Name=n; PropertyType=pt; OwnerType=o; } }
    public class DependencyObject { public object GetValue(DependencyProperty dp) => null; public void SetValue(DependencyProperty dp, object val) {} public void ClearValue(DependencyProperty dp) {} }
    public struct Duration { public bool HasTimeSpan { get; set; } public TimeSpan TimeSpan { get; set; } public static Duration Automatic => new Duration(); public static Duration Forever => new Duration(); }
    public struct GridLength { public double Value { get; set; } public GridUnitType GridUnitType { get; set; } public bool IsAbsolute => GridUnitType == GridUnitType.Pixel; public bool IsAuto => GridUnitType == GridUnitType.Auto; public bool IsStar => GridUnitType == GridUnitType.Star; public static GridLength Auto => new GridLength { GridUnitType = GridUnitType.Auto }; public GridLength(double v, GridUnitType t=GridUnitType.Pixel) { Value=v; GridUnitType=t; } }
    public struct Thickness { public double Left, Top, Right, Bottom; public Thickness(double all) { Left=Top=Right=Bottom=all; } public Thickness(double l, double t, double r, double b) { Left=l; Top=t; Right=r; Bottom=b; } }
    public struct CornerRadius { public double TopLeft, TopRight, BottomRight, BottomLeft; public CornerRadius(double all) { TopLeft=TopRight=BottomRight=BottomLeft=all; } public CornerRadius(double tl, double tr, double br, double bl) { TopLeft=tl; TopRight=tr; BottomRight=br; BottomLeft=bl; } }
    public struct Size { public double Width, Height; public Size(double w, double h) { Width=w; Height=h; } public static Size Empty => new Size(double.NegativeInfinity, double.NegativeInfinity); public bool IsEmpty => Width<0 || Height<0; }
    public struct Point { public double X, Y; public Point(double x, double y) { X=x; Y=y; } }
    public struct Rect { public double X, Y, Width, Height; public Rect(double x, double y, double w, double h) { X=x; Y=y; Width=w; Height=h; } public Rect(Point location, Size size) { X=location.X; Y=location.Y; Width=size.Width; Height=size.Height; } public double Left => X; public double Top => Y; public double Right => X+Width; public double Bottom => Y+Height; public bool IsEmpty => Width<0 || Height<0; public bool Contains(Point p) => p.X>=Left && p.X<Right && p.Y>=Top && p.Y<Bottom; }
    public struct Vector { public double X, Y; public Vector(double x, double y) { X=x; Y=y; } }
    public struct Int32Rect { public int X, Y, Width, Height; public Int32Rect(int x, int y, int w, int h) { X=x; Y=y; Width=w; Height=h; } public static Int32Rect Empty => default; public bool IsEmpty => Width==0 && Height==0 && X==0 && Y==0; }
    public class FrameworkElement : UIElement { public Style Style { get; set; } public Style FindResource(object key) => Style; public object TryFindResource(object key) => null; }
    public class UIElement : DependencyObject, IInputElement { public bool Visibility { get; set; } public bool IsEnabled { get; set; } public event EventHandler Loaded; public event EventHandler Unloaded; }
    public interface IInputElement {}
    public class Style {}
    public class ResourceDictionary {}
    public class ControlTemplate {}
    public class DataTemplate {}
    public class FontFamily { public FontFamily() {} public FontFamily(string name) { Source = name; } public string Source { get; } public override string ToString() => Source ?? ""; }
    public class Application : DispatcherObject { public static Application Current => _current; public ResourceDictionary Resources { get; set; } = new ResourceDictionary(); private static Application _current = new Application(); }
    public class DispatcherObject { public Dispatcher Dispatcher { get; } = new Dispatcher(); }
    public class Dispatcher { public void Invoke(Action a) => a(); public T Invoke<T>(Func<T> f) => f(); public DispatcherOperation BeginInvoke(Action a) { a(); return new DispatcherOperation(); } public bool CheckAccess() => true; public bool VerifyAccess() => true; }
    public class DispatcherOperation { public DispatcherOperationStatus Status { get; set; } public object Result { get; set; } public void Wait() {} }
    public enum DispatcherOperationStatus { Pending = 0, Aborted = 1, Completed = 2, Executing = 3 }
    public class Freezable : DependencyObject { public bool IsFrozen { get; } public Freezable Clone() => (Freezable)MemberwiseClone(); public Freezable CloneCurrentValue() => (Freezable)MemberwiseClone(); public void Freeze() {} public static void Freeze(Freezable f) {} }
    public class ImageSource : Freezable {}
    public class BitmapSource : ImageSource { public int PixelWidth { get; set; } public int PixelHeight { get; set; } public double DpiX { get; set; } = 96; public double DpiY { get; set; } = 96; public static BitmapSource Create(int w, int h, double dpiX, double dpiY, int pixelFormat, int palette, byte[] pixels, int stride) => new BitmapSource { PixelWidth=w, PixelHeight=h, DpiX=dpiX, DpiY=dpiY }; }
    public class IconBitmapDecoder {}
    public class RoutedEventArgs : EventArgs { public RoutedEventArgs() {} public RoutedEventArgs(object routedEvent) {} public object Source { get; set; } public object OriginalSource { get; set; } public bool Handled { get; set; } }
    public delegate void RoutedEventHandler(object sender, RoutedEventArgs e);
    public class InputEventArgs : RoutedEventArgs { public InputEventArgs() {} public int Timestamp { get; set; } }
    public class Keyboard { public static ModifierKeys Modifiers => ModifierKeys.None; }
    [Flags] public enum ModifierKeys { None = 0, Alt = 1, Control = 2, Shift = 4, Windows = 8, Apple = 8 }
    public class KeyEventArgs : KeyboardEventArgs { public Key Key { get; set; } = Key.None; public Key SystemKey { get; set; } = Key.None; }
    public class KeyboardEventArgs : InputEventArgs {}
    public enum Key { None=0, Cancel=1, Back=2, Tab=3, LineFeed=4, Clear=5, Return=6, Enter=6, Pause=7, Capital=8, CapsLock=8, KanaMode=9, ImeOn=10, JunjaMode=13, FinalMode=14, HanjaMode=15, ImeOff=16, Escape=17, ImeConvert=18, ImeNoConvert=19, Space=20, PageUp=21, Prior=21, PageDown=22, Next=22, End=23, Home=24, Left=25, Up=26, Right=27, Down=28, Select=29, Print=30, Execute=31, Snapshot=32, PrintScreen=32, Insert=33, Delete=34, Help=35, D0=36, D1=37, D2=38, D3=39, D4=40, D5=41, D6=42, D7=43, D8=44, D9=45, A=56, B=57, C=58, D=59, E=60, F=61, G=62, H=63, I=64, J=65, K=66, L=67, M=68, N=69, O=70, P=71, Q=72, R=73, S=74, T=75, U=76, V=77, W=78, X=79, Y=80, Z=81, LWin=91, RWin=92, Apps=93, Sleep=95, NumPad0=96, NumPad1=97, NumPad2=98, NumPad3=99, NumPad4=100, NumPad5=101, NumPad6=102, NumPad7=103, NumPad8=104, NumPad9=105, Multiply=106, Add=107, Separator=108, Subtract=109, Decimal=110, Divide=111, F1=112, F2=113, F3=114, F4=115, F5=116, F6=117, F7=118, F8=119, F9=120, F10=121, F11=122, F12=123, NumLock=144, Scroll=145, LeftShift=160, RightShift=161, LeftCtrl=162, RightCtrl=163, LeftAlt=164, RightAlt=165, BrowserBack=166, BrowserForward=167, BrowserRefresh=168, BrowserStop=169, BrowserSearch=170, BrowserFavorites=171, BrowserHome=172, VolumeMute=173, VolumeDown=174, VolumeUp=175, MediaNext=176, MediaPrevious=177, MediaStop=178, MediaPlay=179, LaunchMail=180, LaunchApplication1=187, LaunchApplication2=188, Oem1=186, OemSemicolon=186, OemPlus=187, OemComma=188, OemMinus=189, OemPeriod=190, Oem2=191, OemQuestion=191, Oem3=192, OemTilde=192, Oem4=219, OemOpenBrackets=219, Oem5=220, OemPipe=220, Oem6=221, OemCloseBrackets=221, Oem7=222, OemQuotes=222, Oem8=223, Oem102=226, OemBackslash=226, ImeProcessed=229, System=231, OemAttn=239, OemFinish=240, Attn=246, CrSel=247, ExSel=248, EraseEof=249, Play=250, Zoom=251, NoName=252, Pa1=253, OemClear=254, DeadCharProcessed=255 }
    public class MouseEventArgs : InputEventArgs {}
    public class MouseButtonEventArgs : MouseEventArgs { public MouseButton ChangedButton { get; set; } public int ClickCount { get; set; } public MouseButtonState ButtonState { get; set; } }
    public enum MouseButton { Left = 0, Middle = 1, Right = 2, XButton1 = 3, XButton2 = 4 }
    public enum MouseButtonState { Released = 0, Pressed = 1 }
    public class MouseWheelEventArgs : MouseEventArgs { public int Delta { get; set; } }
    [Flags] public enum DragDropKeyStates { None = 0, LeftMouseButton = 1, RightMouseButton = 2, ShiftKey = 4, ControlKey = 8, MiddleMouseButton = 16, AltKey = 32 }
    [Flags] public enum DragDropEffects { None = 0, Copy = 1, Move = 2, Link = 4, Scroll = -2147483648, All = -2147483645 }
    public class Cursor { public Cursor() {} public Cursor(string cursor) {} public static Cursors Cursors => new Cursors(); }
    public class Cursors { public Cursor None => new Cursor(); public Cursor No => new Cursor(); public Cursor Arrow => new Cursor(); public Cursor Cross => new Cursor(); public Cursor Help => new Cursor(); public Cursor IBeam => new Cursor(); public Cursor SizeAll => new Cursor(); public Cursor SizeNESW => new Cursor(); public Cursor SizeNS => new Cursor(); public Cursor SizeNWSE => new Cursor(); public Cursor SizeWE => new Cursor(); public Cursor UpArrow => new Cursor(); public Cursor Wait => new Cursor(); public Cursor AppStarting => new Cursor(); public Cursor Pen => new Cursor(); public Cursor ScrollNS => new Cursor(); public Cursor ScrollWE => new Cursor(); public Cursor ScrollAll => new Cursor(); public Cursor ScrollN => new Cursor(); public Cursor ScrollS => new Cursor(); public Cursor ScrollW => new Cursor(); public Cursor ScrollE => new Cursor(); public Cursor ScrollNW => new Cursor(); public Cursor ScrollNE => new Cursor(); public Cursor ScrollSW => new Cursor(); public Cursor ScrollSE => new Cursor(); public Cursor Hand => new Cursor(); }
    public class MessageBox { public static MessageBoxResult Show(string text) => MessageBoxResult.None; public static MessageBoxResult Show(string text, string caption, MessageBoxButton b=0, MessageBoxImage i=0) => MessageBoxResult.None; }
    public enum MessageBoxResult { None = 0, OK = 1, Cancel = 2, Yes = 6, No = 7 }
    public enum MessageBoxButton { OK = 0, OKCancel = 1, YesNoCancel = 3, YesNo = 4 }
    public enum MessageBoxImage { None = 0, Hand = 16, Stop = 16, Error = 16, Question = 32, Exclamation = 48, Warning = 48, Asterisk = 64, Information = 64 }
    public class Window : ContentControl { public string Title { get; set; } public double Width { get; set; } public double Height { get; set; } public double Left { get; set; } public double Top { get; set; } public WindowState WindowState { get; set; } public WindowStyle WindowStyle { get; set; } public ResizeMode ResizeMode { get; set; } public bool Topmost { get; set; } public bool ShowActivated { get; set; } = true; public void Show() {} public bool? ShowDialog() => true; public void Close() {} public event EventHandler Closed; }
    public enum WindowState : byte { Normal = 0, Minimized = 1, Maximized = 2 }
    public enum WindowStyle : byte { None = 0, SingleBorderWindow = 1, ThreeDBorderWindow = 2, ToolWindow = 3 }
    public enum ResizeMode : byte { NoResize = 0, CanMinimize = 1, CanResize = 2, CanResizeWithGrip = 3 }
    public class UserControl : ContentControl {}
    public class Page : ContentControl {}
    public class ContentControl : Control { public object Content { get; set; } }
    public class Control : FrameworkElement { public double FontSize { get; set; } = 12; public FontFamily FontFamily { get; set; } public FontStyle FontStyle { get; set; } public FontWeight FontWeight { get; set; } public FontStretch FontStretch { get; set; } public HorizontalAlignment HorizontalContentAlignment { get; set; } public VerticalAlignment VerticalContentAlignment { get; set; } public Thickness Padding { get; set; } public Thickness BorderThickness { get; set; } public int TabIndex { get; set; } public bool IsTabStop { get; set; } = true; public string ToolTip { get; set; } }
    public class ItemsControl : Control { public System.Collections.IEnumerable ItemsSource { get; set; } public object SelectedItem { get; set; } public int SelectedIndex { get; set; } }
    public class ContentPresenter : FrameworkElement { public object Content { get; set; } }
    public class ItemsPanelTemplate {}
    public class Panel : FrameworkElement { public System.Collections.IEnumerable Children { get; set; } = new object[0]; }
    public class Canvas : Panel {}
    public class StackPanel : Panel { public Orientation Orientation { get; set; } }
    public class WrapPanel : Panel { public Orientation Orientation { get; set; } }
    public class DockPanel : Panel {}
    public class Grid : Panel { public ColumnDefinitionCollection ColumnDefinitions { get; } = new ColumnDefinitionCollection(); public RowDefinitionCollection RowDefinitions { get; } = new RowDefinitionCollection(); }
    public class ColumnDefinition : DependencyObject { public GridLength Width { get; set; } = GridLength.Auto; public double MinWidth { get; set; } public double MaxWidth { get; set; } public double ActualWidth => 0; }
    public class ColumnDefinitionCollection : System.Collections.Generic.List<ColumnDefinition> {}
    public class RowDefinition : DependencyObject { public GridLength Height { get; set; } = GridLength.Auto; public double MinHeight { get; set; } public double MaxHeight { get; set; } public double ActualHeight => 0; }
    public class RowDefinitionCollection : System.Collections.Generic.List<RowDefinition> {}
    public class Border : FrameworkElement { public Thickness BorderThickness { get; set; } public object Background { get; set; } public object BorderBrush { get; set; } public CornerRadius CornerRadius { get; set; } public Thickness Padding { get; set; } public UIElement Child { get; set; } }
    public class TextBlock : FrameworkElement { public string Text { get; set; } public double FontSize { get; set; } = 12; public FontFamily FontFamily { get; set; } public FontStyle FontStyle { get; set; } public FontWeight FontWeight { get; set; } public TextAlignment TextAlignment { get; set; } public TextWrapping TextWrapping { get; set; } public TextTrimming TextTrimming { get; set; } public Brush Foreground { get; set; } public Brush Background { get; set; } }
    public class TextBox : Control { public string Text { get; set; } public bool IsReadOnly { get; set; } public bool AcceptsReturn { get; set; } public bool AcceptsTab { get; set; } public int MaxLength { get; set; } public event EventHandler TextChanged; public event RoutedEventHandler SelectionChanged; }
    public class ButtonBase : ContentControl { public ICommand Command { get; set; } public object CommandParameter { get; set; } public ClickMode ClickMode { get; set; } public event RoutedEventHandler Click; }
    public class Button : ButtonBase {}
    public class RepeatButton : ButtonBase {}
    public class ToggleButton : ButtonBase { public bool? IsChecked { get; set; } public event RoutedEventHandler Checked; public event RoutedEventHandler Unchecked; }
    public class CheckBox : ToggleButton {}
    public class RadioButton : ToggleButton {}
    public class ComboBox : Selector { public bool IsDropDownOpen { get; set; } public bool IsEditable { get; set; } public bool IsReadOnly { get; set; } public string Text { get; set; } public int MaxDropDownHeight { get; set; } }
    public class Selector : ItemsControl {}
    public class ListBox : Selector { public ScrollViewer ScrollHost { get; set; } public bool VirtualizingStackPanel { get; set; } }
    public class ListView : ListBox { public GridView View { get; set; } }
    public class GridView {}
    public class TreeView : ItemsControl {}
    public class TreeViewItem : HeaderedItemsControl {}
    public class HeaderedItemsControl : ItemsControl { public object Header { get; set; } }
    public class HeaderedContentControl : ContentControl { public object Header { get; set; } }
    public class TabControl : Selector { public object SelectedContent { get; set; } }
    public class TabItem : HeaderedContentControl { public object Header { get; set; } }
    public class Expander : HeaderedContentControl { public bool IsExpanded { get; set; } public ExpandDirection ExpandDirection { get; set; } }
    public enum ExpandDirection { Down = 0, Up = 1, Left = 2, Right = 3 }
    public class GroupBox : HeaderedContentControl {}
    public class Slider : RangeBase { public Orientation Orientation { get; set; } public double TickFrequency { get; set; } public bool IsSnapToTickEnabled { get; set; } public double SmallChange { get; set; } = 1; public double LargeChange { get; set; } = 10; }
    public class RangeBase : Control { public double Minimum { get; set; } public double Maximum { get; set; } = 100; public double Value { get; set; } public event RoutedEventHandler ValueChanged; }
    public class ProgressBar : RangeBase { public bool IsIndeterminate { get; set; } public Orientation Orientation { get; set; } }
    public class ScrollBar : RangeBase { public Orientation Orientation { get; set; } public ScrollBarVisibility Visibility { get; set; } public double ViewportSize { get; set; } = double.NaN; }
    public enum ScrollBarVisibility { Disabled = 0, Auto = 1, Hidden = 2, Visible = 3 }
    public class ScrollViewer : ContentControl { public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; } public ScrollBarVisibility VerticalScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto; public bool CanVerticallyScroll { get; set; } public bool CanHorizontallyScroll { get; set; } public double VerticalOffset { get; set; } public double HorizontalOffset { get; set; } public double ExtentHeight { get; set; } public double ExtentWidth { get; set; } public double ViewportHeight { get; set; } public double ViewportWidth { get; set; } public void ScrollToVerticalOffset(double v) {} public void ScrollToHorizontalOffset(double v) {} public void ScrollToHome() {} public void ScrollToEnd() {} }
    public class Separator : Control {}
    public class Label : ContentControl { public string ContentStringFormat { get; set; } }
    public class Image : FrameworkElement { public ImageSource Source { get; set; } public Stretch Stretch { get; set; } public StretchDirection StretchDirection { get; set; } }
    public enum Stretch { None = 0, Fill = 1, Uniform = 2, UniformToFill = 3 }
    public enum StretchDirection { UpOnly = 0, DownOnly = 1, Both = 2 }
    public class Shape : FrameworkElement { public Brush Fill { get; set; } public Brush Stroke { get; set; } public double StrokeThickness { get; set; } }
    public class Rectangle : Shape { public double RadiusX { get; set; } public double RadiusY { get; set; } }
    public class Ellipse : Shape {}
    public class Line : Shape { public double X1 { get; set; } public double Y1 { get; set; } public double X2 { get; set; } public double Y2 { get; set; } }
    public class Polyline : Shape { public PointCollection Points { get; set; } }
    public class Polygon : Shape { public PointCollection Points { get; set; } public FillRule FillRule { get; set; } }
    public class Path : Shape { public Geometry Data { get; set; } }
    public enum FillRule { EvenOdd = 0, Nonzero = 1 }
    public class PointCollection : System.Collections.Generic.List<Point> { public PointCollection() {} public PointCollection(System.Collections.Generic.IEnumerable<Point> pts) : base(pts) {} }
    public class Geometry : Freezable { public static Geometry Empty => new Geometry(); public Rect Bounds => new Rect(); }
    public class EllipseGeometry : Geometry { public Point Center { get; set; } public double RadiusX { get; set; } public double RadiusY { get; set; } }
    public class RectangleGeometry : Geometry { public Rect Rect { get; set; } public double RadiusX { get; set; } public double RadiusY { get; set; } }
    public class LineGeometry : Geometry { public Point StartPoint { get; set; } public Point EndPoint { get; set; } }
    public class PathGeometry : Geometry {}
    public class StreamGeometry : Geometry {}
    public class GeometryGroup : Geometry { public GeometryCollection Children { get; set; } }
    public class GeometryCollection : System.Collections.Generic.List<Geometry> {}
    public class StreamGeometryContext : IDisposable { public void Dispose() {} public void BeginFigure(Point start, bool filled, bool closed) {} public void LineTo(Point p, bool isStroked, bool isSmoothJoin) {} public void ArcTo(Point p, Size s, double rot, bool isLarge, SweepDirection d, bool s2, bool s3) {} public void BezierTo(Point p1, Point p2, Point p3, bool s, bool s2) {} public void QuadraticBezierTo(Point p1, Point p2, bool s, bool s2) {} public void Close() {} }
    public enum SweepDirection { Counterclockwise = 0, Clockwise = 1 }
    public class MediaPlayer { public void Open(Uri source) {} public void Play() {} public void Pause() {} public void Stop() {} public void Close() {} public double Volume { get; set; } public double SpeedRatio { get; set; } }
    public class SoundPlayer { public SoundPlayer() {} public SoundPlayer(string location) {} public SoundPlayer(System.IO.Stream s) {} public void Load() {} public void Play() {} public void PlaySync() {} public void Stop() {} }
    public class Timer : System.Threading.Timer { public Timer() : base(null, null, -1, -1) {} public Timer(System.Threading.TimerCallback cb) : base(cb, null, -1, -1) {} public Timer(System.Threading.TimerCallback cb, object s, int d, int p) : base(cb, s, d, p) {} public new void Change(int due, int period) { base.Change(due, period); } public event EventHandler Tick; }
    public class DispatcherTimer { public DispatcherTimer() {} public TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(100); public bool IsEnabled { get; set; } public DispatcherPriority Priority { get; set; } = DispatcherPriority.Background; public void Start() { IsEnabled = true; } public void Stop() { IsEnabled = false; } public event EventHandler Tick; }
    public enum DispatcherPriority { Invalid = -1, Inactive = 0, SystemIdle = 1, ApplicationIdle = 2, ContextIdle = 3, Background = 4, Input = 5, Loaded = 6, Render = 7, DataBind = 8, Normal = 9, Send = 10 }
    public class Brush : Freezable { public double Opacity { get; set; } = 1.0; public Transform Transform { get; set; } }
    public class SolidColorBrush : Brush { public SolidColorBrush() {} public SolidColorBrush(Color c) { Color = c; } public Color Color { get; set; } }
    public class GradientBrush : Brush { public GradientStopCollection GradientStops { get; set; } = new GradientStopCollection(); public ColorInterpolationMode ColorInterpolationMode { get; set; } public BrushMappingMode MappingMode { get; set; } public GradientSpreadMethod SpreadMethod { get; set; } }
    public class LinearGradientBrush : GradientBrush { public Point StartPoint { get; set; } public Point EndPoint { get; set; } }
    public class RadialGradientBrush : GradientBrush { public Point Center { get; set; } = new Point(0.5, 0.5); public Point GradientOrigin { get; set; } = new Point(0.5, 0.5); public double RadiusX { get; set; } = 0.5; public double RadiusY { get; set; } = 0.5; }
    public class GradientStopCollection : System.Collections.Generic.List<GradientStop> { public GradientStopCollection() {} }
    public class GradientStop { public GradientStop() {} public GradientStop(Color c, double o) { Color=c; Offset=o; } public Color Color { get; set; } public double Offset { get; set; } }
    public enum ColorInterpolationMode { SRgbLinearInterpolation = 0, ScRgbLinearInterpolation = 1 }
    public enum BrushMappingMode { Absolute = 0, RelativeToBoundingBox = 1 }
    public enum GradientSpreadMethod { Pad = 0, Reflect = 1, Repeat = 2 }
    public class TileBrush : Brush {}
    public class ImageBrush : TileBrush { public ImageSource ImageSource { get; set; } }
    public class VisualBrush : TileBrush {}
    public class DrawingBrush : TileBrush {}
    public class Visual : DependencyObject {}
    public abstract class Transform : Freezable { public abstract Matrix Value { get; } public abstract Transform Inverse { get; } }
    public class MatrixTransform : Transform { public MatrixTransform() {} public MatrixTransform(Matrix m) { Matrix = m; } public MatrixTransform(double m11, double m12, double m21, double m22, double oX, double oY) { Matrix = new Matrix(m11,m12,m21,m22,oX,oY); } public override Matrix Value => Matrix; public override Transform Inverse => this; public Matrix Matrix { get; set; } = Matrix.Identity; public double M11 { get=>Matrix.M11; set{var m=Matrix; m.M11=value; Matrix=m;} } public double M12 { get=>Matrix.M12; set{var m=Matrix; m.M12=value; Matrix=m;} } public double M21 { get=>Matrix.M21; set{var m=Matrix; m.M21=value; Matrix=m;} } public double M22 { get=>Matrix.M22; set{var m=Matrix; m.M22=value; Matrix=m;} } public double OffsetX { get=>Matrix.OffsetX; set{var m=Matrix; m.OffsetX=value; Matrix=m;} } public double OffsetY { get=>Matrix.OffsetY; set{var m=Matrix; m.OffsetY=value; Matrix=m;} } }
    public class TranslateTransform : Transform { public TranslateTransform() {} public TranslateTransform(double x, double y) { X=x; Y=y; } public double X { get; set; } public double Y { get; set; } public override Matrix Value => Matrix.Identity; public override Transform Inverse => this; }
    public class ScaleTransform : Transform { public ScaleTransform() {} public ScaleTransform(double sx, double sy) { ScaleX=sx; ScaleY=sy; } public ScaleTransform(double sx, double sy, double cx, double cy) { ScaleX=sx; ScaleY=sy; CenterX=cx; CenterY=cy; } public double ScaleX { get; set; } = 1; public double ScaleY { get; set; } = 1; public double CenterX { get; set; } public double CenterY { get; set; } public override Matrix Value => Matrix.Identity; public override Transform Inverse => this; }
    public class RotateTransform : Transform { public RotateTransform() {} public RotateTransform(double angle) { Angle=angle; } public RotateTransform(double angle, double cx, double cy) { Angle=angle; CenterX=cx; CenterY=cy; } public double Angle { get; set; } public double CenterX { get; set; } public double CenterY { get; set; } public override Matrix Value => Matrix.Identity; public override Transform Inverse => this; }
    public class SkewTransform : Transform { public double AngleX { get; set; } public double AngleY { get; set; } public double CenterX { get; set; } public double CenterY { get; set; } public override Matrix Value => Matrix.Identity; public override Transform Inverse => this; }
    public class TransformGroup : Transform { public TransformCollection Children { get; set; } = new TransformCollection(); public override Matrix Value => Matrix.Identity; public override Transform Inverse => this; }
    public class TransformCollection : System.Collections.Generic.List<Transform> {}
    public struct Matrix { public double M11,M12,M21,M22,OffsetX,OffsetY; public static Matrix Identity = new Matrix(1,0,0,1,0,0); public bool IsIdentity => M11==1 && M12==0 && M21==0 && M22==1 && OffsetX==0 && OffsetY==0; public Matrix(double m11, double m12, double m21, double m22, double ox, double oy) { M11=m11; M12=m12; M21=m21; M22=m22; OffsetX=ox; OffsetY=oy; } public void SetIdentity() { this=Identity; } public void Translate(double x, double y) { OffsetX+=x; OffsetY+=y; } public void Scale(double sx, double sy) { M11*=sx; M12*=sx; M21*=sy; M22*=sy; OffsetX*=sx; OffsetY*=sy; } public void Rotate(double angle) { double r = angle*Math.PI/180.0; double c=Math.Cos(r), s=Math.Sin(r); var n = new Matrix(M11*c+M12*s, M11*(-s)+M12*c, M21*c+M22*s, M21*(-s)+M22*c, OffsetX*c+OffsetY*s, OffsetX*(-s)+OffsetY*c); this=n; } public Point Transform(Point p) => new Point(M11*p.X + M12*p.Y + OffsetX, M21*p.X + M22*p.Y + OffsetY); public static Matrix operator *(Matrix a, Matrix b) { return new Matrix(a.M11*b.M11+a.M12*b.M21, a.M11*b.M12+a.M12*b.M22, a.M21*b.M11+a.M22*b.M21, a.M21*b.M12+a.M22*b.M22, a.M11*b.OffsetX+a.M12*b.OffsetY+a.OffsetX, a.M21*b.OffsetX+a.M22*b.OffsetY+a.OffsetY); } }
    public interface IValueConverter { object Convert(object value, Type targetType, object parameter, CultureInfo culture); object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture); }
    public interface IMultiValueConverter { object Convert(object[] values, Type targetType, object parameter, CultureInfo culture); object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture); }
    public interface ICommand { event EventHandler CanExecuteChanged; bool CanExecute(object parameter); void Execute(object parameter); }
    public sealed class DelegateCommand : ICommand { private readonly Action<object> _exec; private readonly Func<object,bool> _can; public DelegateCommand(Action<object> e, Func<object,bool> c=null) { _exec=e; _can=c; } public event EventHandler CanExecuteChanged; public bool CanExecute(object p) => _can?.Invoke(p) ?? true; public void Execute(object p) => _exec?.Invoke(p); public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    public interface INotifyPropertyChanged { event System.ComponentModel.PropertyChangedEventHandler PropertyChanged; }
    public abstract class ObservableObject : INotifyPropertyChanged { public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged; protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name)); protected bool Set<T>(ref T f, T v, [System.Runtime.CompilerServices.CallerMemberName] string n=null) { if(EqualityComparer<T>.Default.Equals(f,v)) return false; f=v; OnPropertyChanged(n); return true; } }
    public class ObservableCollection<T> : System.Collections.ObjectModel.Collection<T>, System.Collections.Specialized.INotifyCollectionChanged { public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged; protected override void InsertItem(int i, T item) { base.InsertItem(i, item); CollectionChanged?.Invoke(this, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, item, i)); } protected override void RemoveItem(int i) { var item = this[i]; base.RemoveItem(i); CollectionChanged?.Invoke(this, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Remove, item, i)); } protected override void ClearItems() { base.ClearItems(); CollectionChanged?.Invoke(this, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset)); } }
    public class DataTemplateSelector { public virtual DataTemplate SelectTemplate(object item, DependencyObject container) => null; }
    public class HierarchicalDataTemplate : DataTemplate {}
    public class Binding : BindingBase { public Binding() {} public Binding(string path) { Path = path; } public string Path { get; set; } public BindingMode Mode { get; set; } public UpdateSourceTrigger UpdateSourceTrigger { get; set; } public object FallbackValue { get; set; } public object TargetNullValue { get; set; } public IValueConverter Converter { get; set; } public object ConverterParameter { get; set; } public string ElementName { get; set; } public RelativeSource RelativeSource { get; set; } public object Source { get; set; } }
    public class BindingBase {}
    public enum BindingMode { Default = 0, OneWay = 1, OneTime = 2, OneWayToSource = 3, TwoWay = 4 }
    public enum UpdateSourceTrigger { Default = 0, PropertyChanged = 1, LostFocus = 2, Explicit = 3 }
    public class RelativeSource { public RelativeSource() {} public RelativeSource(RelativeSourceMode mode) { Mode = mode; } public RelativeSourceMode Mode { get; set; } public Type AncestorType { get; set; } public int AncestorLevel { get; set; } = 1; public static RelativeSource TemplatedParent => new RelativeSource(RelativeSourceMode.TemplatedParent); public static RelativeSource Self => new RelativeSource(RelativeSourceMode.Self); public static RelativeSource PreviousData => new RelativeSource(RelativeSourceMode.PreviousData); }
    public enum RelativeSourceMode { PreviousData = 0, TemplatedParent = 1, Self = 2, FindAncestor = 3 }
    public class MultiBinding : BindingBase { public BindingCollection Bindings { get; } = new BindingCollection(); public IMultiValueConverter Converter { get; set; } public object ConverterParameter { get; set; } public BindingMode Mode { get; set; } public UpdateSourceTrigger UpdateSourceTrigger { get; set; } }
    public class BindingCollection : System.Collections.ObjectModel.Collection<BindingBase> {}
    public class PriorityBinding : BindingBase { public BindingCollection Bindings { get; } = new BindingCollection(); }
    public class PropertyPath { public PropertyPath(string path) { Path = path; } public PropertyPath(object dataItem, params string[] path) {} public string Path { get; set; } }
    public sealed class TemplateBindingExtension : Markup.MarkupExtension { public TemplateBindingExtension() {} public TemplateBindingExtension(DependencyProperty p) { Property = p; } public DependencyProperty Property { get; set; } public Type ConverterType { get; set; } public IValueConverter Converter { get; set; } public object ConverterParameter { get; set; } public override object ProvideValue(IServiceProvider sp) => this; }
    public interface ISupportInitialize { void BeginInit(); void EndInit(); }
    public interface INameScope { object FindName(string name); void RegisterName(string name, object scopedElement); void UnregisterName(string name); }
    public class NameScope : INameScope { public object FindName(string name) => null; public void RegisterName(string name, object scopedElement) {} public void UnregisterName(string name) {} }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)] public sealed class TypeForwardedToAttribute : Attribute { public TypeForwardedToAttribute(Type d) { Destination = d; } public Type Destination { get; } }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)] public sealed class DesignerSerializationVisibilityAttribute : Attribute { public DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility v) { Visibility = v; } public DesignerSerializationVisibility Visibility { get; } }
    public enum DesignerSerializationVisibility { Hidden = 0, Visible = 1, Content = 2 }
    public class Uri { public Uri(string uriString) { OriginalString = uriString; } public Uri(Uri baseUri, string relativeUri) { OriginalString = baseUri.OriginalString + "/" + relativeUri; } public string OriginalString { get; } public override string ToString() => OriginalString; public static implicit operator Uri(string s) => new Uri(s); public string AbsoluteUri => OriginalString; public bool IsAbsoluteUri => true; public string LocalPath => OriginalString; public string Authority => ""; public string Scheme => "pack"; }
}

namespace System.Windows.Markup
{
    using System.Windows;
    public abstract class MarkupExtension { public abstract object ProvideValue(IServiceProvider serviceProvider); }
    public class StaticResourceExtension : MarkupExtension { public StaticResourceExtension() {} public StaticResourceExtension(object resourceKey) { ResourceKey = resourceKey; } public object ResourceKey { get; set; } public override object ProvideValue(IServiceProvider sp) => null; }
    public class DynamicResourceExtension : MarkupExtension { public DynamicResourceExtension() {} public DynamicResourceExtension(object resourceKey) { ResourceKey = resourceKey; } public object ResourceKey { get; set; } public override object ProvideValue(IServiceProvider sp) => null; }
    public class BindingExtension : MarkupExtension { public BindingExtension() {} public BindingExtension(string path) { Path = path; } public string Path { get; set; } public BindingMode Mode { get; set; } public IValueConverter Converter { get; set; } public object ConverterParameter { get; set; } public override object ProvideValue(IServiceProvider sp) => new Binding(Path) { Mode = Mode, Converter = Converter, ConverterParameter = ConverterParameter }; }
    public class XamlDeferLoadAttribute : Attribute { public XamlDeferLoadAttribute(Type c, Type t) { ContentType = c; LoaderType = t; } public Type ContentType { get; } public Type LoaderType { get; } }
    public class XamlSetMarkupExtensionAttribute : Attribute { public XamlSetMarkupExtensionAttribute(string h) { XamlSetMarkupExtensionHandler = h; } public string XamlSetMarkupExtensionHandler { get; } }
    public class XamlSetTypeConverterAttribute : Attribute { public XamlSetTypeConverterAttribute(string h) { XamlSetTypeConverterHandler = h; } public string XamlSetTypeConverterHandler { get; } }
    public interface IUriContext { Uri BaseUri { get; set; } }
    public interface IProvideValueTarget { object TargetObject { get; } public object TargetProperty { get; } }
    public interface IXamlTypeResolver { Type Resolve(string qualifiedTypeName); }
    public interface INameScopeDictionary : System.Collections.IDictionary, INameScope {}
    public class RootObject { }
    public class AmbientAttribute : Attribute { }
    public class ConstructorArgumentAttribute : Attribute { public ConstructorArgumentAttribute(string name) { ArgumentName = name; } public string ArgumentName { get; } }
    public class ContentPropertyAttribute : Attribute { public ContentPropertyAttribute() {} public ContentPropertyAttribute(string name) { Name = name; } public string Name { get; } }
    public class ContentWrapperAttribute : Attribute { public ContentWrapperAttribute(Type t) { ContentWrapper = t; } public Type ContentWrapper { get; } }
    public class DependsOnAttribute : Attribute { public DependsOnAttribute(string name) { Name = name; } public string Name { get; } }
    public class DesignTimeResourceDictionaryAttribute : Attribute { }
    public class DictionaryKeyPropertyAttribute : Attribute { public DictionaryKeyPropertyAttribute(string n) { Name = n; } public string Name { get; } }
    public class AcceptedMarkupExtensionExpressionTypeAttribute : Attribute { public AcceptedMarkupExtensionExpressionTypeAttribute(Type t) { Type = t; } public Type Type { get; } }
    public class MemberDefinition { }
    public interface IAttachedPropertyStore { bool StoreProperty(AttachableMemberIdentifier id, object value); bool RemoveProperty(AttachableMemberIdentifier id); int PropertyCount { get; } System.Collections.Generic.IEnumerator<AttachableMemberIdentifier> EnumerateProperties(); }
    public class AttachableMemberIdentifier { public AttachableMemberIdentifier(Type t, string n) { DeclaringType = t; MemberName = n; } public Type DeclaringType { get; } public string MemberName { get; } }
}

namespace System.Windows.Input
{
    using System.Windows;
    public class RoutedCommand : ICommand { public RoutedCommand() {} public RoutedCommand(string name, Type owner) {} public string Name { get; set; } public Type OwnerType { get; set; } public InputGestureCollection InputGestures { get; } = new InputGestureCollection(); public event EventHandler CanExecuteChanged { add {} remove {} } public bool CanExecute(object parameter) => true; public void Execute(object parameter) {} }
    public class RoutedUICommand : RoutedCommand { public RoutedUICommand() {} public RoutedUICommand(string text, string name, Type owner) { Text = text; Name = name; OwnerType = owner; } public string Text { get; set; } }
    public class InputGestureCollection : System.Collections.ObjectModel.Collection<InputGesture> {}
    public abstract class InputGesture {}
    public class KeyGesture : InputGesture { public KeyGesture(Key key) { Key = key; } public KeyGesture(Key key, ModifierKeys modifiers) { Key = key; Modifiers = modifiers; } public Key Key { get; set; } public ModifierKeys Modifiers { get; set; } public string DisplayString { get; set; } }
    public class MouseGesture : InputGesture { public MouseGesture(MouseAction a) { MouseAction = a; } public MouseAction MouseAction { get; set; } public ModifierKeys Modifiers { get; set; } }
    public enum MouseAction { None = 0, LeftClick = 1, RightClick = 2, MiddleClick = 3, WheelClick = 4, LeftDoubleClick = 5, RightDoubleClick = 6, MiddleDoubleClick = 7 }
    public static class ApplicationCommands { public static RoutedUICommand None => new RoutedUICommand(); public static RoutedUICommand CancelPrint => new RoutedUICommand(); public static RoutedUICommand Close => new RoutedUICommand(); public static RoutedUICommand ContextMenu => new RoutedUICommand(); public static RoutedUICommand Copy => new RoutedUICommand(); public static RoutedUICommand CorrectionList => new RoutedUICommand(); public static RoutedUICommand Cut => new RoutedUICommand(); public static RoutedUICommand Delete => new RoutedUICommand(); public static RoutedUICommand Find => new RoutedUICommand(); public static RoutedUICommand Help => new RoutedUICommand(); public static RoutedUICommand New => new RoutedUICommand(); public static RoutedUICommand Open => new RoutedUICommand(); public static RoutedUICommand Paste => new RoutedUICommand(); public static RoutedUICommand Print => new RoutedUICommand(); public static RoutedUICommand PrintPreview => new RoutedUICommand(); public static RoutedUICommand Properties => new RoutedUICommand(); public static RoutedUICommand Redo => new RoutedUICommand(); public static RoutedUICommand Replace => new RoutedUICommand(); public static RoutedUICommand Save => new RoutedUICommand(); public static RoutedUICommand SaveAs => new RoutedUICommand(); public static RoutedUICommand SelectAll => new RoutedUICommand(); public static RoutedUICommand Stop => new RoutedUICommand(); public static RoutedUICommand Undo => new RoutedUICommand(); }
    public static class ComponentCommands { public static RoutedUICommand MoveDown => new RoutedUICommand(); public static RoutedUICommand MoveLeft => new RoutedUICommand(); public static RoutedUICommand MoveRight => new RoutedUICommand(); public static RoutedUICommand MoveUp => new RoutedUICommand(); public static RoutedUICommand MoveToEnd => new RoutedUICommand(); public static RoutedUICommand MoveToHome => new RoutedUICommand(); public static RoutedUICommand MoveToPageDown => new RoutedUICommand(); public static RoutedUICommand MoveToPageUp => new RoutedUICommand(); public static RoutedUICommand ScrollByLine => new RoutedUICommand(); public static RoutedUICommand ScrollPageDown => new RoutedUICommand(); public static RoutedUICommand ScrollPageLeft => new RoutedUICommand(); public static RoutedUICommand ScrollPageRight => new RoutedUICommand(); public static RoutedUICommand ScrollPageUp => new RoutedUICommand(); public static RoutedUICommand SelectToEnd => new RoutedUICommand(); public static RoutedUICommand SelectToHome => new RoutedUICommand(); public static RoutedUICommand SelectToPageDown => new RoutedUICommand(); public static RoutedUICommand SelectToPageUp => new RoutedUICommand(); public static RoutedUICommand ExtendSelectionDown => new RoutedUICommand(); public static RoutedUICommand ExtendSelectionLeft => new RoutedUICommand(); public static RoutedUICommand ExtendSelectionRight => new RoutedUICommand(); public static RoutedUICommand ExtendSelectionUp => new RoutedUICommand(); }
    public static class NavigationCommands { public static RoutedUICommand BrowseBack => new RoutedUICommand(); public static RoutedUICommand BrowseForward => new RoutedUICommand(); public static RoutedUICommand BrowseHome => new RoutedUICommand(); public static RoutedUICommand BrowseStop => new RoutedUICommand(); public static RoutedUICommand BrowseRefresh => new RoutedUICommand(); public static RoutedUICommand Favorites => new RoutedUICommand(); public static RoutedUICommand FirstPage => new RoutedUICommand(); public static RoutedUICommand GoToPage => new RoutedUICommand(); public static RoutedUICommand GoToRequest => new RoutedUICommand(); public static RoutedUICommand IncreaseZoom => new RoutedUICommand(); public static RoutedUICommand LastPage => new RoutedUICommand(); public static RoutedUICommand NavigateJournal => new RoutedUICommand(); public static RoutedUICommand NextPage => new RoutedUICommand(); public static RoutedUICommand PreviousPage => new RoutedUICommand(); public static RoutedUICommand Zoom => new RoutedUICommand(); public static RoutedUICommand ZoomByValue => new RoutedUICommand(); public static RoutedUICommand DecreaseZoom => new RoutedUICommand(); public static RoutedUICommand NavigateUri => new RoutedUICommand(); public static RoutedUICommand NavigateSearch => new RoutedUICommand(); }
    public static class EditingCommands { public static RoutedUICommand AlignCenter => new RoutedUICommand(); public static RoutedUICommand AlignJustify => new RoutedUICommand(); public static RoutedUICommand AlignLeft => new RoutedUICommand(); public static RoutedUICommand AlignRight => new RoutedUICommand(); public static RoutedUICommand Backspace => new RoutedUICommand(); public static RoutedUICommand CorrectSpellingError => new RoutedUICommand(); public static RoutedUICommand DecreaseFontSize => new RoutedUICommand(); public static RoutedUICommand DecreaseIndentation => new RoutedUICommand(); public static RoutedUICommand Delete => new RoutedUICommand(); public static RoutedUICommand DeleteNextWord => new RoutedUICommand(); public static RoutedUICommand DeletePreviousWord => new RoutedUICommand(); public static RoutedUICommand EnterLineBreak => new RoutedUICommand(); public static RoutedUICommand EnterParagraphBreak => new RoutedUICommand(); public static RoutedUICommand IgnoreSpellingError => new RoutedUICommand(); public static RoutedUICommand IncreaseFontSize => new RoutedUICommand(); public static RoutedUICommand IncreaseIndentation => new RoutedUICommand(); public static RoutedUICommand MoveDownByLine => new RoutedUICommand(); public static RoutedUICommand MoveDownByParagraph => new RoutedUICommand(); public static RoutedUICommand MoveLeftByCharacter => new RoutedUICommand(); public static RoutedUICommand MoveLeftByWord => new RoutedUICommand(); public static RoutedUICommand MoveRightByCharacter => new RoutedUICommand(); public static RoutedUICommand MoveRightByWord => new RoutedUICommand(); public static RoutedUICommand MoveToDocumentEnd => new RoutedUICommand(); public static RoutedUICommand MoveToDocumentStart => new RoutedUICommand(); public static RoutedUICommand MoveToLineEnd => new RoutedUICommand(); public static RoutedUICommand MoveToLineStart => new RoutedUICommand(); public static RoutedUICommand MoveUpByLine => new RoutedUICommand(); public static RoutedUICommand MoveUpByParagraph => new RoutedUICommand(); public static RoutedUICommand ToggleBullet => new RoutedUICommand(); public static RoutedUICommand ToggleBold => new RoutedUICommand(); public static RoutedUICommand ToggleInsert => new RoutedUICommand(); public static RoutedUICommand ToggleItalic => new RoutedUICommand(); public static RoutedUICommand ToggleNumbering => new RoutedUICommand(); public static RoutedUICommand ToggleSubscript => new RoutedUICommand(); public static RoutedUICommand ToggleSuperscript => new RoutedUICommand(); public static RoutedUICommand ToggleUnderline => new RoutedUICommand(); public static RoutedUICommand MoveDownByPage => new RoutedUICommand(); public static RoutedUICommand MoveUpByPage => new RoutedUICommand(); public static RoutedUICommand MoveLeftByPage => new RoutedUICommand(); public static RoutedUICommand MoveRightByPage => new RoutedUICommand(); }
    public class InputBinding { public InputBinding() {} public InputBinding(ICommand cmd, InputGesture gesture) { Command = cmd; Gesture = gesture; } public ICommand Command { get; set; } public InputGesture Gesture { get; set; } public object CommandParameter { get; set; } }
    public class KeyBinding : InputBinding { public KeyBinding() {} public KeyBinding(ICommand cmd, KeyGesture gesture) : base(cmd, gesture) {} public Key Key { get; set; } public ModifierKeys Modifiers { get; set; } }
    public class MouseBinding : InputBinding { public MouseBinding() {} public MouseBinding(ICommand cmd, MouseGesture gesture) : base(cmd, gesture) {} public MouseAction MouseAction { get; set; } public ModifierKeys Modifiers { get; set; } }
    public class InputBindingCollection : System.Collections.ObjectModel.Collection<InputBinding> {}
    public class CommandBinding { public CommandBinding() {} public CommandBinding(ICommand cmd) { Command = cmd; } public CommandBinding(ICommand cmd, ExecutedRoutedEventHandler h) : this(cmd) { Executed += h; } public ICommand Command { get; set; } public event ExecutedRoutedEventHandler Executed; public event CanExecuteRoutedEventHandler CanExecute; public event ExecutedRoutedEventHandler PreviewExecuted; public event CanExecuteRoutedEventHandler PreviewCanExecute; }
    public delegate void ExecutedRoutedEventHandler(object sender, ExecutedRoutedEventArgs e);
    public delegate void CanExecuteRoutedEventHandler(object sender, CanExecuteRoutedEventArgs e);
    public class ExecutedRoutedEventArgs : RoutedEventArgs { public ICommand Command { get; set; } public object Parameter { get; set; } }
    public class CanExecuteRoutedEventArgs : RoutedEventArgs { public ICommand Command { get; set; } public object Parameter { get; set; } public bool CanExecute { get; set; } public bool ContinueRouting { get; set; } }
    public class CommandBindingCollection : System.Collections.ObjectModel.Collection<CommandBinding> {}
    public static class CommandManager { public static event EventHandler RequerySuggested; public static void InvalidateRequerySuggested() => RequerySuggested?.Invoke(null, EventArgs.Empty); public static void RegisterClassCommandBinding(Type type, CommandBinding b) {} public static void RegisterClassInputBinding(Type type, InputBinding b) {} }
    public enum CaptureMode { None = 0, Element = 1, SubTree = 2 }
    public class InputMethod {}
    public class StylusDevice {}
    public class InputManager {}
    public class FocusManager { public static readonly DependencyProperty FocusedElementProperty = DependencyProperty.Register("FocusedElement", typeof(IInputElement), typeof(FocusManager)); public static IInputElement GetFocusedElement(DependencyObject element) => null; public static void SetFocusedElement(DependencyObject element, IInputElement value) {} public static readonly DependencyProperty IsFocusScopeProperty = DependencyProperty.Register("IsFocusScope", typeof(bool), typeof(FocusManager)); public static bool GetIsFocusScope(DependencyObject element) => false; public static void SetIsFocusScope(DependencyObject element, bool value) {} }
}

namespace System.IO.Packaging
{
    using System;
    using System.IO;

    public abstract class Package : IDisposable
    {
        public PackageProperties PackageProperties { get; } = new PackageProperties();
        public FileAccess FileOpenAccess { get; } = FileAccess.ReadWrite;
        public static Package Open(string path) => new ZipPackage(File.OpenRead(path));
        public static Package Open(string path, FileMode mode) => new ZipPackage(new FileStream(path, mode));
        public static Package Open(string path, FileMode mode, FileAccess access) => new ZipPackage(new FileStream(path, mode, access));
        public static Package Open(Stream stream) => new ZipPackage(stream);
        public static Package Open(Stream stream, FileMode mode, FileAccess access) => new ZipPackage(stream);
        public virtual void Close() { Dispose(); }
        public void Dispose() { }
    }
    public class ZipPackage : Package { public ZipPackage(Stream s) {} }
    public class PackageProperties : IDisposable { public string Creator { get; set; } public string Title { get; set; } public string Subject { get; set; } public string Description { get; set; } public string Publisher { get; set; } public string Identifier { get; set; } public string Language { get; set; } public string Keywords { get; set; } public string ContentType { get; set; } public string Category { get; set; } public string Version { get; set; } public string Revision { get; set; } public DateTime? Created { get; set; } public DateTime? Modified { get; set; } public void Dispose() {} }
    public class PackUriHelper { public static Uri Create(Uri packageUri, Uri partUri) => packageUri; public static bool TryGetPartUri(Uri uri, out Uri partUri) { partUri = null; return false; } }
}

namespace System.IO.Packaging
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PackagePart
    {
        public Uri Uri { get; }
        public string ContentType { get; }
        public Package PartPackage { get; }
        public PackagePart(Package pkg, Uri uri, string contentType) { PartPackage = pkg; Uri = uri; ContentType = contentType; }
        public virtual Stream GetStream() => new MemoryStream();
        public virtual Stream GetStream(FileMode mode) => new MemoryStream();
        public virtual Stream GetStream(FileMode mode, FileAccess access) => new MemoryStream();
    }
}

namespace System.Xaml
{
    public class XamlReader { public static object Load(Stream s) => null; public static object Load(Stream s, bool useDefaults) => null; public static object Load(XamlReader reader) => null; public static object LoadXml(string xml) => null; }
    public class XamlWriter { public static string Save(object obj) => ""; public static void Save(object obj, Stream s) {} }
    public class XamlParseException : Exception { public XamlParseException() {} public XamlParseException(string message) : base(message) {} public int LineNumber { get; set; } public int LinePosition { get; set; } }
}

namespace System.Windows.Resources
{
    public static class ApplicationResourceLoader { public static Stream LoadResource(Uri uri) => null; }
}

namespace Microsoft.Win32
{
    public class OpenFileDialog : FileDialog { public bool Multiselect { get; set; } public string[] FileNames => new[] { FileName }; public bool? ShowDialog() => false; }
    public class SaveFileDialog : FileDialog { public bool OverwritePrompt { get; set; } = true; public bool CreatePrompt { get; set; } public bool? ShowDialog() => false; }
    public abstract class FileDialog : CommonDialog { public string Title { get; set; } public string Filter { get; set; } public int FilterIndex { get; set; } = 1; public string DefaultExt { get; set; } public string FileName { get; set; } public bool CheckFileExists { get; set; } public bool CheckPathExists { get; set; } public string InitialDirectory { get; set; } public bool RestoreDirectory { get; set; } public bool AddExtension { get; set; } = true; public bool DereferenceLinks { get; set; } = true; public string SafeFileName { get; set; } public override bool? ShowDialog() => false; }
    public abstract class CommonDialog { public abstract bool? ShowDialog(); public virtual void Reset() {} protected virtual IntPtr OwnerWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) => IntPtr.Zero; }
    public class FolderBrowserDialog : CommonDialog { public string Description { get; set; } public string SelectedPath { get; set; } public bool ShowNewFolderButton { get; set; } = true; public Environment.SpecialFolder RootFolder { get; set; } = Environment.SpecialFolder.Desktop; public override bool? ShowDialog() => false; }
}

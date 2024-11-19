using System.Xml;

using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Platforms.Skia;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using SkiaSharp;

using MouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;
using Vector2 = Natsu.Mathematics.Vector2;

namespace Natsu.Sandbox;

public class AppWindow() : GameWindow(new GameWindowSettings { UpdateFrequency = 500 }, new NativeWindowSettings { ClientSize = new Vector2i(800, 600), Title = "Natsu Sandbox", Vsync = VSyncMode.On }) {

    private readonly List<float> fps = new();
    public List<OffscreenSurfaceElement> LayerSurfaces = new();

    public void CreateSurface(int width, int height) {
        lock (this) {
            _surface?.Dispose();

            _target = new GRBackendRenderTarget(width, height, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            _surface = SKSurface.Create(_context, _target, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

            if (App == null)
                App = new MyApp(_surface);
            else
                App.LoadRenderer(_surface);
        }
    }

    public void ResizeApp() {
        TryGetCurrentMonitorScale(out float scaleX, out float scaleY);
        // App.Root.Scale = new Vector2(scaleX, scaleY);
        App.Root.Size = new Vector2(Size.X * scaleX, Size.Y * scaleY);
    }

    protected override void OnLoad() {
        base.OnLoad();

        _interface = GRGlInterface.Create();
        _context = GRContext.CreateGl(_interface);
        CreateSurface(Size.X, Size.Y);

        App.Load();
        ResizeApp();
    }


    public void LoadMaps() {
        XmlDocument doc = new();
        doc.Load("untitled.tmx");
        XmlNodeList tilesets = doc.GetElementsByTagName("tileset");

        XmlNode map = doc.GetElementsByTagName("map")[0]!;
        int width = int.Parse(map.Attributes!["width"]!.Value);
        int height = int.Parse(map.Attributes!["height"]!.Value);
        int tileWidth = int.Parse(map.Attributes!["tilewidth"]!.Value);
        int tileHeight = int.Parse(map.Attributes!["tileheight"]!.Value);

        Dictionary<int, IOffscreenSurface> tiles = new();

        // test.Clear(SKColors.White);
        foreach (XmlNode tileset in tilesets) {
            Console.WriteLine(tileset.Attributes!["firstgid"]!.Value);
            string source = tileset.Attributes["source"]!.Value;

            XmlDocument tilesetDoc = new();
            tilesetDoc.Load(source);

            XmlNode tileconf = tilesetDoc.GetElementsByTagName("tileset")[0]!;
            XmlNode image = tileconf["image"]!;
            string imagesource = image.Attributes!["source"]!.Value;
            int firstGid = int.Parse(tileset.Attributes!["firstgid"]!.Value);

            int confTileWidth = int.Parse(tileconf.Attributes!["tilewidth"]!.Value);
            int confTileHeight = int.Parse(tileconf.Attributes!["tileheight"]!.Value);
            Console.WriteLine($"Tile Width: {confTileWidth}, Tile Height: {confTileHeight}");

            SKBitmap bitmap = SKBitmap.Decode(imagesource);
            bitmap.SetImmutable();

            int c = 0;

            for (int y = 0; y < bitmap.Height; y += confTileHeight) {
                Console.WriteLine($"Added tiles: {tiles.Count}");
                for (int x = 0; x < bitmap.Width; x += confTileWidth) {
                    SKBitmap tile = new();

                    if (!bitmap.ExtractSubset(tile, new SKRectI(x, y, x + confTileWidth, y + confTileHeight))) {
                        Console.WriteLine("Failed to extract subset");
                        continue;
                    }

                    SkiaOffscreenSurface surface = new(confTileWidth, confTileHeight);
                    SkiaImage img = new(SKImage.FromBitmap(tile));

                    surface.Canvas.DrawImage(img, new Vector2(0, 0), new Paint());
                    surface.Flush();

                    tiles.Add(c + firstGid, surface);
                    c++;
                }
            }
        }

        XmlNodeList layers = doc.GetElementsByTagName("layer");
        foreach (XmlNode layer in layers) {
            XmlNode data = layer["data"]!;
            string encoding = data.Attributes!["encoding"]!.Value;
            string content = data.InnerText;
            int w = int.Parse(layer.Attributes!["width"]!.Value);
            int h = int.Parse(layer.Attributes!["height"]!.Value);

            if (encoding == "csv") {
                IOffscreenSurface layerSurface = new SkiaOffscreenSurface(w * tileWidth, h * tileHeight);
                string[] tilesData = content.Split(',');
                for (int i = 0; i < tilesData.Length; i++) {
                    int tileId = int.Parse(tilesData[i]);
                    if (tileId == 0) continue;

                    Console.WriteLine($"Drawing tile {tileId}, count: {tiles.Count}");
                    int x = i % w;
                    int y = i / w;

                    layerSurface.Canvas.DrawOffscreenSurface(tiles[tileId], new Vector2(x * tileWidth, y * tileHeight));
                }

                layerSurface.Flush();

                OffscreenSurfaceElement elm = new(layerSurface) {
                    Index = 99999,
                    Scale = new Vector2(1),
                    RelativeSizeAxes = Axes.Both,
                    RenderSurface = false,
                    ImageScaling = true
                };
                LayerSurfaces.Add(elm);
                App.Root.Add(elm);
            }
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        App.Render();

        fps.Add((float)(1 / args.Time));
        if (fps.Count > 0) {
            const float div = 2;
            while (fps.Count > Size.X / div) fps.RemoveAt(0);

            SKPath path = new();
            float max = fps.Max();
            float min = fps.Min();
            float avg = fps.Average();
            const float graphHeight = 100;

            path.MoveTo(0, 0);
            for (int i = 0; i < fps.Count; i++) {
                float x = i * div;
                float y = graphHeight - fps[i] / max * graphHeight;
                path.LineTo(x, y);
            }

            path.LineTo(fps.Count * div, 0);
            path.Close();

            App.Canvas.DrawPath(new VectorPath(path), new Paint { Color = SKColors.White, IsStroke = true, StrokeWidth = 1 });
            SkiaCanvas canvas = (SkiaCanvas)App.Canvas;
            canvas.Canvas.DrawText($"FPS: {avg}, Min: {min}, Max: {max}", 0, graphHeight + 12, new SKPaint { Color = SKColors.White, TextSize = 12 });
            App.Renderer.Flush();
        }


        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args) {
        base.OnUpdateFrame(args);
        App.Update();
    }

    protected override void OnResize(ResizeEventArgs e) {
        float clampWidth = Math.Max(e.Width, 1);
        float clampHeight = Math.Max(e.Height, 1);

        CreateSurface((int)clampWidth, (int)clampHeight);
        ResizeApp();
    }

    public Input.MouseButton ConvertButton(MouseButton button) => button switch {
        MouseButton.Left => Input.MouseButton.Left,
        MouseButton.Right => Input.MouseButton.Right,
        MouseButton.Middle => Input.MouseButton.Middle,
        MouseButton.Button4 => Input.MouseButton.X1,
        MouseButton.Button5 => Input.MouseButton.X2,
        _ => Input.MouseButton.Unknown
    };

    protected override void OnMouseDown(MouseButtonEventArgs e) => App.MouseDown(ConvertButton(e.Button), new Vector2(MouseState.X, MouseState.Y));
    protected override void OnMouseUp(MouseButtonEventArgs e) => App.MouseUp(ConvertButton(e.Button), new Vector2(MouseState.X, MouseState.Y));
    protected override void OnMouseMove(MouseMoveEventArgs e) => App.MouseMove(new Vector2(MouseState.X, MouseState.Y));
    protected override void OnMouseWheel(MouseWheelEventArgs e) => App.MouseWheel(new Vector2(e.Offset.X, e.Offset.Y));

    protected override void OnKeyDown(KeyboardKeyEventArgs e) {
        if (e.IsRepeat) return;

        App.KeyDown((Key)e.Key);
    }

    protected override void OnKeyUp(KeyboardKeyEventArgs e) => App.KeyUp((Key)e.Key);


#nullable disable
    public MyApp App;
    private GRContext _context;
    private GRGlInterface _interface;
    private GRBackendRenderTarget _target;
    private SKSurface _surface;

#nullable restore
}

public static class Program {
    // public static void LogSKMatrix(SKMatrix matrix) {
    //     for (int i = 0; i < 3; i++) Console.WriteLine($"[{matrix.Values[i * 3]}, {matrix.Values[i * 3 + 1]}, {matrix.Values[i * 3 + 2]}]");
    // }

    // public static void LogMatrix(Matrix matrix) {
    //     for (int i = 0; i < 3; i++) Console.WriteLine($"[{matrix.Values[i, 0]}, {matrix.Values[i, 1]}, {matrix.Values[i, 2]}]");
    // }

    // 2 0 10 cos(0) -sin(0) 10
    // 0 2 10 sin(0) cos(0) 10
    // 0 0 1

    // m1 m2 m3
    // m4 m5 m6
    // m7 m8 m9

    // x
    // y
    // 1

    // x = m1 * x + m2 * y + m3

    public static void Main(string[] args) =>
        // Matrix testMatrix = new();
        // Matrix matrix = new();
        // testMatrix.PreRotate(45, -10, -10);
        // matrix.PreRotate(45, -10, -10);
        // testMatrix.PreRotate(45, 10, 50);
        // matrix.PreRotate(45, 10, 50);
        // testMatrix.PreScale(2, 2);
        // matrix.PreScale(2, 2);
        // testMatrix.PreTranslate(10, 10);
        // matrix.PreTranslate(10, 10);
        // testMatrix.PreSkew(10, 10);
        // matrix.PreSkew(10, 10);
        // LogSKMatrix(testMatrix);
        // Console.WriteLine();
        // LogMatrix(matrix);
        new AppWindow().Run();
}
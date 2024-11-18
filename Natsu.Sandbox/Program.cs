using System.Xml;

using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Platforms.Skia;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using SkiaSharp;

using MouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;

namespace Natsu.Sandbox;

public class AppWindow() : GameWindow(new GameWindowSettings { UpdateFrequency = 5 }, NativeWindowSettings.Default) {
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

            TryGetCurrentMonitorScale(out float dpiX, out float dpiY);
            App.Root.Scale = new Vector2(dpiX, dpiY);
        }
    }

    protected override void OnLoad() {
        base.OnLoad();

        _interface = GRGlInterface.Create();
        _context = GRContext.CreateGl(_interface);
        CreateSurface(Size.X, Size.Y);

        App.Load();
        App.Resize(Size.X, Size.Y);
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
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e) {
        float clampWidth = Math.Max(e.Width, 1);
        float clampHeight = Math.Max(e.Height, 1);

        CreateSurface((int)clampWidth, (int)clampHeight);
        App.Resize((int)clampWidth, (int)clampHeight);
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

public class TestCanvas : SkiaCanvas {
    protected TestCanvas(SKCanvas canvas, SKSurface surface) : base(canvas) {
        Surface = surface;
    }

    public SKSurface Surface { get; }

    public static TestCanvas FromSize(int w, int h) {
        SKSurface surface = SKSurface.Create(new SKImageInfo(w, h));
        return new TestCanvas(surface.Canvas, surface);
    }

    public void ToImage(string path) {
        Surface.Flush();
        using SKImage image = Surface.Snapshot();
        using SKData data = image.Encode();
        using FileStream fs = new(path, FileMode.Create);
        data.SaveTo(fs);
    }
}

public static class Program {
    public static void Main(string[] args) {
        HashSet<int> set = new();
        set.Add(1);
        set.Add(1);
        new AppWindow().Run();
    }
}

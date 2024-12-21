using Natsu.Graphics;
using Natsu.Platforms.Desktop;

namespace NatsuApp;

public static class Program {
    public static void Main() {
        Application app = new MyApp();

        DesktopWindowSettings settings = new() {
            Title = "NatsuApp"
        };

        DesktopWindow window = new(app, settings);
        window.Run();
    }
}

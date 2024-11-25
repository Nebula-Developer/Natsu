using Natsu.Platforms.Desktop;

namespace Natsu.Sandbox;

public static class Program {
    public static void Main(string[] args) => new DesktopWindow(new MyApp()).Run();
}
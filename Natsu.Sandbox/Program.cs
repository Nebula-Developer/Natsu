
using System;
using System.Diagnostics;
using System.Numerics;

using Natsu.Graphics;
using Natsu.Platforms.Skia;

using SkiaSharp;

namespace Natsu.Sandbox;

public static unsafe class Program {
    public static void Main(string[] args) {
        SkiaApplication app = new();
        app.Run();
    }
}
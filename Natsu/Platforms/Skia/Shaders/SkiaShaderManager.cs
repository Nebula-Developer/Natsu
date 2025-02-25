using Natsu.Graphics.Shaders;
using SkiaSharp;

namespace Natsu.Platforms.Skia.Shaders;

public class SkiaShaderManager : IShaderManager {
    public IShaderBuilder CreateBuilder() => new SkiaShaderBuilder();

    public IShader Parse(string source) {
        SKRuntimeEffect effect = SKRuntimeEffect.CreateShader(source, out string err);
        if (err != null) throw new("Failed to compile skia shader: " + err);

        return new SkiaShader(effect);
    }

    public IShader Compile(IShaderBuilder builder) => Parse(builder.Builder.ToString());
}

using System.Text;
using Natsu.Graphics.Shaders;

namespace Natsu.Platforms.Skia.Shaders;

public class SkiaShaderBuilder : IShaderBuilder {
    public StringBuilder Builder { get; set; } = new();

    public IShaderFunction Function(string name, ShaderType returnType, params ShaderArg[] args) {
        Builder.Append($"{returnType.ToString().ToLower()} {name}(");
        for (int i = 0; i < args.Length; i++) {
            ShaderArg arg = args[i];
            Builder.Append($"{arg.Type.ToString().ToLower()} {arg.Name}");
            if (i < args.Length - 1) Builder.Append(", ");
        }

        Builder.Append(") {\n");
        return new SkiaShaderFunction(this);
    }

    public string ToShader() => Builder.ToString();

    public void Append(string value) => Builder.Append(value);
}

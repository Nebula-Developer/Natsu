using Natsu.Graphics.Shaders;

namespace Natsu.Platforms.Skia.Shaders;

public class SkiaShaderFunction : IShaderFunction {
    public SkiaShaderFunction(IShaderBuilder builder) => Builder = builder;

    public int BracketCloseCount { get; set; }
    public IShaderBuilder Builder { get; }

    public IShaderFunction Declare(string target, string value) {
        Builder.Append($"{target} = {value};\n");
        return this;
    }

    public IShaderFunction Return(string value) {
        Builder.Append($"return {value};\n");
        return this;
    }

    public IShaderBuilder Out() {
        Builder.Append(new string('}', BracketCloseCount + 1) + "\n");
        return Builder;
    }

    public IShaderFunction If(string condition) {
        Builder.Append($"if ({condition}) {{\n");
        BracketCloseCount++;
        return this;
    }

    public IShaderFunction ElseIf(string condition) {
        Builder.Append($"}} else if ({condition}) {{\n");
        return this;
    }

    public IShaderFunction Else() {
        Builder.Append("} else {\n");
        return this;
    }

    public IShaderFunction Up() {
        if (BracketCloseCount == 0) return this;
        Builder.Append(new string('}', BracketCloseCount) + "\n");
        BracketCloseCount--;
        return this;
    }
}

using Natsu.Graphics.Shaders;

namespace Natsu.Platforms.Empty.Shaders;

public class EmptyShaderFunction(IShaderBuilder builder) : IShaderFunction {
    public IShaderBuilder Builder { get; } = builder;
    public IShaderFunction Declare(string target, string value) => this;
    public IShaderFunction Return(string value) => this;
    public IShaderFunction If(string condition) => this;
    public IShaderFunction ElseIf(string condition) => this;
    public IShaderFunction Else() => this;
    public IShaderFunction Up() => this;
}

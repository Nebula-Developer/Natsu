using Natsu.Graphics.Shaders;

namespace Natsu.Platforms.Empty.Shaders;

public class EmptyShaderManager : IShaderManager {
    public IShaderBuilder CreateBuilder() => new EmptyShaderBuilder();
    public IShader Parse(string source) => new EmptyShader();
    public IShader Compile(IShaderBuilder builder) => new EmptyShader();
}

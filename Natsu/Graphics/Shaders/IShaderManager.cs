namespace Natsu.Graphics.Shaders;

public interface IShaderManager {
    public IShaderBuilder CreateBuilder();
    public IShader Parse(string source);
    public IShader Compile(IShaderBuilder builder);
}

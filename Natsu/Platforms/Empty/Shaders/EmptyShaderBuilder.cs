using System.Text;
using Natsu.Graphics.Shaders;

namespace Natsu.Platforms.Empty.Shaders;

public class EmptyShaderBuilder : IShaderBuilder {
    public StringBuilder Builder { get; set; } = new();
    public IShaderFunction Function(string name, ShaderType returnType, params ShaderArg[] args) => new EmptyShaderFunction(this);

    public string ToShader() => string.Empty;
}

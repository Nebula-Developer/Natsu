using System.Text;

namespace Natsu.Graphics.Shaders;

public enum ShaderType {
    Void,
    Float,
    Vec2,
    Vec3,
    Vec4,
    Mat2,
    Mat3,
    Mat4,
    Color
}

public enum ShaderArgType {
    In,
    Out,
    InOut
}

public enum ShaderOp {
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Power
}

public class ShaderArg {
    public ShaderArg(string name, ShaderType type, ShaderArgType argType) {
        Name = name;
        Type = type;
        ArgType = argType;
    }

    public string Name { get; }
    public ShaderType Type { get; }
    public ShaderArgType ArgType { get; }
}

public interface IShaderComponent { }

public interface IShaderFunction : IShaderComponent {
    public IShaderBuilder Builder { get; }
    public IShaderFunction Declare(string target, string value);
    public IShaderFunction Return(string value);
    public IShaderFunction If(string condition);
    public IShaderFunction ElseIf(string condition);
    public IShaderFunction Else();
    public IShaderFunction Up();
    public IShaderBuilder Out() => Builder;
}

public interface IShaderBuilder {
    public StringBuilder Builder { get; set; }
    public IShaderFunction Function(string name, ShaderType returnType, params ShaderArg[] args);
    public string ToShader();
    public void Append(string value) => Builder.Append(value);
}

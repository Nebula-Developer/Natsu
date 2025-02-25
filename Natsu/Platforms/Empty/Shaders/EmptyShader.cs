using Natsu.Graphics.Shaders;
using Natsu.Mathematics.Transforms;

namespace Natsu.Platforms.Empty.Shaders;

public class EmptyShader : TransformSequenceManager, IShader {
    public void SetUniform<T>(string name, T value) where T : notnull { }
    public T GetUniform<T>(string name) where T : notnull => default!;
}

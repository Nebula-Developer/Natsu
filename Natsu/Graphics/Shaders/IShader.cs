using Natsu.Mathematics.Transforms;

namespace Natsu.Graphics.Shaders;

public interface IShader : ITransformable {
    // Invalidation can be handled manually for the likes of skia shaders
    // to ensure that the shader isn't rebuilt on every uniform change.
    public void SetUniform<T>(string name, T value) where T : notnull;
    public T GetUniform<T>(string name) where T : notnull;
}

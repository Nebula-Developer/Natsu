using Natsu.Mathematics;

namespace Natsu.Graphics;

public interface IImage : IDisposable {
    public Vector2i Size { get; }
}

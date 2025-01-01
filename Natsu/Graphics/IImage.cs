using Natsu.Mathematics;

namespace Natsu.Graphics;

public interface IImage : IDisposable {
    Vector2i Size { get; }
}

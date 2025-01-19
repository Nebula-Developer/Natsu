using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Platforms.Empty;

public record EmptyImage : IImage {
    public void Dispose() { }

    public Vector2i Size => Vector2.Zero;
}

using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Platforms.Empty;

public record EmptyFont : IFont {
    public Vector2 MeasureText(string text, float size) => Vector2.Zero;
}

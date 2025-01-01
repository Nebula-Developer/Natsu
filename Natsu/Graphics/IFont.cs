using Natsu.Mathematics;

namespace Natsu.Graphics;

public interface IFont {
    Vector2 MeasureText(string text, float size);
}

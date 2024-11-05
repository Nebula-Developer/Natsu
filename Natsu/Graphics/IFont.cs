using Natsu.Mathematics;

namespace Natsu.Graphics;

public interface IFont {
    public Vector2 MeasureText(string text, float size);
}
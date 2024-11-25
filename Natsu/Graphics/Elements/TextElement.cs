using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class TextElement : PaintableElement {
    private string _text = string.Empty;

    public TextElement(string text, IFont font) {
        Font = font;
        Text = text;
        OnPaintChanged();
    }

    public string Text {
        get => _text;
        set {
            _text = value;
            Size = Font.MeasureText(value, Paint.TextSize);
        }
    }

    public IFont Font { get; set; }

    public override void OnPaintChanged() {
        base.OnPaintChanged();
        Paint.OnChanged += () => Size = Font.MeasureText(Text, Paint.TextSize);
        Size = Font.MeasureText(Text, Paint.TextSize);
    }

    public override void OnRender(ICanvas canvas) => canvas.DrawText(Text, new Vector2(0, 0), Font, Paint);
}
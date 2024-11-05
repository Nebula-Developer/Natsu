
namespace Natsu.Graphics;

public class TextElement : PaintableElement {
    public string Text {
        get => _text;
        set {
            _text = value;
            Size = Font.MeasureText(value, Paint.TextSize);
        }
    }
    private string _text = string.Empty;
    public IFont Font { get; set; }

    public TextElement(string text, IFont font) {
        Font = font;
        Text = text;
        OnPaintChanged();
    }

    public override void OnPaintChanged() {
        base.OnPaintChanged();
        Paint.OnChanged += () => Size = Font.MeasureText(Text, Paint.TextSize);
    }

    public override void OnRender(ICanvas canvas) =>
        canvas.DrawText(Text, new(0, 0), Font, Paint);
}
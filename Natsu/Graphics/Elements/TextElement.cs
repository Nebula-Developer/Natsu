using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class TextElement : PaintableElement {
    private string _text = string.Empty;
    private bool _sizeValid;

    public string Text {
        get => _text;
        set {
            _text = value;
            _sizeValid = false;
        }
    }

    public TextElement() { }

    public TextElement(string text) {
        Text = text;
    }

    public TextElement(string text, IFont font) {
        Text = text;
        Font = font;
    }

    private IFont? _font;
    public IFont? Font {
        get {
            if (_font == null && App != null)
                _font = ResourceLoader.DefaultFont;
            return _font;
        }
        set {
            _font = value;
            _sizeValid = false;
        }
    }
    
    public void CalculateSize() {
        if (Font == null) return;
        Size = Font.MeasureText(Text, Paint.TextSize);
    }

    private void AssignPaintEvent() {
        Paint.OnChanged += () => _sizeValid = false;
        _sizeValid = false;
    }

    public override void OnPaintChanged() => AssignPaintEvent();
    public override void OnLoad() => AssignPaintEvent();

    public override void OnRender(ICanvas canvas) {
        if (Font == null) return;
        if (!_sizeValid) CalculateSize();
        canvas.DrawText(Text, new Vector2(0, 0), Font, Paint);
    }
}
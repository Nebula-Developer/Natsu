using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class TextElement : PaintableElement {
    private bool _autoSize = true;

    private IFont? _font;
    private bool _sizeValid;
    private string _text = string.Empty;
    private Vector2 _textSize;

    public TextElement() { }

    public TextElement(string text) {
        Text = text;
    }

    public TextElement(string text, IFont font) {
        Text = text;
        Font = font;
    }

    public string Text {
        get => _text;
        set {
            _text = value;
            InvalidateAutoSize();
        }
    }
    public IFont? Font {
        get {
            if (_font == null && App != null)
                _font = ResourceLoader.DefaultFont;
            return _font;
        }
        set {
            _font = value;
            InvalidateAutoSize();
        }
    }

    public override Vector2 Size {
        get {
            if (!AutoSize) return base.Size;

            if (!_sizeValid) CalculateSize();
            return _textSize;
        }
        set => base.Size = value;
    }

    public bool AutoSize {
        get => _autoSize;
        set {
            _autoSize = value;
            Invalidate(Invalidation.DrawSize);
        }
    }

    private void InvalidateAutoSize() {
        Invalidate(Invalidation.Geometry);
        _sizeValid = false;
    }

    public void CalculateSize() {
        if (Font == null) return;

        _textSize = Font.MeasureText(Text, Paint.TextSize);
    }

    private void AssignPaintEvent() {
        Paint.DoChange += () => InvalidateAutoSize();
        InvalidateAutoSize();
    }

    protected override void OnPaintChanged() => AssignPaintEvent();
    protected override void OnLoad() => AssignPaintEvent();

    protected override void OnRender(ICanvas canvas) {
        if (Font == null) return;

        canvas.DrawText(Text, new Vector2(0, 0), Font, Paint);
    }
}

using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

public class TextElement : PaintableElement {
    private bool _autoSize = true;

    private IFont? _font;
    private string _text = string.Empty;
    private Vector2 _textSize;

    public TextElement() { }

    public TextElement(string text) => Text = text;

    public TextElement(string text, IFont font) {
        Text = text;
        Font = font;
    }

    public string Text {
        get => _text;
        set {
            _text = value;
            Invalidate(Invalidation.DrawSize | Invalidation.Layout);
        }
    }

    public IFont? Font {
        get {
            if (_font == null && App != null) _font = ResourceLoader.DefaultFont;

            return _font;
        }
        set {
            _font = value;
            Invalidate(Invalidation.DrawSize | Invalidation.Layout);
        }
    }

    public override Vector2 Size {
        get {
            if (!AutoSize) return base.Size;

            if (Invalidated.HasFlag(Invalidation.Layout)) CalculateSize();

            return _textSize;
        }
        set => base.Size = value;
    }

    public bool AutoSize {
        get => _autoSize;
        set {
            _autoSize = value;
            Invalidate(Invalidation.DrawSize | Invalidation.Layout);
        }
    }

    public void CalculateSize() {
        if (Font == null) return;

        _textSize = Font.MeasureText(Text, Paint.TextSize);

        Validate(Invalidation.Layout);
        Invalidate(Invalidation.DrawSize);

        HandleParentSizeChange();
    }

    protected override void OnPaintValueChange() => Invalidate(Invalidation.DrawSize | Invalidation.Layout);

    protected override void OnLoad() => Invalidate(Invalidation.DrawSize | Invalidation.Layout);

    protected override void OnRender(ICanvas canvas) {
        if (Font == null) return;

        canvas.DrawText(Text, new(0, 0), Font, Paint);
    }
}

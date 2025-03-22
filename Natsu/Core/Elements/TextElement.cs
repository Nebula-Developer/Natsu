using Natsu.Core.InvalidationTemp;
using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A text element that will draw text to the screen.
/// </summary>
public class TextElement : Element {
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

    /// <summary>
    ///     The text to draw.
    /// </summary>
    public string Text {
        get => _text;
        set {
            _text = value;
            Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);
        }
    }

    /// <summary>
    ///     The font to use when drawing the text.
    ///     <br />
    ///     If null, the <see cref="Native.ResourceLoader.DefaultFont" /> will be used.
    /// </summary>
    public IFont? Font {
        get {
            if (_font == null && App != null) _font = ResourceLoader.DefaultFont;

            return _font;
        }
        set {
            _font = value;
            Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);
        }
    }

    public override Vector2 Size {
        get {
            if (!AutoSize) return base.Size;

            if (Invalidated.HasFlag(ElementInvalidation.Layout)) CalculateSize();

            return _textSize;
        }
        set => base.Size = value;
    }

    /// <summary>
    ///     Whether to set this element's <see cref="Size" /> to the size of the text.
    /// </summary>
    public bool AutoSize {
        get => _autoSize;
        set {
            _autoSize = value;
            Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);
        }
    }

    /// <summary>
    ///     Calculate the size of the text.
    /// </summary>
    public void CalculateSize() {
        if (Font == null) return;

        _textSize = Font.MeasureText(Text, Paint.TextSize);

        Validate(ElementInvalidation.Layout);
        Invalidate(ElementInvalidation.DrawSize);

        HandleParentSizeChange();
    }

    protected override void OnPaintValueChange() => Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);

    protected override void OnLoad() => Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);

    protected override void OnRender(ICanvas canvas) {
        if (Font == null) return;

        canvas.DrawText(Text, new(0, 0), Font, Paint);
    }
}

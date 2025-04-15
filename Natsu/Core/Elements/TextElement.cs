using Natsu.Core.Invalidation;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Core.Elements;

/// <summary>
///     A text element that will draw text to the screen.
/// </summary>
public class TextElement : Element {
    private bool _autoSize = true;

    private IFont? _font;

    public TextElement() => TextBindable = string.Empty.Bindable(_ => Invalidate(ElementInvalidation.Layout));

    public TextElement(string text) : this() => Text = text;
    public TextElement(IBindable<string> textBindable) : this() => TextBindable.BindTo(textBindable);

    public TextElement(string text, IFont font) : this() {
        Text = text;
        Font = font;
    }

    /// <summary>
    ///     The bindable of the text.
    ///     <br />
    ///     Useful for binding to updating/shared values.
    /// </summary>
    public IBindable<string> TextBindable { get; }

    /// <summary>
    ///     The text to draw.
    /// </summary>
    public string Text {
        get => TextBindable.Value;
        set => TextBindable.Value = value;
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
            Invalidate(ElementInvalidation.Layout);
        }
    }

    /// <summary>
    ///     Whether to set this element's <see cref="Element.Size" /> to the size of the text.
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
    public Vector2 CalculateSize() {
        if (Font == null) return Vector2.Zero;
        return Font.MeasureText(Text, Paint.TextSize);
    }

    protected override void OnPaintValueChange() => Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);

    protected override void OnLoad() => Invalidate(ElementInvalidation.DrawSize | ElementInvalidation.Layout);

    protected override void OnUpdate(double deltaTime) {
        base.OnUpdate(deltaTime);

        if (AutoSize && Invalidated.HasFlag(ElementInvalidation.Layout)) {
            Size = CalculateSize();
            Invalidate(ElementInvalidation.Layout);
        }
    }

    protected override void OnRender(ICanvas canvas) {
        if (Font == null) return;

        canvas.DrawText(Text, new(0, 0), Font, Paint);
    }
}

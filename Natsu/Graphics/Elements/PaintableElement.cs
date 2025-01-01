namespace Natsu.Graphics.Elements;

public class PaintableElement : Element, IPaint {
    public PaintableElement() =>
        Paint.DoChange += () => {
            DoPaintValueChange?.Invoke();
            OnPaintValueChange();
        };

    public Paint Paint { get; } = new();

    public Color Color {
        get => Paint.Color;
        set => Paint.Color = value;
    }

    public float StrokeWidth {
        get => Paint.StrokeWidth;
        set => Paint.StrokeWidth = value;
    }

    public bool IsStroke {
        get => Paint.IsStroke;
        set => Paint.IsStroke = value;
    }

    public bool IsAntialias {
        get => Paint.IsAntialias;
        set => Paint.IsAntialias = value;
    }

    public FilterQuality FilterQuality {
        get => Paint.FilterQuality;
        set => Paint.FilterQuality = value;
    }

    public float TextSize {
        get => Paint.TextSize;
        set => Paint.TextSize = value;
    }

    public StrokeCap StrokeCap {
        get => Paint.StrokeCap;
        set => Paint.StrokeCap = value;
    }

    public StrokeJoin StrokeJoin {
        get => Paint.StrokeJoin;
        set => Paint.StrokeJoin = value;
    }

    public event Action? DoPaintValueChange;

    protected virtual void OnPaintValueChange() { }
}

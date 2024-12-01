namespace Natsu.Graphics.Elements;

public class PaintableElement : Element {
    private Paint _paint = new();

    public Paint Paint {
        get => _paint;
        set {
            _paint = value;
            OnPaintChanged();
        }
    }

    public Color Color {
        get => Paint.Color;
        set => Paint.Color = value;
    }

    public virtual void OnPaintChanged() { }
}

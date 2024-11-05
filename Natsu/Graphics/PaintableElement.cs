
namespace Natsu.Graphics;

public class PaintableElement : Element {
    public Paint Paint {
        get => _paint;
        set {
            _paint = value;
            OnPaintChanged();
        }
    }
    private Paint _paint = new();

    public virtual void OnPaintChanged() { }
}

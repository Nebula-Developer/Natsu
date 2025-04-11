using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Utils;

public class SliderElement : InputElement {
    private float _min, _max = 1f, _step;

    public Element Grabber = null!;

    public SliderElement() {
        Add(Grabber = MakeGrabber().With(g => { }));

        ValueBindable = new(0f, UpdateValue);
    }

    public Bindable<float> ValueBindable { get; }

    public float Value {
        get => ValueBindable.Value;
        set {
            if (ValueBindable.Settable) return;
            ValueBindable.Value = value;
        }
    }

    public float Min {
        get => _min;
        set {
            _min = value;
            UpdateValue(Value);
        }
    }

    public float Max {
        get => _max;
        set {
            _max = value;
            UpdateValue(Value);
        }
    }

    public float Step {
        get => _step;
        set {
            _step = value;
            UpdateValue(Value);
        }
    }

    public virtual void OnValueChanged(float value) { }
    public event Action<float>? DoValueChanged;

    public void UpdateValue(float value) {
        if (value < _min) value = _min;
        if (value > _max) value = _max;

        if (_step > 0) {
            float stepCount = MathF.Round((value - _min) / _step);
            value = _min + stepCount * _step;
        }

        if (value == Value) return;

        Value = value;
        float percent = (Value - _min) / (_max - _min);
        percent = Math.Clamp(percent, 0f, 1f);

        Grabber.StopTransformSequence(nameof(Grabber.Pivot));
        Grabber.PivotTo(new(percent, 0.5f), 0.2f, Easing.ExpoOut);

        OnValueChanged(Value);
        DoValueChanged?.Invoke(Value);
    }

    protected Element MakeGrabber() {
        BoxElement grabber = new() {
            Color = Colors.White,
            RoundedCorners = 5,
            IsAntialias = true
        };

        DoDrawSizeChange += size => { grabber.Size = size.Y; };

        return grabber;
    }

    protected override bool OnMouseDown(MouseButton button, Vector2 position) => button == MouseButton.Left;

    protected override void OnPressDown(int index, Vector2 position) {
        if (index != 0) return;
        UpdateValue(position.X / DrawSize.X * (_max - _min) + _min);
    }

    protected override void OnPressMove(int index, Vector2 position) => UpdateValue(position.X / DrawSize.X * (_max - _min) + _min);

    protected override void OnPressUp(int index, Vector2 position) {
        if (index != 0) return;
        UpdateValue(position.X / DrawSize.X * (_max - _min) + _min);
    }
}

public class MyApp : Application {
    private Vector2 _targetScale = Vector2.One;

    protected override void OnLoad() {
        SliderElement slider = new() {
            RelativeSizeAxes = Axes.X,
            Margin = new(5),
            Size = new(1, 60f),
            Parent = this,

            Step = 0.0001f
        };

        TextElement test = new(slider.ValueBindable.Cast<float, string>()) {
            Size = new(100, 50),
            Color = Colors.White,
            Font = ResourceLoader.DefaultFont,
            Parent = this,
            ImageFilter = Renderer.CreateBlur(3, 3),
            TextSize = 40
        };

        BlurredBackgroundElement elm = new() {
            Size = 100,
            Index = 999,
            Parent = this,
            Children = [
                new BoxElement {
                    RelativeSizeAxes = Axes.Both,
                    Color = Colors.White
                }
            ]
        };
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        if (key == Key.Up)
            _targetScale *= 2f;
        else if (key == Key.Down) _targetScale /= 2f;

        Root.StopTransformSequence(nameof(Root.Scale));
        Root.ScaleTo(_targetScale, 0.5f, Easing.ExpoOut);
    }
}

public class BlurredBackgroundElement : Element {
    protected override void OnRenderChildren(ICanvas canvas) {
        int restore = canvas.CreateBackdropFilter(Renderer.CreateBlur(10, 10));
        base.OnRenderChildren(canvas);
        canvas.Restore(restore);
    }
}

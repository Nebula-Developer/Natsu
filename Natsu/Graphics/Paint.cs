namespace Natsu.Graphics;

public class Paint {
    private Color _color = Colors.White;
    private FilterQuality _filterQuality = FilterQuality.None;
    private float _fontSize = 12;
    private bool _isAntialias;
    private bool _isStroke;
    private float _strokeWidth = 1;

    public Color Color {
        get => _color;
        set {
            _color = value;
            DoChange?.Invoke();
        }
    }

    public float StrokeWidth {
        get => _strokeWidth;
        set {
            _strokeWidth = value;
            DoChange?.Invoke();
        }
    }

    public bool IsStroke {
        get => _isStroke;
        set {
            _isStroke = value;
            DoChange?.Invoke();
        }
    }

    public bool IsAntialias {
        get => _isAntialias;
        set {
            _isAntialias = value;
            DoChange?.Invoke();
        }
    }

    public FilterQuality FilterQuality {
        get => _filterQuality;
        set {
            _filterQuality = value;
            DoChange?.Invoke();
        }
    }

    public float TextSize {
        get => _fontSize;
        set {
            _fontSize = value;
            DoChange?.Invoke();
        }
    }

    public StrokeCap StrokeCap { get; set; } = StrokeCap.Butt;

    public StrokeJoin StrokeJoin { get; set; } = StrokeJoin.Miter;

    public event Action? DoChange;
}

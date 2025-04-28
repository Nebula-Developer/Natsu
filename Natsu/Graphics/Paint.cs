using Natsu.Graphics.Shaders;

namespace Natsu.Graphics;

public class Paint : IPaint, IEquatable<Paint> {
    private BlendMode _blendMode = BlendMode.SrcOver;
    private Color _color = Colors.White;
    private FilterQuality _filterQuality = FilterQuality.None;
    private float _fontSize = 12;
    private IImageFilter? _imageFilter;
    private bool _isAntialias;
    private bool _isStroke;

    private IShader? _shader;

    private StrokeCap _strokeCap = StrokeCap.Butt;
    private StrokeJoin _strokeJoin = StrokeJoin.Miter;
    private float _strokeWidth = 1;

    public Paint() => Color.DoChange += () => DoChange?.Invoke();

    public bool Equals(Paint? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _color.Equals(other._color) && _filterQuality == other._filterQuality && _fontSize.Equals(other._fontSize) && _isAntialias == other._isAntialias && _isStroke == other._isStroke && _strokeWidth.Equals(other._strokeWidth) && StrokeCap == other.StrokeCap && StrokeJoin == other.StrokeJoin;
    }

    public Color Color {
        get => _color;
        set {
            _color = value;
            DoChange?.Invoke();
            Color.DoChange += () => DoChange?.Invoke();
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

    public BlendMode BlendMode {
        get => _blendMode;
        set {
            _blendMode = value;
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

    public StrokeCap StrokeCap {
        get => _strokeCap;
        set {
            _strokeCap = value;
            DoChange?.Invoke();
        }
    }

    public StrokeJoin StrokeJoin {
        get => _strokeJoin;
        set {
            _strokeJoin = value;
            DoChange?.Invoke();
        }
    }

    public IShader? Shader {
        get => _shader;
        set {
            _shader = value;
            DoChange?.Invoke();
            DoShaderChange?.Invoke();
        }
    }

    public IImageFilter? ImageFilter {
        get => _imageFilter;
        set {
            _imageFilter = value;
            DoChange?.Invoke();
        }
    }

    public event Action? DoChange, DoShaderChange;
}

using SkiaSharp;

namespace Natsu.Graphics;

public class Paint {
    public event Action? OnChanged;

    public Color Color {
        get => _color;
        set {
            _color = value;
            OnChanged?.Invoke();
        }
    }
    private Color _color = Colors.White;

    public float StrokeWidth {
        get => _strokeWidth;
        set {
            _strokeWidth = value;
            OnChanged?.Invoke();
        }
    }
    private float _strokeWidth = 1;

    public bool IsStroke {
        get => _isStroke;
        set {
            _isStroke = value;
            OnChanged?.Invoke();
        }
    }
    private bool _isStroke = false;

    public bool IsAntialias {
        get => _isAntialias;
        set {
            _isAntialias = value;
            OnChanged?.Invoke();
        }
    }
    private bool _isAntialias = true;

    public FilterQuality FilterQuality {
        get => _filterQuality;
        set {
            _filterQuality = value;
            OnChanged?.Invoke();
        }
    }
    private FilterQuality _filterQuality = FilterQuality.High;

    public float TextSize {
        get => _fontSize;
        set {
            _fontSize = value;
            OnChanged?.Invoke();
        }
    }
    private float _fontSize = 12;

    public bool DisableTextYCorrection {
        get => _disableTextYCorrection;
        set {
            _disableTextYCorrection = value;
            OnChanged?.Invoke();
        }
    }
    private bool _disableTextYCorrection = false;
}
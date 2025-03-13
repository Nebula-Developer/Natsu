using Natsu.Graphics;
using Natsu.Graphics.Shaders;

namespace Natsu.Core;

public partial class Element : IPaint {
    private float _opacity = 1;
    private Paint? _paint;
    private float _worldOpacity = 1;

    public Paint Paint {
        get {
            if (_paint == null) {
                _paint = new ElementPaint(this);
                _paint.DoChange += () => {
                    DoPaintValueChange?.Invoke();
                    OnPaintValueChange();
                };

                Paint.DoShaderChange += () => {
                    if (UpdateShaderResolution) Shader?.SetUniform("resolution", DrawSize);
                    if (UpdateShaderPosition) Shader?.SetUniform("pos", WorldPosition);
                    if (UpdateShaderTime && App?.Time != null) Shader?.SetUniform("time", (float)App.Time.Time);
                };
            }

            return _paint;
        }
    }

    /// <summary>
    ///     Whether to update the resolution uniform of the shader.
    /// </summary>
    public bool UpdateShaderResolution { get; set; } = true;

    /// <summary>
    ///     Whether to update the position uniform of the shader.
    /// </summary>
    public bool UpdateShaderPosition { get; set; } = true;

    /// <summary>
    ///     Whether to update the time uniform of the shader.
    /// </summary>
    public bool UpdateShaderTime { get; set; } = true;

    public float WorldOpacity {
        get {
            if (Invalidated.HasFlag(Invalidation.Opacity)) {
                _worldOpacity = Parent?.WorldOpacity * Opacity ?? Opacity;
                Validate(Invalidation.Opacity);
            }

            return _worldOpacity;
        }
        set => Opacity = value / (Parent?.WorldOpacity ?? 1);
    }

    public Color Color {
        get => Paint.Color;
        set => Paint.Color = value;
    }

    public float Opacity {
        get => _opacity;
        set {
            _opacity = value;
            Invalidate(Invalidation.Opacity, InvalidationPropagation.Children);
            DoPaintValueChange?.Invoke();
        }
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

    public IShader? Shader {
        get => Paint != null ? Paint.Shader : null;
        set => Paint.Shader = value;
    }

    protected void UpdateShader() {
        if (UpdateShaderTime && Shader != null) {
            Shader.SetUniform("time", (float)App.Time.Time);
            Shader.UpdateTransformSequences(App.Time.DeltaTime);
        }
    }

    protected virtual void OnPaintValueChange() { }
    public event Action? DoPaintValueChange;
}

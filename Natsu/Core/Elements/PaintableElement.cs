using Natsu.Graphics;
using Natsu.Graphics.Shaders;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     An element that has inherits <see cref="Paint" /> properties.
/// </summary>
public class PaintableElement : Element, IPaint {
    public PaintableElement() =>
        Paint.DoChange += () => {
            DoPaintValueChange?.Invoke();
            OnPaintValueChange();
        };

    /// <summary>
    ///     The element's paint.
    /// </summary>
    public Paint Paint { get; } = new();

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

    public Color Color {
        get => Paint.Color;
        set => Paint.Color = value;
    }

    public float Opacity {
        get => Paint.Opacity;
        set => Paint.Opacity = value;
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
        get => Paint.Shader;
        set => Paint.Shader = value;
    }

    protected override void OnDrawSizeChange(Vector2 size) {
        base.OnDrawSizeChange(size);
        if (UpdateShaderResolution && Shader != null) Shader.SetUniform("resolution", size);
    }

    protected override void OnUpdate() {
        base.OnUpdate();
        if (UpdateShaderTime && Shader != null) {
            Shader.SetUniform("time", (float)App.Time.Time);
            Shader.UpdateTransformSequences(App.Time.DeltaTime);
        }
    }

    protected override void OnWorldPositionChange(Vector2 position) {
        base.OnWorldPositionChange(position);
        if (UpdateShaderPosition && Shader != null) Shader.SetUniform("pos", position);
    }

    public event Action? DoPaintValueChange;

    protected virtual void OnPaintValueChange() { }
}

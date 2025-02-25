using Natsu.Graphics;
using Natsu.Graphics.Shaders;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;
using Natsu.Utils;
using SkiaSharp;

namespace Natsu.Platforms.Skia.Shaders;

public class SkiaShader : IShader {
    private readonly DirtyValue<SKShader> _dirtyShader = new();
    private readonly SKRuntimeEffect _effect;
    private readonly SKRuntimeEffectUniforms _effectUniforms;

    private readonly List<ITransformSequence> _transformSequences = new();
    private readonly Dictionary<string, object> _uniforms = new();

    public SkiaShader(SKRuntimeEffect effect) {
        _effect = effect;
        _effectUniforms = new(_effect);
        _dirtyShader.Validate(_effect.ToShader(_effectUniforms));
    }

    public SkiaShader(string source) {
        _effect = SKRuntimeEffect.CreateShader(source, out string err);
        if (err != null) throw new("Failed to compile skia shader: " + err);

        _effectUniforms = new(_effect);
        _dirtyShader.Validate(_effect.ToShader(_effectUniforms));
    }

    public SKShader Shader {
        get {
            if (_dirtyShader.IsDirty) _dirtyShader.Validate(_effect.ToShader(_effectUniforms));

            return _dirtyShader.Value!;
        }
        set => _dirtyShader.Validate(value);
    }

    public void AddTransformSequence(ITransformSequence sequence) {
        lock (_transformSequences) {
            _transformSequences.Add(sequence);
        }
    }

    public void StopTransformSequences(params string[] names) {
        lock (_transformSequences) {
            _transformSequences.RemoveAll(s => names.Contains(s.Name));
        }
    }

    public void StopTransformSequences() {
        lock (_transformSequences) {
            _transformSequences.Clear();
        }
    }

    public void StopTransformSequence(ITransformSequence sequence) {
        lock (_transformSequences) {
            _transformSequences.Remove(sequence);
        }
    }

    public void UpdateTransformSequences(double time) {
        lock (_transformSequences) {
            foreach (ITransformSequence? sequence in _transformSequences) sequence.Update((float)time);
        }
    }

    public void SetUniform<T>(string name, T value) where T : notnull {
        _setUniform(name, value);
        _dirtyShader.Invalidate();
    }

    public T GetUniform<T>(string name) where T : notnull {
        if (_uniforms.TryGetValue(name, out object? value)) return (T)value;

        throw new ArgumentException("Uniform not found: " + name);
    }

    private void _setUniform(string name, object value) {
        if (value is float f) {
            _effectUniforms[name] = f;
            _uniforms[name] = f;
        } else if (value is float[] fs) {
            _effectUniforms[name] = fs;
            _uniforms[name] = fs;
        } else if (value is int i) {
            _effectUniforms[name] = i;
            _uniforms[name] = i;
        } else if (value is int[] ints) {
            _effectUniforms[name] = ints;
            _uniforms[name] = ints;
        } else if (value is Color c) {
            _effectUniforms[name] = (SKColor)c;
            _uniforms[name] = c;
        } else if (value is Vector2 v) {
            _effectUniforms[name] = (SKPoint)v;
            _uniforms[name] = v;
        } else {
            throw new ArgumentException("Invalid uniform type: " + value?.GetType() ?? "null");
        }
    }
}

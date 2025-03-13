using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Graphics.Shaders;
using Natsu.Input;
using Natsu.Mathematics;

public class MyApp : Application {
    private Vector2 _targetScale = Vector2.One;

    protected override void OnLoad() {
        GridElement grid = new() {
            RelativeSizeAxes = Axes.Both,
            Parent = Root,
            Columns = 10,
            OverflowDirection = GridOverflowDirection.Horizontal
        };

        string shader = @"
            uniform vec4 color;
            uniform vec2 resolution;
            uniform float time;
            uniform vec2 pos;

            vec4 main(vec2 coord) {
                vec2 uv = coord / resolution;

                return vec4(
                    sin(uv.x * 10 + time) * 0.5 + 0.5,
                    sin(uv.y * 10 + time) * 0.5 + 0.5,
                    sin(uv.x * 10 + uv.y * 10 + time) * 0.5 + 0.5,
                    1
                );
            }
        ";

        for (int i = 0; i < 100; i++) {
            IShader shaderParsed = ShaderManager.Parse(shader);
            shaderParsed.SetUniform("color", Color.FromHSV(i / 100f * 360, MathF.Min(1, i / 100f), 1));

            BoxElement box = new() {
                Size = new(100, 100),
                Shader = shaderParsed,
                UpdateShaderTime = true
            };
            grid.Add(box);

            box.After(i % 2).SetLoopPoint(1).ScaleTo(new(2, 2), 3f, Easing.ExpoInOut).Then().ScaleTo(new(1, 1), 3f, Easing.ExpoInOut).Loop(-1, 1);
        }

        TextElement fpsText = new() {
            Pivot = new(1),
            Position = -5,
            Parent = Root,
            TextSize = 40
        };

        InputElement fpsInput = new() {
            Parent = fpsText,
            RelativeSizeAxes = Axes.Both
        };

        bool fpsMode = false;
        fpsInput.DoPress += (_, _) => fpsMode = !fpsMode;

        fpsText.DoUpdate += () => { fpsText.Text = fpsMode ? $"{Math.Round(Time.Time * 10) / 10}s" : $"{(int)Time.TPS} FPS"; };
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        Vector2 orig = _targetScale;
        if (key == Key.Up)
            _targetScale *= 1.1f;
        else if (key == Key.Down) _targetScale /= 1.1f;

        Root.StopTransformSequence(nameof(Root.Scale));
        Root.ScaleTo(_targetScale, 0.5f, Easing.ExpoOut);
    }
}

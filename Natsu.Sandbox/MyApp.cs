using System.Diagnostics;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Graphics.Shaders;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class BouncyButton : InputElement {
    public BoxElement Background = new() {
        Color = new(30, 40, 50),
        RoundedCorners = new(10),
        Pivot = new(0.5f),
        RelativeSizeAxes = Axes.Both,
        IsAntialias = true
        // Clip = true
    };

    protected override void OnLoad() => Add(Background);

    protected override void OnPressDown(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(0.6f, 2f, Easing.ExpoOut);
    }

    protected override void OnPressUp(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(1f, 0.8f, Easing.ElasticOut);
    }

    protected override void OnPress(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Color));
        Background.ColorTo(Colors.White).Then().ColorTo(new(30, 40, 50), 0.5f, Easing.QuadOut);
    }
}

public class MyApp : Application {
    private readonly Stopwatch _stopwatch = new();

    private Vector2 _scale = 1;

    public BouncyButton Button = new() {
        Pivot = new(0.5f),
        RelativeSizeAxes = Axes.Both,
        Size = new(0.75f)
    };

    public TextElement fpsText = new() {
        TextSize = 60,
        Color = Colors.White,
        OffsetPosition = new(0.5f),
        AnchorPosition = new(0.5f)
    };

    public bool fpsTypeToggle;

    protected override void OnLoad() {
        Add(Button);
        Platform.VSync = true;
        Platform.UpdateFrequency = 60;

        string shader = @"
            uniform float time;
            uniform vec2 resolution;
            uniform vec2 pos;
            uniform vec2 blurAmount;

            vec4 main(vec2 pos) {
                vec2 uv = pos / resolution;
                vec2 center = vec2(0.5, 0.5);
                float dist = distance(uv, center);
                float blur = 0.1;
                float alpha = smoothstep(0.5 - blur, 0.5 + blur, dist);
                return vec4(1, 0, 0, alpha);
            }
        ";

        IShader? iShader = ShaderManager.Parse(shader);

        Button.Background.Shader = iShader;

        InputElement fpsInput = new() {
            ChildRelativeSizeAxes = Axes.Both,
            Position = new(10)
        };

        BoxElement bg = new() {
            Parent = fpsInput,
            Color = Colors.DarkGray,
            RoundedCorners = new(10),
            Index = -10,
            Size = new(200, 100)
        };

        fpsInput.DoPressDown += (_, _) => {
            fpsText.StopTransformSequences(nameof(fpsText.Scale));
            fpsText.ScaleTo(0.5f, 0.3f, Easing.ExpoOut);
        };

        fpsInput.DoPressUp += (_, _) => {
            fpsText.StopTransformSequences(nameof(fpsText.Scale), nameof(fpsText.Opacity));
            fpsText.ScaleTo(1, 0.3f, Easing.ExpoOut);

            fpsText.Opacity = 0;
            fpsText.OpacityTo(1f, 0.5f);

            fpsTypeToggle = !fpsTypeToggle;
        };

        Add(fpsInput);
    }

    protected override void OnUpdate() {
        fpsText.Text = fpsTypeToggle ? (MathF.Round((float)Time.Time * 100f) / 100f).ToString() : (MathF.Round((float)Time.TPS * 100f) / 100f).ToString();
        Button.Background.Shader!.SetUniform("time", (float)Time.Time);
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        if (key == Key.Up) {
            Root.StopTransformSequences(nameof(Root.Scale));
            Root.ScaleTo(_scale *= 2, 0.5f, Easing.ExpoOut);
        } else if (key == Key.Down) {
            Root.StopTransformSequences(nameof(Root.Scale));
            Root.ScaleTo(_scale /= 2, 0.5f, Easing.ExpoOut);
        }

        // move  button on a
        if (key == Key.A) {
            Button.StopTransformSequences(nameof(Button.Position));
            Button.MoveTo(Button.Position + new Vector2(-100, 0), 0.5f, Easing.ExpoOut);
        } else if (key == Key.D) {
            Button.StopTransformSequences(nameof(Button.Position));
            Button.MoveTo(Button.Position + new Vector2(100, 0), 0.5f, Easing.ExpoOut);
        } else if (key == Key.W) {
            Button.StopTransformSequences(nameof(Button.Position));
            Button.MoveTo(Button.Position + new Vector2(0, -100), 0.5f, Easing.ExpoOut);
        } else if (key == Key.S) {
            Button.StopTransformSequences(nameof(Button.Position));
            Button.MoveTo(Button.Position + new Vector2(0, 100), 0.5f, Easing.ExpoOut);
        }
    }
}

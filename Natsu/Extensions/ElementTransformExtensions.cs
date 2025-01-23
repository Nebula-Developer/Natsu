using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;
using Natsu.Utils.Logging;

namespace Natsu.Extensions;

public static class ElementTransformExtensions {
    public static TransformSequence<T> TransformTo<T>(this T element, string property, Action<double> setter, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Transform t = new(t => setter(t)) { Duration = (float)duration, Easing = Easings.FromEase(ease), Name = property };
        TransformSequence<T> seq = new TransformSequence<T>(element).Append(t);
        seq.Name = property;
        element.AddTransformSequence(seq);
        return seq;
    }

    public static TransformSequence<T> TransformTo<T>(this TransformSequence<T> sequence, string property, Action<double> setter, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Transform t = new(t => setter(t)) { Duration = (float)duration, Easing = Easings.FromEase(ease), Name = property };
        return sequence.Append(t);
    }

    public static TransformSequence<T> Begin<T>(this T element, string name) where T : Element {
        TransformSequence<T> seq = new(element) { Name = name };
        element.AddTransformSequence(seq);
        return seq;
    }

    public static TransformSequence<T> ScaleTo<T>(this T element, Vector2 scale, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentScale = element.Scale;
        TransformSequence<T> seq = element.TransformTo("Scale", t => element.Scale = currentScale.Lerp(scale, (float)t).Max(0), duration, ease);
        seq.FutureData["Scale"] = scale;
        return seq;
    }

    public static TransformSequence<T> ScaleTo<T>(this TransformSequence<T> sequence, Vector2 scale, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentScale = sequence.FutureData.ContainsKey("Scale") ? (Vector2)sequence.FutureData["Scale"] : sequence.Target.Scale;
        sequence.FutureData["Scale"] = scale;
        return sequence.TransformTo("Scale", t => sequence.Target.Scale = currentScale.Lerp(scale, (float)t).Max(0), duration, ease);
    }

    public static TransformSequence<T> MoveTo<T>(this T element, Vector2 position, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentPosition = element.Position;
        TransformSequence<T> seq = element.TransformTo("Position", t => element.Position = currentPosition.Lerp(position, (float)t), duration, ease);
        seq.FutureData["Position"] = position;
        return seq;
    }

    public static TransformSequence<T> MoveTo<T>(this TransformSequence<T> sequence, Vector2 position, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentPosition = sequence.FutureData.ContainsKey("Position") ? (Vector2)sequence.FutureData["Position"] : sequence.Target.Position;
        sequence.FutureData["Position"] = position;
        return sequence.TransformTo("Position", t => sequence.Target.Position = currentPosition.Lerp(position, (float)t), duration, ease);
    }

    public static TransformSequence<T> RotateTo<T>(this T element, float rotation, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        float currentRotation = element.Rotation;
        TransformSequence<T> seq = element.TransformTo("Rotation", t => element.Rotation = (float)Easings.Lerp(currentRotation, rotation, (float)t), duration, ease);
        seq.FutureData["Rotation"] = rotation;
        return seq;
    }

    public static TransformSequence<T> RotateTo<T>(this TransformSequence<T> sequence, float rotation, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        float currentRotation = sequence.FutureData.ContainsKey("Rotation") ? (float)sequence.FutureData["Rotation"] : sequence.Target.Rotation;
        sequence.FutureData["Rotation"] = rotation;
        return sequence.TransformTo("Rotation", t => sequence.Target.Rotation = (float)Easings.Lerp(currentRotation, rotation, (float)t), duration, ease);
    }

    public static TransformSequence<T> SizeTo<T>(this T element, Vector2 size, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentSize = element.Size;
        TransformSequence<T> seq = element.TransformTo("Size", t => element.Size = currentSize.Lerp(size, (float)t), duration, ease);
        seq.FutureData["Size"] = size;
        return seq;
    }

    public static TransformSequence<T> SizeTo<T>(this TransformSequence<T> sequence, Vector2 size, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentSize = sequence.FutureData.ContainsKey("Size") ? (Vector2)sequence.FutureData["Size"] : sequence.Target.Size;
        sequence.FutureData["Size"] = size;
        return sequence.TransformTo("Size", t => sequence.Target.Size = currentSize.Lerp(size, (float)t), duration, ease);
    }

    public static TransformSequence<T> MarginTo<T>(this T element, Vector2 margin, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentMargin = element.Margin;
        TransformSequence<T> seq = element.TransformTo("Margin", t => element.Margin = currentMargin.Lerp(margin, (float)t), duration, ease);
        seq.FutureData["Margin"] = margin;
        return seq;
    }

    public static TransformSequence<T> MarginTo<T>(this TransformSequence<T> sequence, Vector2 margin, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentMargin = sequence.FutureData.ContainsKey("Margin") ? (Vector2)sequence.FutureData["Margin"] : sequence.Target.Margin;
        sequence.FutureData["Margin"] = margin;
        return sequence.TransformTo("Margin", t => sequence.Target.Margin = currentMargin.Lerp(margin, (float)t), duration, ease);
    }

    public static TransformSequence<T> AnchorTo<T>(this T element, Vector2 anchor, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentAnchor = element.AnchorPosition;
        TransformSequence<T> seq = element.TransformTo("Anchor", t => element.AnchorPosition = currentAnchor.Lerp(anchor, (float)t), duration, ease);
        seq.FutureData["Anchor"] = anchor;
        return seq;
    }

    public static TransformSequence<T> AnchorTo<T>(this TransformSequence<T> sequence, Vector2 anchor, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentAnchor = sequence.FutureData.ContainsKey("Anchor") ? (Vector2)sequence.FutureData["Anchor"] : sequence.Target.AnchorPosition;
        sequence.FutureData["Anchor"] = anchor;
        return sequence.TransformTo("Anchor", t => sequence.Target.AnchorPosition = currentAnchor.Lerp(anchor, (float)t), duration, ease);
    }

    public static TransformSequence<T> OffsetTo<T>(this T element, Vector2 offset, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentOffset = element.OffsetPosition;
        TransformSequence<T> seq = element.TransformTo("Offset", t => element.OffsetPosition = currentOffset.Lerp(offset, (float)t), duration, ease);
        seq.FutureData["Offset"] = offset;
        return seq;
    }

    public static TransformSequence<T> OffsetTo<T>(this TransformSequence<T> sequence, Vector2 offset, double duration = 0, Ease ease = Ease.Linear) where T : Element {
        Vector2 currentOffset = sequence.FutureData.ContainsKey("Offset") ? (Vector2)sequence.FutureData["Offset"] : sequence.Target.OffsetPosition;
        sequence.FutureData["Offset"] = offset;
        return sequence.TransformTo("Offset", t => sequence.Target.OffsetPosition = currentOffset.Lerp(offset, (float)t), duration, ease);
    }

    public static TransformSequence<T> ColorTo<T>(this T element, Color color, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement {
        Color? currentColor = element.Paint.Color;
        TransformSequence<T> seq = element.TransformTo("Color", t => element.Paint.Color = Color.Lerp(currentColor, color, (float)t), duration, ease);
        seq.FutureData["Color"] = color;
        return seq;
    }

    public static TransformSequence<T> ColorTo<T>(this TransformSequence<T> sequence, Color color, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement {
        Color? currentColor = sequence.FutureData.ContainsKey("Color") ? (Color)sequence.FutureData["Color"] : sequence.Target.Paint.Color;
        sequence.FutureData["Color"] = color;
        return sequence.TransformTo("Color", t => sequence.Target.Paint.Color = Color.Lerp(currentColor, color, (float)t), duration, ease);
    }

    public static TransformSequence<T> OpacityTo<T>(this T element, float alpha, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement {
        float currentAlpha = element.Paint.Opacity;
        TransformSequence<T> seq = element.TransformTo("Alpha", t => element.Opacity = (float)Easings.Lerp(currentAlpha, alpha, t), duration, ease);
        seq.FutureData["Alpha"] = alpha;
        return seq;
    }

    public static TransformSequence<T> OpacityTo<T>(this TransformSequence<T> sequence, float alpha, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement {
        float currentAlpha = sequence.FutureData.ContainsKey("Alpha") ? (float)sequence.FutureData["Alpha"] : sequence.Target.Paint.Opacity;
        sequence.FutureData["Alpha"] = alpha;
        return sequence.TransformTo("Alpha", t => { sequence.Target.Opacity = (float)Easings.Lerp(currentAlpha, alpha, t); }, duration, ease);
    }

    public static TransformSequence<T> FadeOut<T>(this T element, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement => element.OpacityTo(0, duration, ease);

    public static TransformSequence<T> FadeOut<T>(this TransformSequence<T> sequence, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement => sequence.OpacityTo(0, duration, ease);

    public static TransformSequence<T> FadeIn<T>(this T element, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement => element.OpacityTo(1, duration, ease);

    public static TransformSequence<T> FadeIn<T>(this TransformSequence<T> sequence, double duration = 0, Ease ease = Ease.Linear) where T : PaintableElement => sequence.OpacityTo(1, duration, ease);
}

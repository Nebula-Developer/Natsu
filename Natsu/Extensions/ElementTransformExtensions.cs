using Natsu.Core;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;

namespace Natsu.Extensions;

public static class ElementTransformExtensions {
    public static TransformSequence<T> ScaleTo<T>(this T element, Vector2 scale, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Scale), scale, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> ScaleTo<T>(this TransformSequence<T> sequence, Vector2 scale, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Scale), scale, (float)duration, ease);

    public static TransformSequence<T> MoveTo<T>(this T element, Vector2 position, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Position), position, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> MoveTo<T>(this TransformSequence<T> sequence, Vector2 position, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Position), position, (float)duration, ease);

    public static TransformSequence<T> RotateTo<T>(this T element, float rotation, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Rotation), rotation, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> RotateTo<T>(this TransformSequence<T> sequence, float rotation, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Rotation), rotation, (float)duration, ease);

    public static TransformSequence<T> SizeTo<T>(this T element, Vector2 size, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Size), size, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> SizeTo<T>(this TransformSequence<T> sequence, Vector2 size, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Size), size, (float)duration, ease);

    public static TransformSequence<T> MarginTo<T>(this T element, Margin margin, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Margin), margin, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> MarginTo<T>(this TransformSequence<T> sequence, Margin margin, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Margin), margin, (float)duration, ease);

    public static TransformSequence<T> PaddingTo<T>(this T element, Margin padding, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Padding), padding, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> PaddingTo<T>(this TransformSequence<T> sequence, Margin padding, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Padding), padding, (float)duration, ease);

    public static TransformSequence<T> AnchorTo<T>(this T element, Vector2 anchor, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.AnchorPosition), anchor, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> AnchorTo<T>(this TransformSequence<T> sequence, Vector2 anchor, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.AnchorPosition), anchor, (float)duration, ease);

    public static TransformSequence<T> OffsetTo<T>(this T element, Vector2 offset, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.OffsetPosition), offset, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> OffsetTo<T>(this TransformSequence<T> sequence, Vector2 offset, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.OffsetPosition), offset, (float)duration, ease);

    public static TransformSequence<T> ColorTo<T>(this T element, Color color, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Paint.Color), color, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> ColorTo<T>(this TransformSequence<T> sequence, Color color, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Paint.Color), color, (float)duration, ease);

    public static TransformSequence<T> OpacityTo<T>(this T element, float alpha, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Opacity), alpha, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> OpacityTo<T>(this TransformSequence<T> sequence, float alpha, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Opacity), alpha, (float)duration, ease);

    public static TransformSequence<T> FadeOut<T>(this T element, double duration = 0, Easing ease = Easing.Linear) where T : Element => element.OpacityTo(0, duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> FadeOut<T>(this TransformSequence<T> sequence, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.OpacityTo(0, duration, ease);

    public static TransformSequence<T> FadeIn<T>(this T element, double duration = 0, Easing ease = Easing.Linear) where T : Element => element.OpacityTo(1, duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> FadeIn<T>(this TransformSequence<T> sequence, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.OpacityTo(1, duration, ease);

    public static TransformSequence<T> PivotTo<T>(this T element, Vector2 pivot, double duration = 0, Easing ease = Easing.Linear) where T : Element => new TransformSequence<T>(element).Create(nameof(element.Pivot), pivot, (float)duration, ease).AppendToTransformable(element);

    public static TransformSequence<T> PivotTo<T>(this TransformSequence<T> sequence, Vector2 pivot, double duration = 0, Easing ease = Easing.Linear) where T : Element => sequence.Create(nameof(sequence.Target.Pivot), pivot, (float)duration, ease);
}

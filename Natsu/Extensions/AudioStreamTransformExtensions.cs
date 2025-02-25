using Natsu.Audio;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;

namespace Natsu.Extensions;

public static class AudioStreamTransformExtensions {
    public static TransformSequence<T> VolumeTo<T>(this T stream, float volume, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => new TransformSequence<T>(stream).Create(nameof(stream.Volume), volume, (float)duration, ease).AppendToTransformable(stream);

    public static TransformSequence<T> VolumeTo<T>(this TransformSequence<T> sequence, float volume, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => sequence.Create(nameof(sequence.Target.Volume), volume, (float)duration, ease);

    public static TransformSequence<T> FrequencyTo<T>(this T stream, float frequency, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => new TransformSequence<T>(stream).Create(nameof(stream.Frequency), frequency, (float)duration, ease).AppendToTransformable(stream);

    public static TransformSequence<T> FrequencyTo<T>(this TransformSequence<T> sequence, float frequency, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => sequence.Create(nameof(sequence.Target.Frequency), frequency, (float)duration, ease);

    public static TransformSequence<T> PanTo<T>(this T stream, float pan, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => new TransformSequence<T>(stream).Create(nameof(stream.Pan), pan, (float)duration, ease).AppendToTransformable(stream);

    public static TransformSequence<T> PanTo<T>(this TransformSequence<T> sequence, float pan, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => sequence.Create(nameof(sequence.Target.Pan), pan, (float)duration, ease);

    public static TransformSequence<T> PositionTo<T>(this T stream, float position, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => new TransformSequence<T>(stream).Create(nameof(stream.Position), position, (float)duration, ease).AppendToTransformable(stream);

    public static TransformSequence<T> PositionTo<T>(this TransformSequence<T> sequence, float position, double duration = 0, Easing ease = Easing.Linear) where T : IAudioStream => sequence.Create(nameof(sequence.Target.Position), position, (float)duration, ease);
}

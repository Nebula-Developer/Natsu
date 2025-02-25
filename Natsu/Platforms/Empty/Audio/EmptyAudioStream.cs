using Natsu.Audio;
using Natsu.Mathematics.Transforms;

namespace Natsu.Platforms.Empty.Audio;

public class EmptyAudioStream : TransformSequenceManager, IAudioStream {
    public int Handle => 0;
    public float Volume { get; set; } = 0;
    public float Frequency { get; set; } = 0;
    public float Pan { get; set; } = 0;
    public float Position { get; set; } = 0;
    public float Length => 0;
    public bool Active => false;
    public bool Playing => false;

    public void Play() { }
    public void Pause() { }
    public void Stop() { }
}

using Natsu.Audio;

namespace Natsu.Platforms.Empty.Audio;

public class EmptyAudioManager : IAudioManager {
    public List<WeakReference<IAudioStream>> Streams { get; } = new();

    public IAudioStream CreateStream(byte[] data, bool play = false, bool loop = false) {
        IAudioStream stream = new EmptyAudioStream();
        lock (Streams) {
            Streams.Add(new(stream));
        }

        return stream;
    }

    public void Update(double time) {
        lock (Streams) {
            foreach (WeakReference<IAudioStream> reference in Streams)
                if (reference.TryGetTarget(out IAudioStream? stream))
                    stream.UpdateTransformSequences(time);
        }
    }

    public void Load() { }
}

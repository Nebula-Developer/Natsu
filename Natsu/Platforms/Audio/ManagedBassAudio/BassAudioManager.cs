using ManagedBass;
using Natsu.Audio;

namespace Natsu.Platforms.Audio.ManagedBassAudio;

public class BassAudioManager : IAudioManager {
    public List<WeakReference<IAudioStream>> Streams { get; } = new();

    public IAudioStream CreateStream(byte[] data, bool play = false, bool loop = false) {
        int handle = Bass.CreateStream(data, 0, data.Length, (loop ? BassFlags.Loop : BassFlags.Default) | BassFlags.Float | BassFlags.AutoFree);
        if (handle == 0) throw new("Failed to create audio stream: " + Bass.LastError);

        BassAudioStream bassStream = new(handle);

        bassStream.Play();
        if (!play) bassStream.Pause();

        Streams.Add(new(bassStream));

        return bassStream;
    }

    public void Update(double time) {
        lock (Streams) {
            for (int i = 0; i < Streams.Count; i++) {
                Streams[i].TryGetTarget(out IAudioStream? stream);

                if (stream == null || !stream.Active) {
                    Streams.RemoveAt(i);
                    i--;
                    continue;
                }

                stream.UpdateTransformSequences(time);
            }
        }
    }

    public void Load() => BassAudio.Init();
}

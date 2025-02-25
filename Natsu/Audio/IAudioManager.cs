namespace Natsu.Audio;

public interface IAudioManager {
    List<WeakReference<IAudioStream>> Streams { get; }
    IAudioStream CreateStream(byte[] data, bool play = false, bool loop = false);
    void Update(double time);
    void Load();
}

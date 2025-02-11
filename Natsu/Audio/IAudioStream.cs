namespace Natsu.Audio;

/// <summary>
///     Represents an audio stream, which can be used to play audio via an <see cref="IAudioMixer" />.
/// </summary>
public interface IAudioStream : IAudioProperties, IDisposable {
    bool Playing { get; }
    bool Paused { get; }
    bool Stopped { get; }

    bool Looping { get; set; }
    void Play();
    void Pause();
    void Stop();
}

namespace Natsu.Audio;

/// <summary>
///     An audio mixer, which will be used to route audio streams through a single output.
/// </summary>
public interface IAudioMixer : IAudioProperties {
    IAudioStream CreateStream(string path);
    IAudioStream CreateStream(Stream stream);

    void RemoveStream(IAudioStream stream);
    void AddStream(IAudioStream stream);
    void ClearStreams();
}

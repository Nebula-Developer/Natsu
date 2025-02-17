using Natsu.Mathematics.Transforms;

namespace Natsu.Audio;

/// <summary>
///     Represents a controllable audio stream.
/// </summary>
public interface IAudioStream : ITransformable {
    int Handle { get; }

    /// <summary>
    ///     The volume of the audio stream, from 0.0 to 1.0.
    /// </summary>
    float Volume { get; set; }

    /// <summary>
    ///     The frequency of the audio stream.
    /// </summary>
    float Frequency { get; set; }

    /// <summary>
    ///     The pan of the audio stream, from -1.0 (left) to 1.0 (right).
    /// </summary>
    float Pan { get; set; }

    /// <summary>
    ///     The position of the audio stream, in seconds.
    ///     <br />
    ///     This may not be allowed on all audio streams.
    /// </summary>
    float Position { get; set; }

    /// <summary>
    ///     The length of the audio stream, in seconds.
    ///     <br />
    ///     If the length is unknown, this property will return -1.
    /// </summary>
    float Length { get; }

    /// <summary>
    ///     The state of the audio stream handle.
    /// </summary>
    bool Active { get; }

    /// <summary>
    ///     Whether the audio stream is currently playing.
    /// </summary>
    bool Playing { get; }

    /// <summary>
    ///     Plays the audio stream.
    /// </summary>
    void Play();

    /// <summary>
    ///     Pauses the audio stream.
    /// </summary>
    void Pause();

    /// <summary>
    ///     Stops the audio stream.
    /// </summary>
    void Stop();
}

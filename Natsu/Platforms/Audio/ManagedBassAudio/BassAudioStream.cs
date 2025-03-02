using ManagedBass;
using Natsu.Audio;
using Natsu.Mathematics.Transforms;

namespace Natsu.Platforms.Audio.ManagedBassAudio;

public class BassAudioStream : TransformSequenceManager, IAudioStream {
    public BassAudioStream(int handle) => Handle = handle;

    public int Handle { get; }

    public float Volume {
        get => (float)Bass.ChannelGetAttribute(Handle, ChannelAttribute.Volume);
        set => Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, value);
    }

    public float Frequency {
        get => (float)Bass.ChannelGetAttribute(Handle, ChannelAttribute.Frequency);
        set => Bass.ChannelSetAttribute(Handle, ChannelAttribute.Frequency, value);
    }

    public float Pan {
        get => (float)Bass.ChannelGetAttribute(Handle, ChannelAttribute.Pan);
        set => Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pan, value);
    }

    public float Position {
        get => (float)Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetPosition(Handle));
        set => Bass.ChannelSetPosition(Handle, Bass.ChannelSeconds2Bytes(Handle, value));
    }

    public float Length => (float)Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetLength(Handle));

    public bool Playing => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

    public bool Active => Bass.ChannelIsActive(Handle) != PlaybackState.Stopped;

    public void Play() => Bass.ChannelPlay(Handle);

    public void Pause() => Bass.ChannelPause(Handle);

    public void Stop() => Bass.ChannelStop(Handle);

    public void Dispose() => Bass.StreamFree(Handle);
}

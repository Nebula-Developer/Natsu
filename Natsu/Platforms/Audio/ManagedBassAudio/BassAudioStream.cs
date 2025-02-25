using ManagedBass;
using Natsu.Audio;
using Natsu.Mathematics.Transforms;

namespace Natsu.Platforms.Audio.ManagedBassAudio;

public class BassAudioStream : IAudioStream {
    public BassAudioStream(int handle) => Handle = handle;

    public List<ITransformSequence> TransformSequences { get; } = new();

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

    public void AddTransformSequence(ITransformSequence sequence) => TransformSequences.Add(sequence);
    public void StopTransformSequences() => TransformSequences.Clear();

    public void StopTransformSequences(params string[] name) {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                ITransformSequence? sequence = TransformSequences[i];
                if (name.Contains(sequence.Name)) TransformSequences.RemoveAt(i--);
            }
        }
    }

    public void StopTransformSequence(ITransformSequence sequence) => TransformSequences.Remove(sequence);

    public void UpdateTransformSequences(double time) {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                ITransformSequence? sequence = TransformSequences[i];
                sequence.Update((float)time);
                if (sequence.IsCompleted) TransformSequences.RemoveAt(i--);
            }
        }
    }

    public void Dispose() => Bass.StreamFree(Handle);
}

using ManagedBass;

namespace Natsu.Platforms.Audio.ManagedBassAudio;

public static class BassAudio {
    private static readonly HashSet<int> _initializedDevices = new();

    public static void Init(int device = -1) {
        if (_initializedDevices.Contains(device)) return;

        if (!Bass.Init(device)) throw new("Failed to initialize BASS audio: " + Bass.LastError);

        Bass.UpdatePeriod = 5;
        Bass.DeviceBufferLength = 10;
        Bass.PlaybackBufferLength = 100;

        _initializedDevices.Add(device);
    }
}

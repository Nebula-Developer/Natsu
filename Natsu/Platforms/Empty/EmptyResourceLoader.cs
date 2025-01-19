using Natsu.Graphics;
using Natsu.Native;

namespace Natsu.Platforms.Empty;

public class EmptyResourceLoader : ResourceLoader {
    public override IFont DefaultFont => new EmptyFont();
    public override IImage LoadImage(byte[] data, string name) => new EmptyImage();

    public override IFont LoadFont(byte[] data, string name) => new EmptyFont();
}

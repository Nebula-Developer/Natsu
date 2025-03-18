namespace Natsu.Graphics;

public interface IImageFilterGenerator {
    public IImageFilter CreateBlur(float sigmaX, float sigmaY);
    public IImageFilter CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, Color color);
}

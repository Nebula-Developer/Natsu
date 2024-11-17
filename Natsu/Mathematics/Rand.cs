namespace Natsu.Mathematics;

public static class Rand {
    public static Random SysRand { get; } = new();

    public static int Next() => SysRand.Next();

    public static int Next(int min, int max) => SysRand.Next(min, max);

    public static double NextDouble() => SysRand.NextDouble();

    public static double NextDouble(double min, double max) => min + SysRand.NextDouble() * (max - min);

    public static void NextBytes(byte[] buffer) => SysRand.NextBytes(buffer);
}
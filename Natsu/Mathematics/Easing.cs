namespace Natsu.Mathematics;

public delegate double Easing(double t);

public enum Ease {
    Linear,
    QuadraticIn,
    QuadraticOut,
    QuadraticInOut,
    CubicIn,
    CubicOut,
    CubicInOut,
    QuarticIn,
    QuarticOut,
    QuarticInOut,
    QuinticIn,
    QuinticOut,
    QuinticInOut,
    SineIn,
    SineOut,
    SineInOut,
    CircularIn,
    CircularOut,
    CircularInOut,
    ExponentialIn,
    ExponentialOut,
    ExponentialInOut,
    ElasticIn,
    ElasticOut,
    ElasticInOut,
    BackIn,
    BackOut,
    BackInOut
}

public static class Easings {
    public static double Lerp(double a, double b, double t) => a + (b - a) * t;

    // Linear easing
    public static double Linear(double t) => t;

    // Quadratic easing
    public static double QuadraticIn(double t) => t * t;

    public static double QuadraticOut(double t) => t * (2 - t);

    public static double QuadraticInOut(double t) {
        if (t < 0.5) return 2 * t * t;

        return -1 + (4 - 2 * t) * t;
    }

    // Cubic easing
    public static double CubicIn(double t) => t * t * t;

    public static double CubicOut(double t) {
        double u = t - 1;
        return u * u * u + 1;
    }

    public static double CubicInOut(double t) {
        if (t < 0.5) return 4 * t * t * t;

        double u = 2 * t - 2;
        return 0.5 * u * u * u + 1;
    }

    // Quartic easing
    public static double QuarticIn(double t) => t * t * t * t;

    public static double QuarticOut(double t) {
        double u = t - 1;
        return 1 - u * u * u * u;
    }

    public static double QuarticInOut(double t) {
        if (t < 0.5) return 8 * t * t * t * t;

        double u = t - 1;
        return 1 - 8 * u * u * u * u;
    }

    // Quintic easing
    public static double QuinticIn(double t) => t * t * t * t * t;

    public static double QuinticOut(double t) {
        double u = t - 1;
        return u * u * u * u * u + 1;
    }

    public static double QuinticInOut(double t) {
        if (t < 0.5) return 16 * t * t * t * t * t;

        double u = 2 * t - 2;
        return 0.5 * u * u * u * u * u + 1;
    }

    // Sine easing
    public static double SineIn(double t) => 1 - Math.Cos(t * Math.PI / 2);

    public static double SineOut(double t) => Math.Sin(t * Math.PI / 2);

    public static double SineInOut(double t) => 0.5 * (1 - Math.Cos(Math.PI * t));

    // Circular easing
    public static double CircularIn(double t) => 1 - Math.Sqrt(1 - t * t);

    public static double CircularOut(double t) {
        double u = t - 1;
        return Math.Sqrt(1 - u * u);
    }

    public static double CircularInOut(double t) {
        if (t < 0.5) return 0.5 * (1 - Math.Sqrt(1 - 4 * t * t));

        double u = 2 * t - 2;
        return 0.5 * (Math.Sqrt(1 - u * u) + 1);
    }

    // Exponential easing
    public static double ExponentialIn(double t) => t == 0 ? 0 : Math.Pow(2, 10 * (t - 1));

    public static double ExponentialOut(double t) => t == 1 ? 1 : 1 - Math.Pow(2, -10 * t);

    public static double ExponentialInOut(double t) {
        if (t == 0 || t == 1) return t;

        if (t < 0.5) return 0.5 * Math.Pow(2, 20 * t - 10);

        return 0.5 * (2 - Math.Pow(2, -20 * t + 10));
    }

    // Elastic easing
    public static double ElasticIn(double t) {
        if (t == 0 || t == 1) return t;

        return -Math.Pow(2, 10 * (t - 1)) * Math.Sin((t - 1.1) * 5 * Math.PI);
    }

    public static double ElasticOut(double t) {
        if (t == 0 || t == 1) return t;

        return Math.Pow(2, -10 * t) * Math.Sin((t - 0.1) * 5 * Math.PI) + 1;
    }

    public static double ElasticInOut(double t) {
        if (t == 0 || t == 1) return t;

        if (t < 0.5) return -0.5 * Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125) * (2 * Math.PI / 4.5));

        return 0.5 * Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125) * (2 * Math.PI / 4.5)) + 1;
    }

    // Back easing
    public static double BackIn(double t) {
        const double s = 1.70158;
        return t * t * ((s + 1) * t - s);
    }

    public static double BackOut(double t) {
        const double s = 1.70158;
        double u = t - 1;
        return u * u * ((s + 1) * u + s) + 1;
    }

    public static double BackInOut(double t) {
        const double s = 1.70158 * 1.525;
        if (t < 0.5) return 0.5 * t * t * ((s + 1) * t - s);

        double u = t - 1;
        return 0.5 * (u * u * ((s + 1) * u + s) + 1);
    }

    // Bounce easing
    public static double BounceOut(double t) {
        if (t < 1 / 2.75) return 7.5625 * t * t;

        if (t < 2 / 2.75) return 7.5625 * (t -= 1.5 / 2.75) * t + 0.75;

        if (t < 2.5 / 2.75) return 7.5625 * (t -= 2.25 / 2.75) * t + 0.9375;

        return 7.5625 * (t -= 2.625 / 2.75) * t + 0.984375;
    }

    public static double BounceIn(double t) => 1 - BounceOut(1 - t);

    public static double BounceInOut(double t) {
        if (t < 0.5) return 0.5 * BounceIn(t * 2);

        return 0.5 * BounceOut(t * 2 - 1) + 0.5;
    }

    public static Easing FromEase(Ease ease) =>
        ease switch {
            Ease.Linear => Linear,
            Ease.QuadraticIn => QuadraticIn,
            Ease.QuadraticOut => QuadraticOut,
            Ease.QuadraticInOut => QuadraticInOut,
            Ease.CubicIn => CubicIn,
            Ease.CubicOut => CubicOut,
            Ease.CubicInOut => CubicInOut,
            Ease.QuarticIn => QuarticIn,
            Ease.QuarticOut => QuarticOut,
            Ease.QuarticInOut => QuarticInOut,
            Ease.QuinticIn => QuinticIn,
            Ease.QuinticOut => QuinticOut,
            Ease.QuinticInOut => QuinticInOut,
            Ease.SineIn => SineIn,
            Ease.SineOut => SineOut,
            Ease.SineInOut => SineInOut,
            Ease.CircularIn => CircularIn,
            Ease.CircularOut => CircularOut,
            Ease.CircularInOut => CircularInOut,
            Ease.ExponentialIn => ExponentialIn,
            Ease.ExponentialOut => ExponentialOut,
            Ease.ExponentialInOut => ExponentialInOut,
            Ease.ElasticIn => ElasticIn,
            Ease.ElasticOut => ElasticOut,
            Ease.ElasticInOut => ElasticInOut,
            Ease.BackIn => BackIn,
            Ease.BackOut => BackOut,
            Ease.BackInOut => BackInOut,
            _ => Linear
        };
}

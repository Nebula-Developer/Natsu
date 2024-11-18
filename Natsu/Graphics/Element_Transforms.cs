using Natsu.Mathematics;
using Natsu.Utils;

using SkiaSharp;

namespace Natsu.Graphics;

public partial class Element {
    private Vector2 _anchorPosition;
    private Bounds _bounds = Bounds.Empty;
    private Vector2 _margin;
    private Vector2 _offsetPosition;
    private Vector2 _position;
    private Axes _relativeSizeAxes = Axes.None;
    private float _rotation;
    private Vector2 _scale = new(1, 1);
    private Vector2 _size;
    private Vector2 _worldPosition;
    public DirtyValue<Matrix> DirtyMatrix { get; } = new();

    public Matrix Matrix {
        get {
            if (DirtyMatrix.IsDirty) UpdateMatrix();

            return DirtyMatrix.Value ?? SKMatrix.CreateIdentity();
        }
    }

    public virtual Matrix ChildAccessMatrix => Matrix;

    public Vector2 WorldPosition {
        get {
            if (DirtyMatrix.IsDirty) UpdateMatrix();

            return _worldPosition;
        }
    }

    protected virtual Vector2 ComputeAnchorPosition => AnchorPosition * Parent?.DrawSize ?? Vector2.Zero;

    public Bounds Bounds {
        get {
            if (DirtyMatrix.IsDirty) UpdateMatrix();

            return _bounds;
        }
        private set => _bounds = value;
    }

    public Rect RectBounds => new(0, 0, Size.X, Size.Y);

    public float Rotation {
        get => _rotation;
        set {
            if (_rotation == value) return;

            _rotation = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Axes RelativeSizeAxes {
        get => _relativeSizeAxes;
        set {
            if (_relativeSizeAxes == value) return;

            _relativeSizeAxes = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 Margin {
        get => _margin;
        set {
            if (_margin == value) return;

            _margin = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    public Vector2 DrawSize => (Size - Margin * 2) / (ScaleAffectsDrawSize ? Vector2.One : Scale);

    public Vector2 Size {
        get => _size;
        set {
            if (RelativeSizeAxes == Axes.Both) throw new InvalidOperationException("Cannot set size on element with both relative size axes.");

            Vector2 newValue;
            if (RelativeSizeAxes != Axes.None) {
                if (Parent == null) throw new InvalidOperationException("Cannot set size on element with relative size axes and no parent.");

                newValue = new Vector2(RelativeSizeAxes.HasFlag(Axes.X) ? Parent.Size.X : value.X, RelativeSizeAxes.HasFlag(Axes.Y) ? Parent.Size.Y : value.Y);
            } else
                newValue = value;

            if (_size == newValue) return;

            _size = newValue;
            OnSizeChanged?.Invoke(newValue);
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 OffsetPosition {
        get => _offsetPosition;
        set {
            if (_offsetPosition == value) return;

            _offsetPosition = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 AnchorPosition {
        get => _anchorPosition;
        set {
            if (_anchorPosition == value) return;

            _anchorPosition = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 Position {
        get => _position;
        set {
            if (_position == value) return;

            _position = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 Scale {
        get => _scale;
        set {
            if (_scale == value) return;

            _scale = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 ToLocalSpace(Vector2 screenSpace) => Matrix.Invert().MapPoint(screenSpace);
    public Vector2 ToScreenSpace(Vector2 localSpace) => Matrix.MapPoint(localSpace);

    public void UpdateMatrix() {
        UpdateRelativeSize();

        Matrix matrix = Parent?.ChildAccessMatrix.Copy() ?? new Matrix();
        Vector2 offset = OffsetPosition * Size;

        Vector2 translate = -offset + Position;

        translate += ComputeAnchorPosition + Margin;

        matrix.PreTranslate(translate.X, translate.Y);
        matrix.PreRotate(Rotation, offset.X, offset.Y);
        matrix.PreScale(Scale.X, Scale.Y, offset.X, offset.Y);

        _worldPosition = matrix.MapPoint(Vector2.Zero);
        DirtyMatrix.Validate(matrix);

        Vector2 ds = DrawSize;

        Bounds = Matrix.MapBounds(new Bounds(new Vector2(0, 0), new Vector2(ds.X, 0), new Vector2(ds.X, ds.Y), new Vector2(0, ds.Y)));

        InvalidateChildren(Invalidation.Geometry);
    }

    public void InvalidateChildren(Invalidation invalidation, bool propagate = false) => ForChildren(child => child.Invalidate(invalidation, propagate));

    public void Invalidate(Invalidation invalidation, bool propagate = true, bool deep = false) {
        if (invalidation.HasFlag(Invalidation.Geometry)) DirtyMatrix.Invalidate();

        if (propagate) InvalidateChildren(invalidation, deep);
    }

    public void InvalidateParent(Invalidation invalidation) => Parent?.Invalidate(invalidation, false);

    public event Action<Vector2>? OnSizeChanged;

    public bool ScaleAffectsDrawSize { get; set; } = true;

    public void UpdateRelativeSize() {
        if (RelativeSizeAxes == Axes.None || Parent == null) return;

        Vector2 nSize = new(RelativeSizeAxes.HasFlag(Axes.X) ? Parent.Size.X : Size.X, RelativeSizeAxes.HasFlag(Axes.Y) ? Parent.Size.Y : Size.Y);

        if (_size == nSize) return;

        _size = nSize;
        OnSizeChanged?.Invoke(nSize);
        Invalidate(Invalidation.Geometry);
    }

    public virtual bool PointInside(Vector2 point) => Bounds.Contains(point);
}

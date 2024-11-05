

using Natsu.Mathematics;
using Natsu.Utils;


using SkiaSharp;

namespace Natsu.Graphics;

public partial class Element {
    public DirtyValue<Matrix> DirtyMatrix { get; } = new DirtyValue<Matrix>();
    public Matrix Matrix {
        get {
            if (DirtyMatrix.IsDirty)
                UpdateMatrix();
            return DirtyMatrix.Value ?? SKMatrix.CreateIdentity();
        }
    }

    public Vector2 WorldPosition {
        get {
            if (DirtyMatrix.IsDirty)
                UpdateMatrix();
            return _worldPosition;
        }
    }
    private Vector2 _worldPosition;

    public Vector2 ToLocalSpace(Vector2 screenSpace) => Matrix.Invert().MapPoint(screenSpace);
    public Vector2 ToScreenSpace(Vector2 localSpace) => Matrix.MapPoint(localSpace);

    protected virtual Vector2 ComputeAnchorPosition => AnchorPosition * Parent!.Size;

    public void UpdateMatrix() {
        Matrix matrix = Parent?.Matrix.Copy() ?? new Matrix();
        Vector2 offset = OffsetPosition * Size;

        Vector2 translate = -offset + Position;

        if (Parent != null)
            translate += ComputeAnchorPosition;

        matrix.PreTranslate(translate.X, translate.Y);
        matrix.PreRotate(Rotation, offset.X, offset.Y);
        matrix.PreScale(Scale.X, Scale.Y, offset.X, offset.Y);

        _worldPosition = matrix.MapPoint(Vector2.Zero);
        DirtyMatrix.Validate(matrix);

        Bounds = Matrix.MapBounds(new(
            new(0, 0),
            new(Size.X, 0),
            new(Size.X, Size.Y),
            new(0, Size.Y)
        ));

        InvalidateChildren(Invalidation.Geometry);
    }

    public Bounds Bounds {
        get {
            if (DirtyMatrix.IsDirty)
                UpdateMatrix();
            return _bounds;
        }
        private set => _bounds = value;
    }
    private Bounds _bounds = Bounds.Empty;

    public void InvalidateChildren(Invalidation invalidation, bool propagate = false) =>
        ForChildren(child => child.Invalidate(invalidation, propagate));

    public void Invalidate(Invalidation invalidation, bool propagate = true, bool deep = false) {
        if (invalidation.HasFlag(Invalidation.Geometry))
            DirtyMatrix.Invalidate();
        if (propagate)
            InvalidateChildren(invalidation, deep);
    }

    public void InvalidateParent(Invalidation invalidation) =>
        Parent?.Invalidate(invalidation, false);

    public float Rotation {
        get => _rotation;
        set {
            if (_rotation == value)
                return;
            _rotation = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private float _rotation;

    public Axes RelativeSizeAxes {
        get => _relativeSizeAxes;
        set {
            if (_relativeSizeAxes == value)
                return;
            _relativeSizeAxes = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private Axes _relativeSizeAxes = Axes.None;

    public Vector2 Size {
        get {
            if (RelativeSizeAxes == Axes.None)
                return _size;
            if (Parent == null)
                throw new InvalidOperationException("Cannot get size of element with relative size axes but no parent.");
            return new(RelativeSizeAxes.HasFlag(Axes.X) ? Parent.Size.X : _size.X,
                RelativeSizeAxes.HasFlag(Axes.Y) ? Parent.Size.Y : _size.Y);
        }
        set {
            if (RelativeSizeAxes == Axes.Both)
                throw new InvalidOperationException("Cannot set size on element with both relative size axes.");
            if (_size == value)
                return;
            _size = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private Vector2 _size;

    public Vector2 OffsetPosition {
        get => _offsetPosition;
        set {
            if (_offsetPosition == value)
                return;
            _offsetPosition = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private Vector2 _offsetPosition;

    public Vector2 AnchorPosition {
        get => _anchorPosition;
        set {
            if (_anchorPosition == value)
                return;
            _anchorPosition = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private Vector2 _anchorPosition;

    public Vector2 Position {
        get => _position;
        set {
            if (_position == value)
                return;
            _position = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private Vector2 _position;

    public Vector2 Scale {
        get => _scale;
        set {
            if (_scale == value)
                return;
            _scale = value;
            Invalidate(Invalidation.Geometry);
        }
    }
    private Vector2 _scale = new(1, 1);
}

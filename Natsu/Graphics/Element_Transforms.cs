using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Graphics;

public partial class Element {
    private Vector2 _anchorPosition;
    private Bounds _bounds = Bounds.Empty;
    private Axes _childRelativeSizeAxes = Axes.None;
    private Vector2 _margin;
    private Vector2 _offsetPosition;
    private Vector2 _position;
    private Vector2 _relativeSize;
    private Axes _relativeSizeAxes = Axes.None;
    private float _rotation;
    private Vector2 _scale = new(1, 1);
    private bool _scaleAffectsDrawSize = true;
    private Vector2 _size;
    private Vector2 _worldPosition;
    private Vector2 _worldScale = Vector2.One;
    public DirtyStruct<Matrix> DirtyMatrix { get; } = new();

    public Matrix Matrix {
        get {
            if (DirtyMatrix.IsDirty) UpdateMatrix();

            return DirtyMatrix.Value;
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
    public Vector2 test => ComputeAnchorPosition;

    public Bounds Bounds {
        get {
            if (DirtyDrawSize.IsDirty) UpdateDrawSize();

            return _bounds;
        }
        private set => _bounds = value;
    }

    public Rect RectBounds => new(0, 0, DrawSize.X, DrawSize.Y);

    public float Rotation {
        get => _rotation;
        set {
            if (_rotation == value) return;

            _rotation = value % 360;
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

    public Axes ChildRelativeSizeAxes {
        get => _childRelativeSizeAxes;
        set {
            if (_childRelativeSizeAxes == value) return;

            _childRelativeSizeAxes = value;
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

    public DirtyStruct<Vector2> DirtyDrawSize { get; } = new();
    // public Vector2 DrawSize => (_size - _margin * 2) / (ScaleAffectsDrawSize ? Vector2.One : _scale);
    public Vector2 DrawSize {
        get {
            if (DirtyDrawSize.IsDirty)
                UpdateDrawSize();

            return DirtyDrawSize.Value;
        }
    }

    public Vector2 Size {
        get => _size;
        set {
            if ((RelativeSizeAxes | ChildRelativeSizeAxes) == Axes.Both) throw new InvalidOperationException("Cannot set size on element with both relative size axes.");

            if (_size == value) return;

            _size = value;
            OnSizeChanged?.Invoke(_size);

            Invalidate(Invalidation.DrawSize);
            PropogateChildrenSizeChange();
        }
    }

    public Vector2 RelativeSize {
        get {
            if (RelativeSizeAxes == Axes.None && ChildRelativeSizeAxes == Axes.None) return _size;

            if (DirtyDrawSize.IsDirty) UpdateDrawSize();

            return _relativeSize;
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

    public Vector2 WorldScale {
        get {
            if (DirtyMatrix.IsDirty) UpdateMatrix();

            return _worldScale;
        }
    }

    public bool ScaleAffectsDrawSize {
        get => _scaleAffectsDrawSize;
        set {
            if (_scaleAffectsDrawSize == value) return;

            _scaleAffectsDrawSize = value;
            Invalidate(Invalidation.Geometry);
        }
    }

    public Vector2 ToLocalSpace(Vector2 screenSpace) => Matrix.Invert().MapPoint(screenSpace);
    public Vector2 ToScreenSpace(Vector2 localSpace) => Matrix.MapPoint(localSpace);

    public void UpdateMatrix() {
        UpdateDrawSize();

        Matrix matrix = Parent?.ChildAccessMatrix.Copy() ?? new Matrix();
        Vector2 offset = RelativeSize * OffsetPosition;

        Vector2 translate = -offset + Position;

        translate += ComputeAnchorPosition + Margin;

        matrix.PreTranslate(translate.X, translate.Y);
        matrix.PreRotate(Rotation, offset.X, offset.Y);
        matrix.PreScale(Scale.X, Scale.Y, offset.X, offset.Y);

        _worldScale = matrix.MapPoint(Vector2.One) - matrix.MapPoint(Vector2.Zero);

        _worldPosition = matrix.MapPoint(Vector2.Zero);
        DirtyMatrix.Validate(matrix);

        CalculateBounds();

        InvalidateChildren(Invalidation.Geometry);
    }

    public void InvalidateChildren(Invalidation invalidation, bool propagate = false) => ForChildren(child => {
        Invalidation inv = invalidation;
        if (invalidation.HasFlag(Invalidation.Geometry) && child.DirtyMatrix.IsDirty) inv &= ~Invalidation.Geometry;
        child.Invalidate(inv, propagate);
    });

    public void Invalidate(Invalidation invalidation, bool propagate = true, bool deep = false) {
        if (invalidation == Invalidation.None) return;

        if (invalidation.HasFlag(Invalidation.Geometry)) DirtyMatrix.Invalidate();
        if (invalidation.HasFlag(Invalidation.DrawSize)) DirtyDrawSize.Invalidate();

        if (propagate) InvalidateChildren(invalidation, deep);
    }

    public void InvalidateParent(Invalidation invalidation) => Parent?.Invalidate(invalidation, false);

    public event Action<Vector2>? OnSizeChanged;

    public Bounds GetChildBounds() {
        Bounds bounds = Bounds.Empty;

        ForChildren(child => {
            if (child == null) return;

            bounds = bounds.Union(child.Bounds);
        });

        return bounds;
    }

    public Vector2 GetDistantChildrenPosition() {
        float maxX = 0, maxY = 0;

        ForChildren(child => {
            if (child == null) return;

            float rx = child.RelativeSizeAxes.HasFlag(Axes.X) ? 0 : child.DrawSize.X;
            float ry = child.RelativeSizeAxes.HasFlag(Axes.Y) ? 0 : child.DrawSize.Y;

            Vector2[] points = [
                new(rx, 0),
                new(rx, ry),
                new(0, ry)
            ];

            float x = points.Max(p => p.X);
            float y = points.Max(p => p.Y);

            if (x > maxX) maxX = x;
            if (y > maxY) maxY = y;
        });

        return new Vector2(maxX, maxY);
    }

    private void PropogateChildrenSizeChange() => ForChildren(child => {
        if (child == null) return;

        Invalidation inv = Invalidation.None;

        if (child.RelativeSizeAxes != Axes.None)
            inv |= Invalidation.DrawSize;
        if (child.AnchorPosition != Vector2.Zero)
            inv |= Invalidation.Geometry;

        child.Invalidate(inv, false);
        child.PropogateChildrenSizeChange();
    });

    private Vector2 ReplaceRelativeAxes(Vector2 orig) {
        float newX = orig.X;
        float newY = orig.Y;

        Vector2 distPos = ChildRelativeSizeAxes != Axes.None ? GetDistantChildrenPosition() : Vector2.Zero;

        if (ChildRelativeSizeAxes.HasFlag(Axes.X)) newX = distPos.X;
        else if (RelativeSizeAxes.HasFlag(Axes.X) && Parent != null) newX = Parent.DrawSize.X;

        if (ChildRelativeSizeAxes.HasFlag(Axes.Y)) newY = distPos.Y;
        else if (RelativeSizeAxes.HasFlag(Axes.Y) && Parent != null) newY = Parent.DrawSize.Y;

        return new Vector2(newX, newY);
    }

    public void UpdateDrawSize() {
        Vector2 nSize = _size;

        if (RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None)
            nSize = ReplaceRelativeAxes(nSize);

        _relativeSize = nSize;

        Vector2 drawSize = (nSize - _margin * 2) / (ScaleAffectsDrawSize ? Vector2.One : _scale);
        Vector2 oldValue = DirtyDrawSize.Value;
        DirtyDrawSize.Validate(drawSize);

        if (drawSize == oldValue) return;

        if (Parent?.ChildRelativeSizeAxes != Axes.None) InvalidateParent(Invalidation.DrawSize);
        PropogateChildrenSizeChange();

        if (!DirtyMatrix.IsDirty) CalculateBounds();
    }

    public void CalculateBounds() => Bounds = Matrix.MapBounds(new Bounds(new Vector2(0, 0), new Vector2(DrawSize.X, 0), new Vector2(DrawSize.X, DrawSize.Y), new Vector2(0, DrawSize.Y)));

    public virtual bool PointInside(Vector2 point) => Bounds.Contains(point);
}
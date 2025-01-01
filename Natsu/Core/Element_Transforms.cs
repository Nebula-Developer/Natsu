using Natsu.Mathematics;

namespace Natsu.Core;

public partial class Element {
    private Vector2 _anchorPosition;
    private Bounds _bounds = Bounds.Empty;
    private Axes _childRelativeSizeAxes = Axes.None;

    private Vector2 _drawSize;
    private Vector2 _margin;

    private Matrix _matrix = new();
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

    public Matrix Matrix {
        get {
            if (Invalidated.HasFlag(Invalidation.Geometry)) UpdateMatrix();

            return _matrix;
        }
    }

    public virtual Matrix ChildAccessMatrix => Matrix;

    public Vector2 WorldPosition {
        get {
            if (Invalidated.HasFlag(Invalidation.Geometry)) UpdateMatrix();

            return _worldPosition;
        }
    }

    protected virtual Vector2 ComputeAnchorPosition => AnchorPosition * Parent?.DrawSize ?? Vector2.Zero;

    public Bounds Bounds {
        get {
            if (Invalidated.HasFlag(Invalidation.Size)) UpdateDrawSize();

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

            // Rotation currently does not affect ChildRelativeSizeAxes
            // if (Parent?.ChildRelativeSizeAxes != Axes.None) InvalidateParent(Invalidation.DrawSize);
        }
    }

    public Axes RelativeSizeAxes {
        get => _relativeSizeAxes;
        set {
            if (_relativeSizeAxes == value) return;

            _relativeSizeAxes = value;
            Invalidate(Invalidation.Size);
        }
    }

    public Axes ChildRelativeSizeAxes {
        get => _childRelativeSizeAxes;
        set {
            if (_childRelativeSizeAxes == value) return;

            _childRelativeSizeAxes = value;
            Invalidate(Invalidation.Size);
        }
    }

    public Vector2 Margin {
        get => _margin;
        set {
            if (_margin == value) return;

            _margin = value;
            Invalidate(Invalidation.DrawSize);
        }
    }

    public Vector2 DrawSize {
        get {
            if (Invalidated.HasFlag(Invalidation.Size)) UpdateDrawSize();

            return _drawSize;
        }
    }

    public virtual Vector2 Size {
        get => _size;
        set {
            if ((RelativeSizeAxes | ChildRelativeSizeAxes) == Axes.Both) throw new InvalidOperationException("Cannot set size on element with both relative size axes.");

            if (_size == value) return;

            _size = value;

            Invalidate(Invalidation.Size);
            HandleSizeAffectsMatrix();
            HandleParentSizeChange();
            PropagateChildrenSizeChange();
        }
    }

    public Vector2 RelativeSize {
        get {
            if (RelativeSizeAxes == Axes.None && ChildRelativeSizeAxes == Axes.None) return Size;

            if (Invalidated.HasFlag(Invalidation.Size)) UpdateDrawSize();

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

            HandleParentSizeChange();
        }
    }

    public Vector2 Scale {
        get => _scale;
        set {
            if (_scale == value) return;

            _scale = value;
            Invalidate(Invalidation.DrawSize);
        }
    }

    public Vector2 WorldScale {
        get {
            if (Invalidated.HasFlag(Invalidation.Geometry)) UpdateMatrix();

            return _worldScale;
        }
    }

    public bool ScaleAffectsDrawSize {
        get => _scaleAffectsDrawSize;
        set {
            if (_scaleAffectsDrawSize == value) return;

            _scaleAffectsDrawSize = value;
            Invalidate(Invalidation.DrawSize);
        }
    }

    public bool SizeAffectsMatrix => RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None || OffsetPosition != Vector2.Zero || AnchorPosition != Vector2.Zero || Margin != Vector2.Zero || Rotation != 0 || Scale != Vector2.One;

    public bool HandleSizeAffectsMatrix() {
        if (SizeAffectsMatrix) {
            Invalidate(Invalidation.Geometry);
            return true;
        }

        return false;
    }

    public Vector2 ToLocalSpace(Vector2 screenSpace) => Matrix.Invert().MapPoint(screenSpace);

    public Vector2 ToScreenSpace(Vector2 localSpace) => Matrix.MapPoint(localSpace);

    public void UpdateMatrix() {
        if (Invalidated.HasFlag(Invalidation.Size)) UpdateDrawSize();

        Matrix matrix = Parent?.ChildAccessMatrix.Copy() ?? new Matrix();
        Vector2 offset = RelativeSize * OffsetPosition;

        Vector2 translate = -offset + Position;

        translate += ComputeAnchorPosition + Margin;

        matrix.PreTranslate(translate.X, translate.Y);
        matrix.PreRotate(Rotation, offset.X - Margin.X, offset.Y - Margin.Y);
        matrix.PreScale(Scale.X, Scale.Y, offset.X, offset.Y);

        _worldScale = matrix.MapPoint(Vector2.One) - matrix.MapPoint(Vector2.Zero);

        Vector2 nWorldPosition = matrix.MapPoint(Vector2.Zero);
        if (_worldPosition != nWorldPosition) {
            _worldPosition = nWorldPosition;
            DoWorldPositionChange?.Invoke(_worldPosition);
            OnWorldPositionChange(_worldPosition);
        }

        _matrix = matrix;
        Validate(Invalidation.Geometry);

        CalculateBounds();

        InvalidateChildren(Invalidation.Geometry);
    }

    public event Action<Vector2>? DoDrawSizeChange;

    protected virtual void OnDrawSizeChange(Vector2 size) { }

    public event Action<Vector2>? DoWorldPositionChange;

    protected virtual void OnWorldPositionChange(Vector2 position) { }

    public Vector2 GetChildSpan() {
        float spanX = 0, spanY = 0;

        ForChildren(child => {
            if (child.RelativeSizeAxes == Axes.Both) return;

            Vector2 childDrawSize;

            if (child.RelativeSizeAxes != Axes.None)
                childDrawSize = child.Invalidated.HasFlag(Invalidation.Size) ? child.ParentAccessDrawSize() : child.DrawSize;
            else
                childDrawSize = child.DrawSize;

            Vector2 size = childDrawSize * child.Scale;
            Vector2 offset = child.AnchorPosition * _drawSize + child.OffsetPosition * size + child.Position;

            float realX = child.RelativeSizeAxes.HasFlag(Axes.X) ? 0 : offset.X + size.X;
            float realY = child.RelativeSizeAxes.HasFlag(Axes.Y) ? 0 : offset.Y + size.Y;

            spanX = Math.Max(spanX, realX);
            spanY = Math.Max(spanY, realY);
        });

        return new(spanX, spanY);
    }

    private void PropagateChildrenSizeChange() =>
        ForChildren(child => {
            Invalidation inv = Invalidation.None;

            if (child.RelativeSizeAxes != Axes.None) inv |= Invalidation.DrawSize;

            if (child.AnchorPosition != Vector2.Zero) inv |= Invalidation.Geometry;

            child.Invalidate(inv);
        });

    private Vector2 ReplaceRelativeAxes(Vector2 orig, bool accessParent = true) {
        float newX = orig.X;
        float newY = orig.Y;

        Vector2 distPos = ChildRelativeSizeAxes != Axes.None ? GetChildSpan() : Vector2.Zero;

        if (ChildRelativeSizeAxes.HasFlag(Axes.X))
            newX = distPos.X;
        else if (accessParent && RelativeSizeAxes.HasFlag(Axes.X) && Parent != null) newX = Parent.DrawSize.X;

        if (ChildRelativeSizeAxes.HasFlag(Axes.Y))
            newY = distPos.Y;
        else if (accessParent && RelativeSizeAxes.HasFlag(Axes.Y) && Parent != null) newY = Parent.DrawSize.Y;

        return new(newX, newY);
    }

    private Vector2 ApplySizeEffects(Vector2 size) => (size - Margin * 2) / (ScaleAffectsDrawSize ? Vector2.One : Scale);

    private Vector2 ParentAccessDrawSize() {
        Vector2 nSize = Size;

        if (RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None) nSize = ReplaceRelativeAxes(nSize, false);

        return ApplySizeEffects(nSize);
    }

    public void UpdateDrawSize() {
        Vector2 nSize = Size;

        if (RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None) nSize = ReplaceRelativeAxes(nSize);

        _relativeSize = nSize;

        Vector2 drawSize = ApplySizeEffects(nSize);
        Vector2 oldValue = _drawSize;
        _drawSize = drawSize;

        Validate(Invalidation.Size);

        if (drawSize == oldValue) return;

        HandleSizeAffectsMatrix();

        HandleParentSizeChange();
        PropagateChildrenSizeChange();

        CalculateBounds();

        DoDrawSizeChange?.Invoke(drawSize);
        OnDrawSizeChange(drawSize);
    }

    private void PropagateParentSizeDependencies() {
        if (Parent?.ChildRelativeSizeAxes != Axes.None) {
            InvalidateParent(Invalidation.DrawSize);
            Parent?.HandleParentSizeChange();
        }
    }

    protected void HandleParentSizeChange() {
        InvalidateParent(Invalidation.Layout);
        PropagateParentSizeDependencies();
    }

    public void CalculateBounds() => Bounds = Matrix.MapBounds(new(new(0, 0), new(DrawSize.X, 0), new(DrawSize.X, DrawSize.Y), new(0, DrawSize.Y)));

    public virtual bool PointInside(Vector2 point) => Bounds.Contains(point);
}

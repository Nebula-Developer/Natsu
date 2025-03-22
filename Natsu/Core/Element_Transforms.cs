using Natsu.Core.InvalidationTemp;
using Natsu.Mathematics;

namespace Natsu.Core;

public partial class Element {
    private Vector2 _anchorPosition;
    private Bounds _bounds = Bounds.Empty;
    private Axes _childRelativeSizeAxes = Axes.None;

    private Vector2 _drawSize;
    private Margin _margin;

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

    /// <summary>
    ///     The matrix that transforms the element into screen space.
    /// </summary>
    public virtual Matrix Matrix {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Geometry)) UpdateMatrix();

            return _matrix;
        }
    }

    /// <summary>
    ///     A virtual matrix that is used by children to transform into this element's space.
    /// </summary>
    public virtual Matrix ChildAccessMatrix => Matrix;

    /// <summary>
    ///     The position of the element in screen space.
    /// </summary>
    public virtual Vector2 WorldPosition {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Geometry)) UpdateMatrix();

            return _worldPosition;
        }
    }

    /// <summary>
    ///     A virtual position calculation that is used to compute the anchor position under the parent's space.
    /// </summary>
    protected virtual Vector2 ComputeAnchorPosition => AnchorPosition * Parent?.DrawSize ?? Vector2.Zero;

    /// <summary>
    ///     The bounds of the element in screen space.
    /// </summary>
    public virtual Bounds Bounds {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Size)) UpdateDrawSize();

            return _bounds;
        }
        private set => _bounds = value;
    }

    /// <summary>
    ///     A rectangle that represents the local bounds of the element's draw size as a <see cref="Rect" />.
    /// </summary>
    public Rect RectBounds => new(0, 0, DrawSize.X, DrawSize.Y);

    /// <summary>
    ///     The rotation of the element in degrees.
    /// </summary>
    public virtual float Rotation {
        get => _rotation;
        set {
            if (_rotation == value) return;

            _rotation = value % 360;
            Invalidate(ElementInvalidation.Geometry);

            // Rotation currently does not affect ChildRelativeSizeAxes
            // if (Parent?.ChildRelativeSizeAxes != Axes.None) InvalidateParent(Invalidation.DrawSize);
        }
    }

    /// <summary>
    ///     Specifies the axes that are relative to the parent's size.
    ///     <br /><br />
    ///     This value overrides <see cref="Size" />.
    ///     <br />
    ///     To avoid overriding, use <see cref="RawRelativeSizeAxes" />.
    /// </summary>
    public Axes RelativeSizeAxes {
        get => _relativeSizeAxes;
        set {
            if (_relativeSizeAxes == value) return;

            _relativeSizeAxes = value; // Avoid recursion here
            Size = new(RelativeSizeAxes.HasFlag(Axes.X) ? 1 : Size.X, RelativeSizeAxes.HasFlag(Axes.Y) ? 1 : Size.Y);
        }
    }

    /// <summary>
    ///     Specifies the raw axes that are relative to the parent's size,
    ///     without affecting the <see cref="Size" /> property.
    /// </summary>
    public Axes RawRelativeSizeAxes {
        get => _relativeSizeAxes;
        set {
            if (_relativeSizeAxes == value) return;

            _relativeSizeAxes = value;
            Invalidate(ElementInvalidation.Size);
        }
    }

    /// <summary>
    ///     Specifies the axes that are relative to the child's size.
    ///     <br /><br />
    ///     This value overrides <see cref="Size" />.
    ///     <br />
    ///     To avoid overriding, use <see cref="RawChildRelativeSizeAxes" />.
    /// </summary>
    public Axes ChildRelativeSizeAxes {
        get => _childRelativeSizeAxes;
        set {
            if (_childRelativeSizeAxes == value) return;

            _childRelativeSizeAxes = value; // Avoid recursion
            Size = new(ChildRelativeSizeAxes.HasFlag(Axes.X) ? 1 : Size.X, ChildRelativeSizeAxes.HasFlag(Axes.Y) ? 1 : Size.Y);
        }
    }

    /// <summary>
    ///     Specifies the raw axes that are relative to the child's size,
    ///     without affecting the <see cref="Size" /> property.
    /// </summary>
    public Axes RawChildRelativeSizeAxes {
        get => _childRelativeSizeAxes;
        set {
            if (_childRelativeSizeAxes == value) return;

            _childRelativeSizeAxes = value;
            Invalidate(ElementInvalidation.Size);
        }
    }

    /// <summary>
    ///     The margin of the element.
    /// </summary>
    public virtual Margin Margin {
        get => _margin;
        set {
            if (_margin == value) return;

            _margin = value;
            Invalidate(ElementInvalidation.DrawSize);
        }
    }

    /// <summary>
    ///     The size of the element in screen space, after applying all size-related transformations.
    ///     <br />
    ///     This value should almost <i>always</i> be used over <see cref="Size" /> for rendering purposes.
    /// </summary>
    public virtual Vector2 DrawSize {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Size)) UpdateDrawSize();

            return _drawSize;
        }
    }

    /// <summary>
    ///     The raw size of the element.
    ///     <br />
    ///     If <see cref="RelativeSizeAxes" /> or <see cref="ChildRelativeSizeAxes" /> are set, the size will be used as a
    ///     multiplier for those axes.
    /// </summary>
    public virtual Vector2 Size {
        get => _size;
        set {
            if (_size == value) return;

            _size = Vector2.Max(value, Vector2.Zero);

            Invalidate(ElementInvalidation.Size);
            HandleSizeAffectsMatrix();
            HandleParentSizeChange();
            PropagateChildrenSizeChange();
        }
    }

    /// <summary>
    ///     The size from <see cref="RelativeSizeAxes" /> and <see cref="ChildRelativeSizeAxes" /> computation.
    /// </summary>
    public virtual Vector2 RelativeSize {
        get {
            if (RelativeSizeAxes == Axes.None && ChildRelativeSizeAxes == Axes.None) return Size;

            if (Invalidated.HasFlag(ElementInvalidation.Size)) UpdateDrawSize();

            return _relativeSize;
        }
    }

    /// <summary>
    ///     The offset position of the element.
    ///     <br />
    ///     This value subtracts element's position by its own size.
    ///     <br />
    ///     For instance, if the offset position is set to <c>(0.5, 0.5)</c>, the element will move back by half of its size.
    /// </summary>
    public virtual Vector2 OffsetPosition {
        get => _offsetPosition;
        set {
            if (_offsetPosition == value) return;

            _offsetPosition = value;
            Invalidate(ElementInvalidation.Geometry);
        }
    }

    /// <summary>
    ///     The anchor position of the element.
    ///     <br />
    ///     This value adds element's position by its parent's size.
    ///     <br />
    ///     For instance, if the anchor position is set to <c>(0.5, 0.5)</c>, the element will move forward by half of its
    ///     parent's size.
    /// </summary>
    public virtual Vector2 AnchorPosition {
        get => _anchorPosition;
        set {
            if (_anchorPosition == value) return;

            _anchorPosition = value;
            Invalidate(ElementInvalidation.Geometry);
        }
    }

    /// <summary>
    ///     Controls both <see cref="AnchorPosition" /> and <see cref="OffsetPosition" /> to be the same value.
    /// </summary>
    public Vector2 Pivot {
        set {
            AnchorPosition = value;
            OffsetPosition = value;
        }
        get => AnchorPosition;
    }

    /// <summary>
    ///     The local position of the element.
    ///     <br />
    ///     Used as an offset for all position-related transformations.
    /// </summary>
    public virtual Vector2 Position {
        get => _position;
        set {
            if (_position == value) return;

            _position = value;
            Invalidate(ElementInvalidation.Geometry);

            HandleParentSizeChange();
        }
    }

    /// <summary>
    ///     The scale of the element.
    /// </summary>
    public virtual Vector2 Scale {
        get => _scale;
        set {
            if (_scale == value) return;

            _scale = Vector2.Max(value, new(float.MinValue));
            Invalidate(ElementInvalidation.DrawSize);
            Parent?.Invalidate(ElementInvalidation.Layout);
        }
    }

    /// <summary>
    ///     The scale of the element in world space.
    /// </summary>
    public virtual Vector2 WorldScale {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Geometry)) UpdateMatrix();

            return _worldScale;
        }
    }

    /// <summary>
    ///     Whether the scale should divide the draw size.
    ///     <br />
    ///     Use this to keep the element's size consistent when scaling.
    /// </summary>
    public bool ScaleAffectsDrawSize {
        get => _scaleAffectsDrawSize;
        set {
            if (_scaleAffectsDrawSize == value) return;

            _scaleAffectsDrawSize = value;
            Invalidate(ElementInvalidation.DrawSize);
        }
    }

    /// <summary>
    ///     Computation to determine whether a change in <see cref="DrawSize" /> affects the matrix.
    /// </summary>
    public bool SizeAffectsMatrix => OffsetPosition != Vector2.Zero || AnchorPosition != Vector2.Zero || RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None || Margin != 0 || Rotation != 0 || Scale != Vector2.One;

    /// <summary>
    ///     Handles geometric invalidation if <see cref="SizeAffectsMatrix" /> is true.
    /// </summary>
    /// <returns>Whether the invalidation was handled</returns>
    public bool HandleSizeAffectsMatrix() {
        if (SizeAffectsMatrix) {
            Invalidate(ElementInvalidation.Geometry);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Transforms a point from screen space to local space.
    /// </summary>
    /// <param name="screenSpace">The point in screen space</param>
    /// <returns>The point in local space</returns>
    public Vector2 ToLocalSpace(Vector2 screenSpace) => Matrix.Invert().MapPoint(screenSpace);

    /// <summary>
    ///     Transforms a point from local space to screen space.
    /// </summary>
    /// <param name="localSpace">The point in local space</param>
    /// <returns>The point in screen space</returns>
    public Vector2 ToScreenSpace(Vector2 localSpace) => Matrix.MapPoint(localSpace);

    protected virtual void ProcessMatrix(ref Matrix matrix) {
        Vector2 offset = RelativeSize * OffsetPosition;
        Vector2 translate = -offset + Position;

        translate += ComputeAnchorPosition + Margin.TopLeft;

        matrix.PreTranslate(translate.X, translate.Y);
        matrix.PreRotate(Rotation, offset.X - Margin.Left, offset.Y - Margin.Top);
        matrix.PreScale(Scale.X, Scale.Y, offset.X, offset.Y);
    }

    /// <summary>
    ///     Updates the matrix of the element.
    /// </summary>
    public void UpdateMatrix() {
        if (Invalidated.HasFlag(ElementInvalidation.Size)) UpdateDrawSize();

        Matrix matrix = Parent?.ChildAccessMatrix.Copy() ?? new Matrix();

        ProcessMatrix(ref matrix);

        _worldScale = matrix.MapPoint(Vector2.One) - matrix.MapPoint(Vector2.Zero);

        Vector2 nWorldPosition = matrix.MapPoint(Vector2.Zero);
        if (_worldPosition != nWorldPosition) {
            _worldPosition = nWorldPosition;

            DoWorldPositionChange?.Invoke(_worldPosition);
            OnWorldPositionChange(_worldPosition);

            if (UpdateShaderPosition && Shader != null) Shader.SetUniform("pos", _worldPosition);
        }

        _matrix = matrix;
        Validate(ElementInvalidation.Geometry);

        CalculateBounds();

        InvalidateChildren(ElementInvalidation.Geometry);
    }

    public event Action<Vector2>? DoDrawSizeChange;

    protected virtual void OnDrawSizeChange(Vector2 size) { }

    public event Action<Vector2>? DoWorldPositionChange;

    protected virtual void OnWorldPositionChange(Vector2 position) { }

    /// <summary>
    ///     Gets the span of all children based on their positions, scales, and sizes.
    /// </summary>
    /// <returns>The span of all children</returns>
    public Vector2 GetChildSpan() {
        float spanX = 0, spanY = 0;

        ForChildren(child => {
            if (child.RelativeSizeAxes == Axes.Both) return;

            Vector2 childDrawSize;

            if (child.RelativeSizeAxes != Axes.None)
                childDrawSize = child.Invalidated.HasFlag(ElementInvalidation.Size) ? child.ParentAccessDrawSize() : child.DrawSize;
            else
                childDrawSize = child.DrawSize;

            Vector2 size = childDrawSize * child.Scale;
            Vector2 offset = -(child.OffsetPosition * size) + child.Position;

            float realX = child.RelativeSizeAxes.HasFlag(Axes.X) ? 0 : offset.X + size.X;
            float realY = child.RelativeSizeAxes.HasFlag(Axes.Y) ? 0 : offset.Y + size.Y;

            spanX = Math.Max(spanX, realX);
            spanY = Math.Max(spanY, realY);
        });

        return new(spanX, spanY);
    }

    private void PropagateChildrenSizeChange() =>
        ForChildren(child => {
            ElementInvalidation inv = ElementInvalidation.None;

            if (child.RelativeSizeAxes != Axes.None) inv |= ElementInvalidation.DrawSize;

            if (child.AnchorPosition != Vector2.Zero) inv |= ElementInvalidation.Geometry;

            child.Invalidate(inv);
        });

    private Vector2 ReplaceRelativeAxes(Vector2 orig, bool accessParent = true) {
        float newX = orig.X;
        float newY = orig.Y;

        Vector2 distPos = ChildRelativeSizeAxes != Axes.None ? GetChildSpan() : Vector2.Zero;

        if (ChildRelativeSizeAxes.HasFlag(Axes.X))
            newX = distPos.X * orig.X;
        else if (accessParent && RelativeSizeAxes.HasFlag(Axes.X) && Parent != null) newX = Parent.DrawSize.X * orig.X;

        if (ChildRelativeSizeAxes.HasFlag(Axes.Y))
            newY = distPos.Y * orig.Y;
        else if (accessParent && RelativeSizeAxes.HasFlag(Axes.Y) && Parent != null) newY = Parent.DrawSize.Y * orig.Y;

        return new(newX, newY);
    }

    private Vector2 ApplySizeEffects(Vector2 size) => (size - Margin.Size) / (ScaleAffectsDrawSize ? Vector2.One : Scale);

    private Vector2 ParentAccessDrawSize() {
        Vector2 nSize = Size;

        if (RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None) nSize = ReplaceRelativeAxes(nSize, false);

        return ApplySizeEffects(nSize);
    }

    /// <summary>
    ///     Updates the draw size of the element.
    /// </summary>
    public void UpdateDrawSize() {
        Vector2 nSize = Size;

        if (RelativeSizeAxes != Axes.None || ChildRelativeSizeAxes != Axes.None) nSize = ReplaceRelativeAxes(nSize);

        _relativeSize = nSize;

        Vector2 drawSize = ApplySizeEffects(nSize);
        Vector2 oldValue = _drawSize;
        _drawSize = drawSize;

        Validate(ElementInvalidation.Size);

        if (drawSize == oldValue) return;

        HandleSizeAffectsMatrix();

        HandleParentSizeChange();
        PropagateChildrenSizeChange();

        CalculateBounds();

        DoDrawSizeChange?.Invoke(drawSize);
        OnDrawSizeChange(drawSize);

        if (UpdateShaderResolution && Shader != null) Shader.SetUniform("resolution", drawSize);
    }

    private void PropagateParentSizeDependencies() {
        if (Parent?.ChildRelativeSizeAxes != Axes.None) {
            InvalidateParent(ElementInvalidation.DrawSize);
            Parent?.HandleParentSizeChange();
        }
    }

    protected void HandleParentSizeChange() {
        InvalidateParent(ElementInvalidation.Layout);
        PropagateParentSizeDependencies();
    }

    /// <summary>
    ///     Calculates the bounds of the element.
    /// </summary>
    public void CalculateBounds() => Bounds = Matrix.MapBounds(new(new(0, 0), new(DrawSize.X, 0), new(DrawSize.X, DrawSize.Y), new(0, DrawSize.Y)));

    /// <summary>
    ///     Virtual method that returns whether a point falls inside the element's bounds.
    ///     <br />
    ///     This is primarily used for positional input checks.
    /// </summary>
    /// <param name="point">The point to check</param>
    /// <returns>Whether the point is inside the element</returns>
    public virtual bool PointInside(Vector2 point) => Bounds.Contains(point);
}

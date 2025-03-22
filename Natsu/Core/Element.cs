using Natsu.Core.InvalidationTemp;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;
using Natsu.Native;

namespace Natsu.Core;

/// <summary>
///     Represents an element in the UI hierarchy.
///     <br />
///     Elements are the building blocks of a Natsu application, and are used to represent
///     any visual or non-visual object in the interface.
/// </summary>
public partial class Element : TransformSequenceManager, IDisposable, ITransformable {
    private Application? _app;
    private int _index;
    private string? _name;

    private INativePlatform _platform = new BatchNativePlatform();

    /// <summary>
    ///     Whether this element should dispose its children when it is disposed.
    /// </summary>
    public bool PropagateChildrenDisposal { get; set; } = true;

    /// <summary>
    ///     Whether this element should be updated by the application.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    ///     Whether this element should be rendered by the application.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    ///     Controls both the <see cref="Active" /> and <see cref="Visible" /> properties.
    /// </summary>
    public bool Enabled {
        get => Active && Visible;
        set {
            Active = value;
            Visible = value;
        }
    }

    /// <summary>
    ///     Whether to prevent this element (and its children) from handling positional input.
    ///     <br />
    ///     Make sure to rebuild the input lists after changing this property.
    /// </summary>
    public virtual bool BlockPositionalInput { get; set; }

    /// <summary>
    ///     Whether this element is loaded. Gets automatically set to true when <see cref="Load" /> is called.
    /// </summary>
    public bool Loaded { get; set; }

    /// <summary>
    ///     The name of this element.
    /// </summary>
    public virtual string Name {
        get => _name ?? GetType().Name;
        set => _name = value;
    }

    /// <summary>
    ///     The <see cref="IRenderer" /> used by the application.
    ///     <br />
    ///     Used to send agnostic rendering commands to the <see cref="ICanvas" /> renderer.
    /// </summary>
    public IRenderer Renderer => App.Renderer;

    /// <summary>
    ///     The <see cref="ResourceLoader" /> used by the application.
    /// </summary>
    public ResourceLoader ResourceLoader => App?.ResourceLoader ?? new NullResourceLoader();

    /// <summary>
    ///     The <see cref="INativePlatform" /> used by the application.
    ///     <br />
    ///     Used to send platform-specific commands to the appplication/platform.
    /// </summary>
    public INativePlatform Platform {
        get => _platform;
        set {
            if (_platform is BatchNativePlatform batch) batch.Apply(value);
            _platform = value;
        }
    }

    /// <summary>
    ///     The index of this element used to sort it in the parent's children list.
    /// </summary>
    public int Index {
        get => _index;
        set {
            if (_index == value) return;

            _index = value;
            Parent?.SortChild(this);
        }
    }

    /// <summary>
    ///     Whether to clip this element's children using <see cref="ClipCanvas" />.
    /// </summary>
    public bool Clip { get; set; } = false;

    /// <summary>
    ///     The corner radius of the clip if <see cref="Clip" /> is true. Used for rounded clipping.
    /// </summary>
    public Vector2 ClipRadius { get; set; } = 0;

    /// <summary>
    ///     Whether to clip the difference between the element and its children.
    /// </summary>
    public bool ClipDifference { get; set; } = false;

    /// <summary>
    ///     Whether to use anti-aliasing when clipping.
    /// </summary>
    public bool ClipAntiAlias { get; set; } = true;

    /// <summary>
    ///     Disposes this element, and its children if <see cref="PropagateChildrenDisposal" /> is true.
    /// </summary>
    public void Dispose() {
        OnDispose();
        DoDispose?.Invoke();
        Parent?.Remove(this);
        if (PropagateChildrenDisposal) Clear(true);
    }

    /// <summary>
    ///     Schedules a task to be executed after a certain amount of time.
    /// </summary>
    /// <param name="time">The time to wait before executing the task</param>
    /// <param name="task">The task to execute</param>
    /// <returns>The scheduled task</returns>
    public ScheduledTask Schedule(double time, Action task) => App.Scheduler.Schedule(time, task);

    /// <summary>
    ///     Loads this element and its children.
    /// </summary>
    /// <returns>Whether the element's load methods were called.</returns>
    public bool Load() {
        if (Loaded) return false;

        OnLoad();
        DoLoad?.Invoke();
        Loaded = true;

        ForChildren(child => child.Load());

        return true;
    }

    /// <summary>
    ///     Virtual method used to handle the clipping of the element.
    /// </summary>
    /// <param name="canvas">The canvas to clip</param>
    public virtual void ClipCanvas(ICanvas canvas) {
        if (ClipRadius == Vector2.Zero) {
            canvas.ClipRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipDifference, ClipAntiAlias);
            return;
        }

        canvas.ClipRoundRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipRadius, ClipDifference, ClipAntiAlias);
    }

    /// <summary>
    ///     Renders this element and its children.
    /// </summary>
    /// <param name="canvas">The canvas to render to</param>
    public void Render(ICanvas canvas) {
        if (!Visible) return;

        canvas.SetMatrix(Matrix);
        int save = canvas.Save();
        if (Clip) ClipCanvas(canvas);

        OnRender(canvas);
        // Any bound rendering can happen here
        // eg:
        // canvas.DrawRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), new Paint() { Color = Colors.Magenta, IsStroke = true, StrokeWidth = 2 });
        OnRenderChildren(canvas);
        DoRender?.Invoke();

        canvas.Restore(save);
    }

    /// <summary>
    ///     Updates this element and its children.
    /// </summary>
    public void Update(double deltaTime) {
        if (!Active) return;

        UpdateTransformSequences(deltaTime);
        UpdateShader();

        OnUpdate(deltaTime);
        DoUpdate?.Invoke(deltaTime);
        OnUpdateChildren(deltaTime);
    }

    protected virtual void OnLoad() { }

    protected virtual void OnDispose() { }

    protected virtual void OnUpdate(double deltaTime) { }

    protected virtual void OnUpdateChildren(double deltaTime) => ForChildren(child => child.Update(deltaTime));

    protected virtual void OnRender(ICanvas canvas) { }

    protected virtual void OnRenderChildren(ICanvas canvas) => ForChildren(child => child.Render(canvas));

    protected virtual void OnAppChange(Application? old) { }

    public event Action? DoLoad;
    public event Action? DoDispose;

    public event Action<double>? DoUpdate;
    public event Action? DoRender;

    public event Action<Application>? DoAppChange;

    #region Invalidation
    /// <summary>
    ///     The invalidation state handler of this element.
    /// </summary>
    public ElementInvalidator InvalidationState { get; } = new();

    /// <summary>
    ///     The invalidation state of this element.
    /// </summary>
    public ElementInvalidation Invalidated => InvalidationState.Element;

    /// <summary>
    ///     The custom invalidation state of this element.
    /// </summary>
    public CustomInvalidation CustomInvalidated => InvalidationState.Custom;

    /// <summary>
    ///     Virtual method used to handle any custom invalidation logic.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagation">The directions to propagate the invalidation</param>
    /// <returns>Whether to block the default invalidation propagation</returns>
    public virtual bool OnInvalidate(ElementInvalidation invalidation, InvalidationPropagation propagation) {
        if (invalidation.HasFlag(ElementInvalidation.DrawSize) && Parent?.ChildRelativeSizeAxes != Axes.None) InvalidateParent(ElementInvalidation.DrawSize);

        return false;
    }

    /// <summary>
    ///     Virtual method used to handle any custom validation logic.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagation">The directions to propagate the invalidation</param>
    /// <returns>Whether to block the default invalidation propagation</returns>
    public virtual bool OnValidate(ElementInvalidation invalidation, InvalidationPropagation propagation) => false;

    /// <summary>
    ///     Invalidates this element and propagates the invalidation to its parent and/or children.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagation">The directions to propagate the invalidation</param>
    /// <returns>Whether the invalidation was successful</returns>
    public bool Invalidate(ElementInvalidation invalidation, InvalidationPropagation propagation = InvalidationPropagation.None) {
        if (invalidation == ElementInvalidation.None) return false;

        if (OnInvalidate(invalidation, propagation)) return true;

        InvalidationState.Invalidate(invalidation);

        if (propagation.HasFlag(InvalidationPropagation.Parent)) InvalidateParent(invalidation, true);

        if (propagation.HasFlag(InvalidationPropagation.Children)) InvalidateChildren(invalidation, true);

        return true;
    }

    /// <summary>
    ///     Validates this element and propagates the invalidation to its parent and/or children.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagation">The directions to propagate the invalidation</param>
    /// <returns>Whether the validation was successful</returns>
    public bool Validate(ElementInvalidation invalidation, InvalidationPropagation propagation = InvalidationPropagation.None) {
        if (invalidation == ElementInvalidation.None) return false;

        if (OnValidate(invalidation, propagation)) return true;

        InvalidationState.Validate(invalidation);

        if (propagation.HasFlag(InvalidationPropagation.Parent)) ValidateParent(invalidation, true);

        if (propagation.HasFlag(InvalidationPropagation.Children)) ValidateChildren(invalidation, true);

        return true;
    }

    /// <summary>
    ///     Invalidates the parent of this element.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagate">Whether to propagate the invalidation up the hierarchy</param>
    /// <returns>Whether the invalidation was successful</returns>
    public bool InvalidateParent(ElementInvalidation invalidation, bool propagate = false) => Parent?.Invalidate(invalidation, propagate ? InvalidationPropagation.Parent : InvalidationPropagation.None) ?? false;

    /// <summary>
    ///     Validates the parent of this element.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagate">Whether to propagate the invalidation up the hierarchy</param>
    /// <returns>Whether the validation was successful</returns>
    public bool ValidateParent(ElementInvalidation invalidation, bool propagate = false) => Parent?.Validate(invalidation, propagate ? InvalidationPropagation.Parent : InvalidationPropagation.None) ?? false;

    /// <summary>
    ///     Invalidates the children of this element.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagate">Whether to propagate the invalidation down the hierarchy</param>
    /// <returns>Whether the invalidation was successful</returns>
    public bool InvalidateChildren(ElementInvalidation invalidation, bool propagate = false) {
        bool result = false;
        ForChildren(child => result |= child.Invalidate(invalidation, propagate ? InvalidationPropagation.Children : InvalidationPropagation.None));
        return result;
    }

    /// <summary>
    ///     Validates the children of this element.
    /// </summary>
    /// <param name="invalidation">The invalidation flags</param>
    /// <param name="propagate">Whether to propagate the invalidation down the hierarchy</param>
    /// <returns>Whether the validation was successful</returns>
    public bool ValidateChildren(ElementInvalidation invalidation, bool propagate = false) {
        bool result = false;
        ForChildren(child => result |= child.Validate(invalidation, propagate ? InvalidationPropagation.Children : InvalidationPropagation.None));
        return result;
    }
    #endregion

#nullable disable
    /// <summary>
    ///     The <see cref="Application" /> this element belongs to.
    ///     <br />
    ///     Can be null if the element is not part of a UI hierarchy, but that will result
    ///     in the lack of Application-spesific classes like <see cref="Renderer" />, <see cref="ResourceLoader" />,
    ///     and <see cref="Platform" />.
    /// </summary>
    public Application App {
        get {
            if (_app != null) return _app;

            if (Parent != null) {
                App = Parent.App;
                return _app;
            }

            return null;
        }
        set => setApp(value);
    }

    private bool setApp(Application app, bool constructInputLists = false) {
        if (_app == app) return false;

        _app?.RemoveInputCandidate(this);

        Application old = _app;
        _app = app;
        Platform = app?.Platform;

        ForChildren(child => child.setApp(app));

        DoAppChange?.Invoke(old);
        OnAppChange(old);

        return true;
    }

    public override string ToString() => Name;
}

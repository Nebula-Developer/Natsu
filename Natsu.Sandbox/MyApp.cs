using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class Slider : Element {
    public RectElement Background, Bar, Thumb;
    public Element ThumbContainer;
    public InputElement ThumbInput;

    public Slider() {
        Children = [
            Background = new RectElement {
                ContentParent = this,
                RelativeSizeAxes = Axes.Both,
                Paint = new Paint {
                    Color = new Color(100, 110, 130),
                    IsAntialias = true,
                    FilterQuality = FilterQuality.Medium
                },
                RoundedCorners = new Vector2(10),
                Content = [
                    Bar = new RectElement {
                        ContentParent = this,
                        RelativeSizeAxes = Axes.Both,
                        Paint = new Paint {
                            Color = new Color(170, 190, 200),
                            IsAntialias = true,
                            FilterQuality = FilterQuality.Medium
                        },
                        RoundedCorners = new Vector2(7.5f),
                        Margin = new Vector2(5),
                        Content = [
                            ThumbContainer = new Element {
                                Margin = new Vector2(5, 0),
                                RelativeSizeAxes = Axes.Both,
                                Content = [
                                    Thumb = new RectElement {
                                        ContentParent = this,
                                        Size = new Vector2(30),
                                        RelativeSizeAxes = Axes.Y,
                                        Margin = new Vector2(0, 5),
                                        Paint = new Paint {
                                            Color = new Color(200, 230, 255),
                                            IsAntialias = true,
                                            FilterQuality = FilterQuality.Medium
                                        },
                                        RoundedCorners = new Vector2(3),
                                        AnchorPosition = new Vector2(1f, 0.5f),
                                        OffsetPosition = new Vector2(1f, 0.5f)
                                    }
                                ]
                            }
                        ]
                    }
                ],
                AnchorPosition = new Vector2(0.5f),
                OffsetPosition = new Vector2(0.5f)
            },
            ThumbInput = new InputElement {
                ContentParent = this,
                RelativeSizeAxes = Axes.Both,
                GrabFallback = true,
                Cursor = CursorStyle.ResizeHorizontal
            }
        ];

        bool pressed = false;
        float value = 0;

        void updatePos(Vector2 position) {
            float coord = ThumbInput.ToLocalSpace(position).X;
            coord = Math.Clamp(coord, 25, DrawSize.X - Thumb.DrawSize.X + 5);
            Thumb.StopTransformSequences("Anchor", "Offset");

            value = (coord - 25) / (DrawSize.X - Thumb.DrawSize.X - 20);
            value = Math.Clamp(value, 0, 1);

            Thumb.AnchorTo(new Vector2(value, 0.5f), 0.3f, Ease.ExponentialOut);
            Thumb.OffsetTo(new Vector2(value, 0.5f), 0.3f, Ease.ExponentialOut);

            updateRoot();
        }

        void updateRoot(bool force = false) {
            if (force) {
                App.Root.StopTransformSequences(nameof(App.Root.Scale));
                App.Root.Scale = new Vector2(1 + value * 5);
                return;
            }

            App.Root.StopTransformSequences(nameof(App.Root.Scale));
            App.Root.ScaleTo(new Vector2(1 + value * 5), 0.5f, Ease.ExponentialOut);
        }

        DoAppChange += old => {
            App.DoResize += (w, h) => {
                updateRoot(true);
            };
        };

        ThumbInput.DoMouseDown += (button, position) => {
            if (button != MouseButton.Left) return;

            pressed = true;
            updatePos(position);

            Thumb.StopTransformSequences(nameof(Thumb.Scale), nameof(Thumb.Paint.Color));
            Thumb.ScaleTo(new Vector2(0.9f), 0.2f, Ease.ExponentialOut);
            Thumb.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);

            Background.StopTransformSequences();
            Background.ScaleTo(new Vector2(1.02f), 0.2f, Ease.ExponentialOut);
            Background.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);

            Bar.StopTransformSequences();
            Bar.MarginTo(new Vector2(3), 0.2f, Ease.ExponentialOut);

            ThumbContainer.StopTransformSequences();
            ThumbContainer.MarginTo(new Vector2(10, 0), 0.2f, Ease.ExponentialOut);
        };

        ThumbInput.DoMouseUp += (button, position) => {
            if (button != MouseButton.Left) return;

            pressed = false;

            Thumb.StopTransformSequences(nameof(Thumb.Scale), nameof(Thumb.Paint.Color));
            Thumb.ScaleTo(new Vector2(1), 0.2f, Ease.ExponentialOut);
            Thumb.ColorTo(new Color(200, 230, 255), 0.2f, Ease.ExponentialOut);

            Background.StopTransformSequences();
            Background.ScaleTo(new Vector2(1), 0.2f, Ease.ExponentialOut);
            Background.ColorTo(new Color(100, 110, 130), 0.2f, Ease.ExponentialOut);

            Bar.StopTransformSequences();
            Bar.MarginTo(new Vector2(5), 0.2f, Ease.ExponentialOut);

            ThumbContainer.StopTransformSequences();
            ThumbContainer.MarginTo(new Vector2(5, 0), 0.2f, Ease.ExponentialOut);
        };

        ThumbInput.DoMouseMove += position => {
            if (!pressed) return;

            updatePos(position);
        };
    }
}

public class TextBox : InputElement {
    private int _caretIndex;
    private Vector2 _clickPos;

    private DateTime _lastClick, _secondLastClick = DateTime.Now;
    private int _selectionEnd;

    private int _selectionStart;
    private Dictionary<int, float> _substringWidths = new();

    private Vector2 _targetPos;
    public RectElement Background, Caret, Selection;

    public int Limit = 1000;

    public TextElement Preview;
    public Element PreviewContent;

    public TextBox() {
        Children = [
            Background = new RectElement {
                ContentParent = this,
                RelativeSizeAxes = Axes.Both,
                Paint = new Paint {
                    Color = new Color(100, 110, 130),
                    IsAntialias = true,
                    FilterQuality = FilterQuality.Medium
                },
                RoundedCorners = new Vector2(10),
                Clip = true,
                Content = [
                    new Element {
                        RelativeSizeAxes = Axes.Both,
                        Position = new Vector2(0, -3),
                        Content = [
                            Preview = new TextElement {
                                ContentParent = this,
                                Text = "",
                                Margin = new Vector2(20, 0),
                                AnchorPosition = new Vector2(0, 0.5f),
                                OffsetPosition = new Vector2(0, 0.5f),
                                Index = 10,
                                Paint = new Paint {
                                    Color = Colors.White,
                                    IsAntialias = true,
                                    FilterQuality = FilterQuality.High,
                                    TextSize = 40
                                },
                                Content = [
                                    PreviewContent = new Element {
                                        RelativeSizeAxes = Axes.Y,
                                        Margin = new Vector2(0, -3),
                                        Position = new Vector2(0, 3),
                                        Content = [
                                            Caret = new RectElement {
                                                ContentParent = this,
                                                Size = new Vector2(2, 40),
                                                RelativeSizeAxes = Axes.Y,
                                                Paint = new Paint {
                                                    Color = Colors.WhiteTransparent,
                                                    IsAntialias = true,
                                                    FilterQuality = FilterQuality.Medium
                                                },
                                                AnchorPosition = new Vector2(0, 0.6f),
                                                OffsetPosition = new Vector2(0, 0.6f),
                                                Index = 10
                                            },
                                            Selection = new RectElement {
                                                ContentParent = this,
                                                RelativeSizeAxes = Axes.Y,
                                                Paint = new Paint {
                                                    Color = new Color(200, 230, 255, 100),
                                                    IsAntialias = true,
                                                    FilterQuality = FilterQuality.Medium
                                                },
                                                RoundedCorners = new Vector2(4),
                                                AnchorPosition = new Vector2(0, 0.5f),
                                                OffsetPosition = new Vector2(0, 0.5f),
                                                Index = 5
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        ];
    }

    public int CaretIndex {
        get => _caretIndex;
        set {
            _caretIndex = value;
            updateCaretPosition();
        }
    }

    public string Text {
        get => Preview.Text;
        set {
            Preview.Text = value;
            calculateSubstringWidths();
        }
    }

    public int SelectionStart {
        get => _selectionStart;
        set {
            _selectionStart = value;
            updateSelection();
        }
    }

    public int SelectionEnd {
        get => _selectionEnd;
        set {
            _selectionEnd = value;
            updateSelection();
        }
    }

    private void calculateSubstringWidths() {
        _substringWidths.Clear();
        float width = 0;

        Dictionary<char, float> charWidths = new();

        for (int i = 0; i < Text.Length; i++) {
            char c = Text[i];
            if (!charWidths.ContainsKey(c))
                charWidths.Add(c, Preview.Font?.MeasureText(c.ToString(), Preview.Paint.TextSize).X ?? 0);
            width += charWidths[c];
            _substringWidths.Add(i, width);
        }

        _substringWidths = _substringWidths.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
    }

    protected override void OnFocus() {
        if (App != null)
            App.Platform.KeyboardVisible = true;
        ShowCaret();
    }

    protected override void OnBlur() {
        if (App != null)
            App.Platform.KeyboardVisible = false;
        HideCaret();
        ClearSelection();
    }

    public void ShowCaret() {
        Caret.StopTransformSequences(nameof(Caret.Paint.Color));
        Caret.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);
    }

    public void HideCaret() {
        Caret.StopTransformSequences(nameof(Caret.Paint.Color));
        Caret.ColorTo(Colors.WhiteTransparent, 0.2f, Ease.ExponentialOut);
    }

    protected override bool OnMouseDown(MouseButton button, Vector2 position) {
        if (button != MouseButton.Left) return false;

        _clickPos = position;
        if (_substringWidths.Count == 0) return true;

        float relativeX = ToLocalSpace(position).X - Preview.Position.X;
        int index = 0;

        lock (_substringWidths)
            foreach ((int i, float width) in _substringWidths)
                if (relativeX >= width - Preview.Paint.TextSize / 4) {
                    index = i;
                    Console.WriteLine($"Index: {index} Width: {width} RelativeX: {relativeX}");
                    break;
                }

        CaretIndex = relativeX < 0 ? 0 : index + 1;
        SelectionStart = SelectionEnd = CaretIndex;

        if ((DateTime.Now - _secondLastClick).TotalMilliseconds < 600) {
            SelectionStart = 0;
            SelectionEnd = Text.Length;
            CaretIndex = Text.Length;
            _secondLastClick = DateTime.Now;
        } else if ((DateTime.Now - _lastClick).TotalMilliseconds < 300) {
            int start = CaretIndex;
            while (start > 0 && !char.IsWhiteSpace(Text[start - 1])) start--;
            int end = CaretIndex;
            while (end < Text.Length && !char.IsWhiteSpace(Text[end])) end++;
            SelectionStart = start;
            SelectionEnd = end;
            CaretIndex = end;
        }

        _secondLastClick = _lastClick;
        _lastClick = DateTime.Now;

        return true;
    }

    protected override void OnMouseMove(Vector2 position) {
        if (!MouseButtons[MouseButton.Left] || Vector2.Distance(_clickPos, position) < 5 && (DateTime.Now - _lastClick).TotalMilliseconds < 300) return;
        if (_substringWidths.Count == 0) return;

        float relativeX = ToLocalSpace(position).X - Preview.Position.X;
        int index = 0;

        for (int i = _substringWidths.Count - 1; i >= 0; i--)
            if (relativeX >= _substringWidths[i] - Preview.Paint.TextSize / 4) {
                index = i;
                break;
            }

        SelectionEnd = relativeX < 0 ? 0 : index + 1;
        CaretIndex = SelectionEnd;
    }

    private void updateCaretPosition(float animationDuration = 0.1f, bool resetPos = false) {
        calculateSubstringWidths();

        float caretX = Preview.Font?.MeasureText(Text[..CaretIndex], Preview.Paint.TextSize).X ?? 0;
        Caret.StopTransformSequences(nameof(Caret.Position));
        if (animationDuration > 0)
            Caret.MoveTo(new Vector2(caretX, 0), animationDuration, Ease.ExponentialOut);
        else
            Caret.Position = new Vector2(caretX, 0);

        if (Text.Length == 0 || resetPos) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(0, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
            return;
        }

        float width = Background.DrawSize.X - Preview.Margin.X * 2;
        float textWidth = Preview.DrawSize.X;

        float offset = _targetPos.X;

        float relativeX = caretX + offset;
        if (relativeX > width) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(-caretX + width, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
        } else if (relativeX < 0) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(-caretX, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
        }

        if (textWidth < width - Preview.Margin.X * 2) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(0, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
        }
    }

    public void ClearSelection() {
        _selectionStart = _selectionEnd = CaretIndex;
        updateSelection();
    }

    private void updateSelection() {
        if (SelectionStart == SelectionEnd) {
            Selection.StopTransformSequences(nameof(Selection.Size), nameof(Selection.Position));
            Selection.SizeTo(new Vector2(0, 20), 0.05f, Ease.ExponentialOut);
            float x = Preview.Font?.MeasureText(Text[..CaretIndex], Preview.Paint.TextSize).X ?? 0;
            Selection.MoveTo(new Vector2(x, 0), 0.05f, Ease.ExponentialOut);
            return;
        }

        int start = Math.Min(SelectionStart, SelectionEnd);
        int end = Math.Max(SelectionStart, SelectionEnd);

        if (start < 0 || end < 0 || start >= Text.Length + 1 || end >= Text.Length + 1) {
            Selection.StopTransformSequences(nameof(Selection.Size), nameof(Selection.Position));
            Selection.SizeTo(new Vector2(0, 20), 0.05f, Ease.ExponentialOut);
            return;
        }

        float selectionStartX = Preview.Font?.MeasureText((Text + " ")[..start], Preview.Paint.TextSize).X ?? 0;
        float selectionEndX = Preview.Font?.MeasureText((Text + " ")[..end], Preview.Paint.TextSize).X ?? 0;
        Selection.StopTransformSequences(nameof(Selection.Size), nameof(Selection.Position));
        Selection.SizeTo(new Vector2(selectionEndX - selectionStartX, 20), 0.05f, Ease.ExponentialOut);
        Selection.MoveTo(new Vector2(selectionStartX, 0), 0.05f, Ease.ExponentialOut);
    }

    private void flashCaret() {
        Caret.StopTransformSequences(nameof(Caret.Scale));
        Caret.StopTransformSequences(nameof(Caret.Rotation));
        Caret.ScaleTo(0.6f).Then().ScaleTo(1f, 0.5f, Ease.ExponentialOut);
        Caret.RotateTo(20).Then().RotateTo(0, 0.5f, Ease.ExponentialOut);
    }

    protected override void OnTextInput(string text) {
        if (SelectionStart != SelectionEnd) {
            Text = Text.Remove(Math.Min(SelectionStart, SelectionEnd), Math.Abs(SelectionStart - SelectionEnd));
            _caretIndex = Math.Min(SelectionStart, SelectionEnd);
        }

        if (Limit > 0 && Text.Length + text.Length > Limit)
            flashCaret();
        else {
            Text = Text.Insert(CaretIndex, text);
            CaretIndex += text.Length;
        }

        ClearSelection();
    }

    protected override bool OnKeyDown(Key key, KeyMods mods) {
        int originalCaretIndex = CaretIndex;
        switch (key) {
            case Key.Left:
                if (SelectionStart != SelectionEnd && !mods.HasFlag(KeyMods.Shift)) {
                    CaretIndex = Math.Min(SelectionStart, SelectionEnd);
                    ClearSelection();
                } else {
                    CaretIndex = Math.Max(0, CaretIndex - 1);
                    if (mods.HasFlag(KeyMods.Control)) {
                        while (CaretIndex > 0 && char.IsWhiteSpace(Text[CaretIndex - 1])) CaretIndex--;
                        while (CaretIndex > 0 && !char.IsWhiteSpace(Text[CaretIndex - 1])) CaretIndex--;
                    }

                    if (mods.HasFlag(KeyMods.Shift)) SelectionEnd = CaretIndex;
                    else ClearSelection();
                }

                break;
            case Key.Right:
                if (SelectionStart != SelectionEnd && !mods.HasFlag(KeyMods.Shift)) {
                    CaretIndex = Math.Max(SelectionStart, SelectionEnd);
                    ClearSelection();
                } else {
                    CaretIndex = Math.Min(Text.Length, CaretIndex + 1);
                    if (mods.HasFlag(KeyMods.Control)) {
                        while (CaretIndex < Text.Length && char.IsWhiteSpace(Text[CaretIndex])) CaretIndex++;
                        while (CaretIndex < Text.Length && !char.IsWhiteSpace(Text[CaretIndex])) CaretIndex++;
                    }

                    if (mods.HasFlag(KeyMods.Shift)) SelectionEnd = CaretIndex;
                    else ClearSelection();
                }

                break;
            case Key.Backspace:
                if (SelectionStart != SelectionEnd) {
                    Text = Text.Remove(Math.Min(SelectionStart, SelectionEnd), Math.Abs(SelectionStart - SelectionEnd));
                    CaretIndex = Math.Min(SelectionStart, SelectionEnd);
                    ClearSelection();
                } else {
                    if (CaretIndex > 0) {
                        if (mods.HasFlag(KeyMods.Control)) {
                            while (CaretIndex > 0 && char.IsWhiteSpace(Text[CaretIndex - 1])) {
                                Text = Text.Remove(CaretIndex - 1, 1);
                                CaretIndex--;
                            }

                            while (CaretIndex > 0 && !char.IsWhiteSpace(Text[CaretIndex - 1])) {
                                Text = Text.Remove(CaretIndex - 1, 1);
                                CaretIndex--;
                            }
                        } else {
                            Text = Text.Remove(CaretIndex - 1, 1);
                            CaretIndex--;
                        }
                    }
                }

                break;
            case Key.Delete:
                if (SelectionStart != SelectionEnd) {
                    Text = Text.Remove(Math.Min(SelectionStart, SelectionEnd), Math.Abs(SelectionStart - SelectionEnd));
                    CaretIndex = Math.Min(SelectionStart, SelectionEnd);
                    ClearSelection();
                } else {
                    if (CaretIndex < Text.Length) {
                        if (mods.HasFlag(KeyMods.Control)) {
                            while (CaretIndex < Text.Length && char.IsWhiteSpace(Text[CaretIndex])) Text = Text.Remove(CaretIndex, 1);
                            while (CaretIndex < Text.Length && !char.IsWhiteSpace(Text[CaretIndex])) Text = Text.Remove(CaretIndex, 1);
                        } else
                            Text = Text.Remove(CaretIndex, 1);
                    }
                }

                break;
            case Key.Home:
                CaretIndex = 0;
                if (mods.HasFlag(KeyMods.Shift)) SelectionEnd = CaretIndex;
                else ClearSelection();
                break;
            case Key.End:
                CaretIndex = Text.Length;
                if (mods.HasFlag(KeyMods.Shift)) SelectionEnd = CaretIndex;
                else ClearSelection();
                break;
            case Key.A:
                if (mods.HasFlag(KeyMods.Control)) {
                    SelectionStart = 0;
                    SelectionEnd = Text.Length;
                    CaretIndex = Text.Length;
                }

                break;
            case Key.C:
                if (mods.HasFlag(KeyMods.Control))
                    if (SelectionStart != SelectionEnd)
                        App.Platform.Clipboard = Text[Math.Min(SelectionStart, SelectionEnd)..Math.Max(SelectionStart, SelectionEnd)];

                break;
            case Key.V:
                if (mods.HasFlag(KeyMods.Control)) {
                    string clipboard = App.Platform.Clipboard;
                    if (SelectionStart != SelectionEnd) {
                        int start = Math.Min(SelectionStart, SelectionEnd);
                        int end = Math.Max(SelectionStart, SelectionEnd);
                        int len = end - start;

                        if (Text.Length - len + clipboard.Length > Limit) {
                            flashCaret();
                            break;
                        }

                        Text = Text.Remove(start, end);
                        CaretIndex = Math.Min(SelectionStart, SelectionEnd);
                    }

                    if (Text.Length + clipboard.Length > Limit) {
                        flashCaret();
                        break;
                    }

                    Text = Text.Insert(CaretIndex, clipboard);
                    CaretIndex += clipboard.Length;
                    ClearSelection();
                }

                break;
            default:
                return false;
        }

        return true;
    }
}

public class MyApp : Application {
    private TextElement? fpsText;

    protected override void OnLoad() {
        Add(fpsText = new TextElement {
            ContentParent = Root,
            Text = "FPS: 0",
            Position = new Vector2(20, 20),
            Index = 10
        });

        Slider slider = new() {
            RelativeSizeAxes = Axes.X,
            Margin = new Vector2(20, 0),
            Size = new Vector2(0, 50),
            AnchorPosition = new Vector2(0.5f, 1),
            OffsetPosition = new Vector2(0.5f, 1f),
            ContentParent = Root,
            Index = 3,
            Position = new Vector2(0, -20)
        };

        TextBox test;

        Add(test = new TextBox {
            RelativeSizeAxes = Axes.X,
            Margin = new Vector2(30, 0),
            Size = new Vector2(100),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            ContentParent = Root,
            Index = 3
        });

        test.Preview.Font = ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf");
    }

    protected override void OnRender() {
        base.OnRender();
        float fps = 1f / (float)RenderTime.DeltaTime;
        fpsText!.Text = $"FPS: {fps}";
    }
}

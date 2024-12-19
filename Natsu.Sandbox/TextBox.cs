using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class TextBox : InputElement {
    private int _caretIndex;
    private Vector2 _clickPos;

    private DateTime _lastClick, _secondLastClick = DateTime.Now;
    private int _selectionEnd;

    private int _selectionStart;
    private Dictionary<int, float> _substringWidths = new();

    private Vector2 _targetPos;
    public RectElement Background, Caret, Selection;

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
                                                Index = 10,
                                                RoundedCorners = new Vector2(1)
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
        if (App != null) {
            App.Platform.KeyboardVisible = true;
            App.Platform.TextCaret = new RangeI(CaretIndex, CaretIndex);
            App.Platform.TextInput = Text;
        }

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

    public bool Press(Vector2 position) {
        _clickPos = position;
        if (_substringWidths.Count == 0) return true;

        float relativeX = ToLocalSpace(position).X - Preview.Position.X;
        int index = 0;

        lock (_substringWidths)
            foreach ((int i, float width) in _substringWidths)
                if (relativeX >= width - Preview.Paint.TextSize / 4) {
                    index = i;
                    break;
                }

        if (relativeX < 0) index = 0;
        if (relativeX > _substringWidths.First().Value) index = Text.Length;

        CaretIndex = index;
        SelectionStart = SelectionEnd = CaretIndex;

        if ((DateTime.Now - _secondLastClick).TotalMilliseconds < 400) {
            SelectionStart = 0;
            SelectionEnd = Text.Length;
            CaretIndex = Text.Length;
            _secondLastClick = DateTime.Now;
        } else if ((DateTime.Now - _lastClick).TotalMilliseconds < 200) {
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

    protected override bool OnMouseDown(MouseButton button, Vector2 position) {
        if (button != MouseButton.Left) return false;

        return Press(position);
    }

    protected override bool OnTouchDown(int id, Vector2 position) => Press(position);

    public void TouchSelection(Vector2 position) {
        if (_substringWidths.Count == 0) return;

        float relativeX = ToLocalSpace(position).X - Preview.Position.X;
        int index = 0;

        for (int i = _substringWidths.Count - 1; i >= 0; i--)
            if (relativeX >= _substringWidths[i] - Preview.Paint.TextSize / 4) {
                index = i;
                break;
            }

        if (relativeX < 0) index = 0;
        if (relativeX > _substringWidths.First().Value) index = Text.Length;

        SelectionEnd = index;
        CaretIndex = SelectionEnd;
    }

    protected override void OnMouseMove(Vector2 position) {
        if (!MouseButtons[MouseButton.Left] || Vector2.Distance(_clickPos, position) < 5 && (DateTime.Now - _lastClick).TotalMilliseconds < 300) return;

        TouchSelection(position);
    }

    protected override void OnTouchMove(int id, Vector2 position) {
        if (id != 0) return;

        TouchSelection(position);
    }

    private void updateCaretPosition(float animationDuration = 0.15f, bool resetPos = false) {
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
        if (textWidth < width - Preview.Margin.X * 2) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(0, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
        } else if (relativeX > width) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(-caretX + width, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
        } else if (relativeX < 0) {
            Preview.StopTransformSequences(nameof(Preview.Position));
            _targetPos = new Vector2(-caretX, 0);
            Preview.MoveTo(_targetPos, animationDuration, Ease.ExponentialOut);
        }
    }

    public void ClearSelection() {
        _selectionStart = _selectionEnd = CaretIndex;
        updateSelection();
    }

    private void updateSelection() {
        if (Platform != null)
            Platform.TextCaret = new RangeI(_selectionStart, _selectionEnd);

        if (SelectionStart == SelectionEnd) {
            Selection.StopTransformSequences(nameof(Selection.Size), nameof(Selection.Position));
            Selection.SizeTo(new Vector2(0, 20), 0.15f, Ease.ExponentialOut);
            float x = Preview.Font?.MeasureText(Text[..CaretIndex], Preview.Paint.TextSize).X ?? 0;
            Selection.MoveTo(new Vector2(x, 0), 0.15f, Ease.ExponentialOut);
            return;
        }

        int start = Math.Min(SelectionStart, SelectionEnd);
        int end = Math.Max(SelectionStart, SelectionEnd);

        if (start < 0 || end < 0 || start >= Text.Length + 1 || end >= Text.Length + 1) {
            Selection.StopTransformSequences(nameof(Selection.Size), nameof(Selection.Position));
            Selection.SizeTo(new Vector2(0, 20), 0.15f, Ease.ExponentialOut);
            return;
        }

        float selectionStartX = Preview.Font?.MeasureText((Text + " ")[..start], Preview.Paint.TextSize).X ?? 0;
        float selectionEndX = Preview.Font?.MeasureText((Text + " ")[..end], Preview.Paint.TextSize).X ?? 0;
        Selection.StopTransformSequences(nameof(Selection.Size), nameof(Selection.Position));
        Selection.SizeTo(new Vector2(selectionEndX - selectionStartX, 20), 0.15f, Ease.ExponentialOut);
        Selection.MoveTo(new Vector2(selectionStartX, 0), 0.15f, Ease.ExponentialOut);
    }

    protected override void OnTextInput(string text, int pos, int removed) {
        if (removed > 0)
            Text = Text.Remove(pos, removed);

        Text = Text.Insert(pos, text);
        CaretIndex = pos + text.Length;
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

                        Platform.TextInput = Text;
                        ClearSelection();
                    }
                }

                break;
            case Key.Delete:
                if (SelectionStart != SelectionEnd) {
                    Text = Text.Remove(Math.Min(SelectionStart, SelectionEnd), Math.Abs(SelectionStart - SelectionEnd));
                    CaretIndex = Math.Min(SelectionStart, SelectionEnd);
                    ClearSelection();
                    Platform.TextInput = Text;
                } else {
                    if (CaretIndex < Text.Length) {
                        if (mods.HasFlag(KeyMods.Control)) {
                            while (CaretIndex < Text.Length && char.IsWhiteSpace(Text[CaretIndex])) Text = Text.Remove(CaretIndex, 1);
                            while (CaretIndex < Text.Length && !char.IsWhiteSpace(Text[CaretIndex])) Text = Text.Remove(CaretIndex, 1);
                        } else
                            Text = Text.Remove(CaretIndex, 1);

                        ClearSelection();
                        Platform.TextInput = Text;
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

                        Text = Text.Remove(start, end);
                        CaretIndex = Math.Min(SelectionStart, SelectionEnd);
                    }

                    Text = Text.Insert(CaretIndex, clipboard);
                    CaretIndex += clipboard.Length;
                    Platform.TextInput = Text;
                    ClearSelection();
                }

                break;
            default:
                return false;
        }

        return true;
    }
}

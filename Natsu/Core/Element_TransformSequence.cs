using Natsu.Mathematics.Transforms;

namespace Natsu.Core;

public partial class Element {
    public List<TransformSequence> TransformSequences { get; } = new();

    /// <summary>
    ///     Updates all transform sequences.
    /// </summary>
    public void UpdateTransformSequences() {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                TransformSequence? sequence = TransformSequences[i];
                if (sequence == null) continue;

                if (sequence.IsComplete || sequence.Stopped) {
                    TransformSequences.RemoveAt(i);
                    i--;
                    continue;
                }

                if (App != null)
                    sequence.Update(App.UpdateTime.DeltaTime);
                else
                    sequence.Update();
            }
        }
    }

    /// <summary>
    ///     Adds a new transform sequence to the element.
    /// </summary>
    /// <param name="sequence">The transform sequence to add</param>
    public void AddTransformSequence(TransformSequence sequence) {
        lock (TransformSequences) {
            TransformSequences.Add(sequence);
        }
    }

    /// <summary>
    ///     Stops all transform sequences of the element.
    /// </summary>
    public void StopTransformSequences() {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                TransformSequence? sequence = TransformSequences[i];
                if (sequence == null) continue;

                TransformSequences.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    ///     Stops all transform sequences of the element with the specified properties.
    /// </summary>
    /// <param name="properties">The properties to stop</param>
    public void StopTransformSequences(params string[] properties) {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                TransformSequence? sequence = TransformSequences[i];
                if (sequence == null || !properties.Contains(sequence.Name)) continue;

                TransformSequences.RemoveAt(i);
                i--;
            }
        }
    }
}

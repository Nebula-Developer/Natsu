using Natsu.Mathematics.Transforms;

namespace Natsu.Core;

public partial class Element {
    public List<ITransformSequence> TransformSequences { get; } = new();

    /// <summary>
    ///     Adds a new transform sequence to the element.
    /// </summary>
    /// <param name="sequence">The transform sequence to add</param>
    public void AddTransformSequence(ITransformSequence sequence) {
        lock (TransformSequences) {
            TransformSequences.Add(sequence);
        }
    }

    /// <summary>
    ///     Updates all transform sequences.
    /// </summary>
    public void UpdateTransformSequences() {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                ITransformSequence? sequence = TransformSequences[i];
                if (sequence == null) continue;

                if (sequence.IsCompleted) {
                    TransformSequences.RemoveAt(i);
                    i--;
                    continue;
                }

                if (App != null) sequence.Update((float)App.Time.DeltaTime);
            }
        }
    }

    /// <summary>
    ///     Stops all transform sequences of the element.
    /// </summary>
    public void StopTransformSequences() {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                ITransformSequence? sequence = TransformSequences[i];
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
                ITransformSequence? sequence = TransformSequences[i];
                if (sequence == null || !properties.Contains(sequence.Name)) continue;

                TransformSequences.RemoveAt(i);
                i--;
            }
        }
    }
}

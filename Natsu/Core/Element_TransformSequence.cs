using Natsu.Mathematics.Transforms;

namespace Natsu.Core;

public partial class Element {
    public List<ITransformSequence> TransformSequences { get; } = new();

    public void AddTransformSequence(ITransformSequence sequence) {
        lock (TransformSequences) {
            TransformSequences.Add(sequence);
        }
    }

    public void StopTransformSequence(ITransformSequence sequence) {
        lock (TransformSequences) {
            TransformSequences.Remove(sequence);
        }
    }

    public void UpdateTransformSequences(double time) {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                ITransformSequence? sequence = TransformSequences[i];
                if (sequence == null) continue;

                if (sequence.IsCompleted) {
                    TransformSequences.RemoveAt(i);
                    i--;
                    continue;
                }

                sequence.Update((float)time);
            }
        }
    }

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

    public void StopTransformSequence(string name) {
        lock (TransformSequences) {
            for (int i = 0; i < TransformSequences.Count; i++) {
                ITransformSequence? sequence = TransformSequences[i];
                if (sequence == null || sequence.Name != name) continue;

                TransformSequences.RemoveAt(i);
                i--;
            }
        }
    }
}

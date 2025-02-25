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

    public void StopTransformSequences() {
        lock (TransformSequences) {
            TransformSequences.Clear();
        }
    }

    public void StopTransformSequences(params string[] properties) {
        lock (TransformSequences) {
            TransformSequences.RemoveAll(s => properties.Contains(s.Name));
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
}

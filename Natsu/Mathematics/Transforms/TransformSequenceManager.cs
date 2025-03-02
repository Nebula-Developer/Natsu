namespace Natsu.Mathematics.Transforms;

public class TransformSequenceManager : ITransformable {
    public List<ITransformSequence> Sequences { get; } = new();

    public void AddTransformSequence(ITransformSequence sequence) {
        lock (Sequences) {
            Sequences.Add(sequence);
        }
    }

    public void StopTransformSequences() {
        lock (Sequences) {
            Sequences.Clear();
        }
    }

    public void StopTransformSequences(params string[] sequences) {
        lock (Sequences) {
            Sequences.RemoveAll(sequence => sequences.Contains(sequence.Name));
        }
    }

    public void StopTransformSequence(string name) {
        lock (Sequences) {
            Sequences.RemoveAll(sequence => sequence.Name == name);
        }
    }

    public void StopTransformSequence(ITransformSequence sequence) {
        lock (Sequences) {
            Sequences.Remove(sequence);
        }
    }

    public void UpdateTransformSequences(double time) {
        lock (Sequences) {
            for (int i = 0; i < Sequences.Count; i++) {
                ITransformSequence? sequence = Sequences[i];
                if (sequence == null) continue;

                if (sequence.IsCompleted) {
                    Sequences.RemoveAt(i);
                    i--;
                    continue;
                }

                sequence.Update((float)time);
            }
        }
    }
}

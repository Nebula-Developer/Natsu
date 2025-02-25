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

    public void StopTransformSequence(ITransformSequence sequence) {
        lock (Sequences) {
            Sequences.Remove(sequence);
        }
    }

    public void UpdateTransformSequences(double time) {
        lock (Sequences) {
            Sequences.ForEach(sequence => sequence.Update((float)time));
        }
    }
}

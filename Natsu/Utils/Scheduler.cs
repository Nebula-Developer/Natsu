using Natsu.Mathematics;

public class ScheduledTask {
    public required double BaseTime { get; set; }
    public double EndTime => BaseTime + Duration;
    public required double Duration { get; set; }
    public required Action Task { get; set; }
    public required CancellationTokenSource CancellationTokenSource { get; set; }
    public bool Loop { get; set; }

    public void Cancel() => CancellationTokenSource.Cancel();
    public void Reschedule(double baseTime) => BaseTime = baseTime;
}

public class Scheduler {
    public Clock TimeKeeper { get; } = new();
    public List<ScheduledTask> Tasks { get; } = new();

    public void Update(double time) {
        TimeKeeper.Update(time);

        for (int i = 0; i < Tasks.Count; i++) {
            if (Tasks[i].CancellationTokenSource.IsCancellationRequested) {
                Tasks.RemoveAt(i);
                i--;
                continue;
            }

            if (Tasks[i].EndTime <= TimeKeeper.Time) {
                Tasks[i].Task();
                if (Tasks[i].Loop) {
                    Tasks[i].Reschedule(Tasks[i].EndTime);
                } else {
                    Tasks.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public ScheduledTask Schedule(double time, Action task, bool loop = false) {
        CancellationTokenSource? cancellationTokenSource = new();
        ScheduledTask? scheduledTask = new() {
            BaseTime = TimeKeeper.Time,
            Duration = time,
            Task = task,
            CancellationTokenSource = cancellationTokenSource,
            Loop = loop
        };
        Tasks.Add(scheduledTask);
        return scheduledTask;
    }
}

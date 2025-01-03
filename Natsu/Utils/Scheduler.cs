using Natsu.Mathematics;

public class ScheduledTask {
    public required double BaseTime { get; set; }
    public required double Time { get; set; }
    public required Action Task { get; set; }
    public required CancellationTokenSource CancellationTokenSource { get; set; }

    public void Cancel() => CancellationTokenSource.Cancel();
    public void Reschedule(double time) => Time = BaseTime + time;
}

public class Scheduler {
    public Clock TimeKeeper { get; } = new();
    public List<ScheduledTask> Tasks { get; } = new();

    public void Update(double time) {
        TimeKeeper.Update(time);

        for (int i = 0; i < Tasks.Count; i++)
            if (Tasks[i].Time <= TimeKeeper.Time && !Tasks[i].CancellationTokenSource.Token.IsCancellationRequested) {
                Tasks[i].Task();
                Tasks.RemoveAt(i);
                i--;
            }
    }

    public ScheduledTask Schedule(double time, Action task) {
        CancellationTokenSource? cancellationTokenSource = new();
        ScheduledTask? scheduledTask = new() {
            BaseTime = TimeKeeper.Time,
            Time = TimeKeeper.Time + time,
            Task = task,
            CancellationTokenSource = cancellationTokenSource
        };
        Tasks.Add(scheduledTask);
        return scheduledTask;
    }
}

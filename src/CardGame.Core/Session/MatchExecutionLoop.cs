namespace CardGame.Core;

internal interface IMatchExecutionHost
{
    EngineStatus Status { get; }
    Winner Winner { get; }
    bool HasFrames { get; }
    bool HasPendingDecision { get; }
    bool HasHumanDecision { get; }
    bool HasUnfinishedAutomaticDecision { get; }
    void EnsureStarted();
    void RunStep();
    void CompleteGame();
    void EndAsDraw(string reason);
}

/// <summary>Drives the synchronous match to the next externally observable boundary.</summary>
internal sealed class MatchExecutionLoop(IMatchExecutionHost host)
{
    internal void AdvanceToHumanBoundary()
    {
        host.EnsureStarted();
        if (host.Status == EngineStatus.Completed || host.HasHumanDecision) return;

        var guard = 0;
        while (host.Status != EngineStatus.Completed && !host.HasHumanDecision)
        {
            if (host.Winner != Winner.None && !host.HasFrames)
            {
                host.CompleteGame();
                break;
            }
            if (++guard > 20_000)
            {
                // Finish an in-flight automatic decision before applying the
                // safety limit again at its next stable boundary.
                if (host.HasUnfinishedAutomaticDecision)
                {
                    host.RunStep();
                    continue;
                }
                host.EndAsDraw("规则循环超过安全上限");
                break;
            }
            host.RunStep();
        }
    }

    internal void AdvanceOneStep()
    {
        host.EnsureStarted();
        if (host.Status != EngineStatus.Completed && host.Winner != Winner.None && !host.HasFrames)
            host.CompleteGame();
        else if (host.Status != EngineStatus.Completed && !host.HasHumanDecision)
        {
            host.RunStep();
            if (host.Winner != Winner.None && host.Status != EngineStatus.Completed &&
                !host.HasPendingDecision && !host.HasFrames)
                host.CompleteGame();
        }
    }
}

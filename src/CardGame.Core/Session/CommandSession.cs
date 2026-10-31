namespace CardGame.Core;

/// <summary>
/// Owns one match's command journal, exclusive execution gate and trusted
/// recovery prefix. Rule state, event storage and output are explicit ports.
/// </summary>
internal sealed class CommandSession
{
    private readonly List<GameCommand> _acceptedCommands = [];
    internal long Revision { get; private set; }
    internal IReadOnlyList<GameCommand> AcceptedCommands => _acceptedCommands.AsReadOnly();
    internal GameCommand? CommandAwaitingCommit { get; set; }
    internal bool IsExecuting { get; private set; }
    internal Exception? FatalRuleFailure { get; private set; }
    internal bool IsFaulted => FatalRuleFailure is not null;
    internal GameCheckpoint? LastTrustedCheckpoint { get; private set; }
    internal EngineRunResult? ResultBeingDelivered { get; private set; }

    internal void EnsureHealthy()
    {
        if (FatalRuleFailure is not null)
            throw new InvalidOperationException(
                "The match stopped after an internal rules failure. Restore its last trusted checkpoint before continuing.",
                FatalRuleFailure);
    }

    internal GameCheckpoint CreateCheckpoint(ICommandCheckpointSource source)
    {
        EnsureHealthy();
        if (Revision != _acceptedCommands.Count)
            throw new InvalidOperationException(
                "Checkpoint capture requires a state driven entirely through Submit; " +
                "legacy adapter calls are not represented in the command journal.");
        return BuildTrustedCheckpoint(source, Revision);
    }

    internal GameCheckpoint BuildTrustedCheckpoint(ICommandCheckpointSource source, long revision) =>
        new(GameCheckpoint.CurrentSchemaVersion, source.Options,
            Array.AsReadOnly(_acceptedCommands.ToArray()), revision, source.ModeId,
            source.ContentPackages, source.ContentHash);

    // Projection and delivery are operation dependencies. Neither callbacks nor
    // runtime adapters are retained in the session or its recovery data.
    internal EngineRunResult Execute(ICommandSessionRuntime runtime, CommandEventJournal journal,
        Action operation, Func<GameSnapshot> project, Action<GameSnapshot> deliver)
    {
        EnsureHealthy();
        if (IsExecuting)
            throw new InvalidOperationException(
                "GameEngine cannot be advanced reentrantly from a synchronous event handler.");

        IsExecuting = true;
        var previousRevision = Revision;
        var previousCommandCount = _acceptedCommands.Count;
        var hadTrustedPrefix = previousRevision == previousCommandCount;
        var committed = false;
        try
        {
            operation();
            runtime.SynchronizeDerivedState();
            runtime.AssertInvariants();

            // Every prepared output uses the next revision before it becomes
            // observable. A failed preparation restores the preceding prefix.
            var nextRevision = checked(previousRevision + 1);
            Revision = nextRevision;
            runtime.RefreshDecisionRevision();
            var snapshot = project();
            var outcome = runtime.Outcome;
            var result = new EngineRunResult(outcome.Status, outcome.Winner, snapshot,
                snapshot.PendingDecision, nextRevision)
            {
                WinnerSeat = outcome.WinnerSeat,
                WinnerTeamId = outcome.WinnerTeamId,
                WinnerFactionId = outcome.WinnerFactionId
            };
            var preparedEvents = journal.Prepare(nextRevision, runtime.EventSequence,
                runtime.PreparePublicEvents);

            if (CommandAwaitingCommit is not null)
                _acceptedCommands.EnsureCapacity(checked(previousCommandCount + 1));
            journal.Reserve(preparedEvents);

            // No rule execution, projection, user callback or allocation runs
            // between these appends and the committed marker.
            if (CommandAwaitingCommit is { } command) _acceptedCommands.Add(command);
            var lastSequence = journal.Commit(preparedEvents);
            if (lastSequence is { } sequence) runtime.EventSequence = sequence;
            runtime.ClearPendingEvents();
            committed = true;

            ResultBeingDelivered = result;
            deliver(snapshot);
            return result;
        }
        catch (Exception failure)
        {
            if (!committed)
            {
                Revision = previousRevision;
                if (_acceptedCommands.Count > previousCommandCount)
                    _acceptedCommands.RemoveRange(previousCommandCount,
                        _acceptedCommands.Count - previousCommandCount);
                LastTrustedCheckpoint = null;
                if (hadTrustedPrefix)
                {
                    try { LastTrustedCheckpoint = BuildTrustedCheckpoint(runtime, previousRevision); }
                    catch { /* Preserve the original execution/preparation failure. */ }
                }
                FatalRuleFailure ??= failure;
                runtime.MarkFaulted();
            }
            // Post-commit delivery failure preserves the visible revision,
            // command and event journal, and releases the execution gate.
            journal.ClearNotifications();
            runtime.ClearPendingEvents();
            runtime.ClearPendingPublication();
            throw;
        }
        finally
        {
            ResultBeingDelivered = null;
            IsExecuting = false;
        }
    }
}

internal interface ICommandCheckpointSource
{
    GameOptions Options { get; }
    string ModeId { get; }
    IReadOnlyList<string> ContentPackages { get; }
    string ContentHash { get; }
}

internal readonly record struct CommandOutcome(EngineStatus Status, Winner Winner,
    int? WinnerSeat, string? WinnerTeamId, string? WinnerFactionId);

/// <summary>Rule preparation and storage primitives used by a command transaction.</summary>
internal interface ICommandSessionRuntime : ICommandCheckpointSource
{
    CommandOutcome Outcome { get; }
    long EventSequence { get; set; }
    void SynchronizeDerivedState();
    void AssertInvariants();
    void RefreshDecisionRevision();
    EventNotification?[] PreparePublicEvents(EventNotification[] events);
    void ClearPendingEvents();
    void ClearPendingPublication();
    void MarkFaulted();
}

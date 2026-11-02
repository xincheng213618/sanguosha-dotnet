namespace CardGame.Core;

internal sealed partial class ProgramEventHistoryIndex
{
    private readonly Dictionary<(int Turn, int Owner),
        List<(long Cursor, int Actor, ActualDiscardRecoveryPhaseKind Kind, ActualPhaseHistoryOutcome Outcome)>> _actualPhaseHistory = [];

    public int CountPhases(int actualTurn, int turnOwner, int actor, ActualDiscardRecoveryPhaseKind? kind,
        ActualPhaseHistoryOutcome outcome, long atCursor)
    {
        SynchronizeNativeHistory();
        return _actualPhaseHistory.TryGetValue((actualTurn, turnOwner), out var facts)
            ? facts.TakeWhile(fact => fact.Cursor <= atCursor).Count(fact => fact.Actor == actor &&
                (kind is null || fact.Kind == kind) && fact.Outcome == outcome) : 0;
    }

    private void IndexActualPhaseFact(IGameEvent payload, long cursor)
    {
        int turn, owner, actor; ActualDiscardRecoveryPhaseKind kind; ActualPhaseHistoryOutcome outcome;
        switch (payload)
        {
            case ActualDiscardRecoveryPhaseStartedEvent { Phase: var phase }:
                (turn, owner, actor, kind, outcome) = (phase.TurnNumber, phase.ActualTurnOwnerSeat,
                    phase.ActorSeat, phase.Kind, ActualPhaseHistoryOutcome.Entered);
                break;
            case ActualPhaseSkippedEvent skipped:
                (turn, owner, actor, kind, outcome) = (skipped.TurnNumber, skipped.ActualTurnOwnerSeat,
                    skipped.ActorSeat, skipped.Kind, ActualPhaseHistoryOutcome.Skipped);
                break;
            default: return;
        }
        if (!_actualPhaseHistory.TryGetValue((turn, owner), out var facts)) _actualPhaseHistory[(turn, owner)] = facts = [];
        facts.Add((cursor, actor, kind, outcome));
    }
}

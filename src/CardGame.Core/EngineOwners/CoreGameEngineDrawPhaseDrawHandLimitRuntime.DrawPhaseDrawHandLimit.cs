#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawPhaseDrawHandLimitRuntime
{
    internal int CurrentTurnDrawPhaseDrawCount(int ownerSeat)
    {
        if (_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber < 1)
            return 0;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var turnStartIndex = Array.FindLastIndex(history, fact => fact is TurnStartedEvent);
        if (turnStartIndex < 0)
            return 0;
        var turnStart = (TurnStartedEvent)history[turnStartIndex];
        var turnHistory = history[(turnStartIndex + 1)..];
        // A completed turn's cursor can already point to the next actor while
        // the old number remains. Its Ended fact wins before any owner check.
        if (turnHistory.OfType<TurnEndedEvent>().Any(fact => fact.TurnNumber == turnStart.TurnNumber && fact.ActorSeat == turnStart.ActorSeat))
            return 0;
        if (turnStart.TurnNumber != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || turnStart.ActorSeat != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            throw new InvalidOperationException("A draw-phase hand-limit query lost its active actual turn.");
        var draws = turnHistory.OfType<NativeDrawInvocationRecordedEvent>().Where(fact => fact.OwnerSeat == ownerSeat && fact.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && fact.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase && phase.ActorSeat == ownerSeat).ToArray();
        if (draws.Length == 0)
            return 0;
        var phases = turnHistory.OfType<ActualDiscardRecoveryPhaseStartedEvent>().Where(fact => fact.Phase.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && fact.Phase.ActorSeat == ownerSeat && fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).GroupBy(fact => fact.Phase.Token).ToDictionary(group => group.Key, group => group.ToArray());
        if (draws.Select(fact => fact.InvocationId).Distinct().Count() != draws.Length)
            throw new InvalidOperationException("A draw-phase hand-limit query repeated a native draw invoice.");
        var total = 0;
        foreach (var draw in draws)
        {
            var phase = draw.Phase!;
            if (draw.InvocationId <= 0 || draw.ActualCount < 1 || draw.Requested < draw.ActualCount || draw.Reason == CardMoveReasons.InitialDeal || draw.PhaseActorSeat != ownerSeat || phase.Token <= 0 || draw.ActualTurnOwnerSeat != turnStart.ActorSeat || phase.TurnNumber != draw.TurnNumber || phase.ActualTurnOwnerSeat != draw.ActualTurnOwnerSeat || !phases.TryGetValue(phase.Token, out var starts) || starts is not [var start] || start.Phase != phase)
                throw new InvalidOperationException("A draw-phase hand-limit query lost its exact actual-phase draw invoice.");
            total = checked(total + draw.ActualCount);
        }

        return total;
    }
}

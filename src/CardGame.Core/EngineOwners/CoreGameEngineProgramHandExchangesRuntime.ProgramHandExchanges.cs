#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramHandExchangesRuntime
{
    internal SkillProgramStepOutcome ExchangeProgramSelectedTargetHands(ProgramSkillFrame frame) => ExchangeProgramSelectedTargetHands(frame, _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).SelectedTargetSeats);
    internal SkillProgramStepOutcome ExchangeProgramSelectedTargetHands(ProgramSkillFrame frame, IReadOnlyList<int> seats)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SkillId != frame.SkillId || active.SkillInstanceId != frame.SkillInstanceId || active.OwnerSeat != frame.OwnerSeat || active.GameplayHash != frame.GameplayHash || seats.Count != 2 || seats[0] == seats[1] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seats[0]) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seats[1]))
            throw new InvalidOperationException("A hand exchange requires two distinct current program targets.");
        var first = _coreMatchParticipantsPlayersAccess._players[seats[0]];
        var second = _coreMatchParticipantsPlayersAccess._players[seats[1]];
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(first.Seat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(second.Seat) || !first.IsAlive || !second.IsAlive)
            throw new InvalidOperationException("A hand exchange requires two living program targets.");
        var firstHand = _corePhysicalOwnedCardQueriesOperations.GetHand(first).OrderBy(card => card.Id).ToArray();
        var secondHand = _corePhysicalOwnedCardQueriesOperations.GetHand(second).OrderBy(card => card.Id).ToArray();
        if (firstHand.Length == 0 && secondHand.Length == 0)
        {
            _coreHalfHandPhaseDebtProgramsOperations.FinalizeDeferredHandExchange(frame.Id);
            return SkillProgramStepOutcome.Continue;
        }

        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.exchange");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new ProgramMovementContinuation(frame.OwnerSeat, 0, null) });
        foreach (var card in firstHand)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(first.Seat), CardLocation.Processing, reason);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.Hand(second.Seat), reason);
        }

        foreach (var card in secondHand)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(second.Seat), CardLocation.Processing, reason);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.Hand(first.Seat), reason);
        }

        _coreHalfHandPhaseDebtProgramsOperations.FinalizeDeferredHandExchange(frame.Id);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}

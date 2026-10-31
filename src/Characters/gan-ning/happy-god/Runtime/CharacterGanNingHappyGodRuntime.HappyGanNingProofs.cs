#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGanNingHappyGodRuntime
{
    internal bool ValidHappyPoxi(ProgramSkillFrame frame)
    {
        if (frame.HappyPoxi is not { } receipt)
            return false;
        if (frame.SkillId != "happy:poxi" || frame.ActivationId != "three-suit-discard" || frame.TriggerId is not null || frame.InstructionIndex != 2 || frame.WindowContext is not null || frame.SelectedTargetSeats is not [var target] || target != receipt.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == frame.OwnerSeat || receipt.Source != new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || receipt.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !Enum.IsDefined(receipt.Stage) || receipt.OwnCount is < 0 or > 3 || receipt.BatchId <= 0 || receipt.Before < 0 || receipt.After <= receipt.Before || receipt.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || receipt.Cost.Count != 3 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.ActivationId == frame.ActivationId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyPoxiPaidEvent>().Count(e => e == new HappyPoxiPaidEvent(frame.Id, receipt.Source, receipt.GameplayHash, target, receipt.OwnCount, receipt.ActualTurn, receipt.BatchId, receipt.Before, receipt.After)) != 1)
            return false;
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, "three-suit-hands");
        if (!binding.CardIds.SequenceEqual(receipt.Cost.Select(c => c.CardId)) || !binding.SourceLocations.SequenceEqual(receipt.Cost.Select(c => c.From)) || binding.CardIds.Distinct().Count() != 3 || receipt.Cost.Count(c => c.From == CardLocation.Hand(frame.OwnerSeat)) != receipt.OwnCount || receipt.Cost.Any(c => c.From.Zone != CardZoneKind.Hand || c.From.OwnerSeat != frame.OwnerSeat && c.From.OwnerSeat != target || c.From == c.To || c.Sequence <= receipt.Before || c.Sequence > receipt.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(actual => actual.Sequence == c.Sequence && actual.CardId == c.CardId && actual.From == c.From && actual.To == c.To && actual.Reason.Value == HappyPoxiReason) != 1))
            return false;
        if (receipt.Stage == HappyPoxiStage.MaximumChildren)
            return receipt.OwnCount == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyPoxiBenefitIssuedEvent>().Count(e => e == new HappyPoxiBenefitIssuedEvent(frame.Id, 0, receipt.MaximumBefore, receipt.MaximumAfter)) == 1;
        if (receipt.Stage == HappyPoxiStage.RecoveryChildren && receipt.OwnCount != 2 || receipt.Stage is HappyPoxiStage.Drawing or HappyPoxiStage.DrawChildren && receipt.OwnCount is not (2 or 3))
            return false;
        if (receipt.Stage is HappyPoxiStage.RecoveryChildren or HappyPoxiStage.Drawing or HappyPoxiStage.DrawChildren && receipt.OwnCount == 2)
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<HappyPoxiBenefitIssuedEvent>().Count(e => e.FrameId == frame.Id && e.OwnCount == 2) != 1)
                return false;
        return receipt.Stage != HappyPoxiStage.DrawChildren || receipt.DrawBefore >= receipt.After && receipt.DrawAfter >= receipt.DrawBefore && receipt.DrawAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyPoxiDrawReturnedEvent>().Count(e => e == new HappyPoxiDrawReturnedEvent(frame.Id, receipt.OwnCount == 2 ? 1 : 3, receipt.DrawCount, receipt.DrawBefore, receipt.DrawAfter)) == 1;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 41680)]
    internal bool HappyPoxiMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.ResolveHappyGanNingPoxi && pending == frame.PendingMovementContinuation && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && frame.HappyPoxi is { Stage: HappyPoxiStage.DiscardChildren or HappyPoxiStage.MaximumChildren or HappyPoxiStage.RecoveryChildren or HappyPoxiStage.DrawChildren } && ValidHappyPoxi(frame);
}

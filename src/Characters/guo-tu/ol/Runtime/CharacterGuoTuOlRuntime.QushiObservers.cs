#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    internal bool ValidQushi(ProgramSkillFrame f)
    {
        if (f.Qushi is not { } r || r.Source != QushiSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<QushiStartedEvent>().Count(e => e == new QushiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActualTurn, r.TurnOwner, r.ParentId)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveQushiDrawAndPrivateSeal)
        {
            if (f.TriggerId is not null || f.ActivationId != "draw-seal" || f.WindowContext is not null)
                return false;
        }
        else
        {
            if (f.WindowContext is not { } context || r.Seal is not { } seal || CurrentQushiEndingSeal(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context) != seal)
                return false;
        }

        if (!r.Paid)
            return false;
        var invoice = _coreProgramEventHistoryOperations.ProgramEventHistory<QushiMovementPaidEvent>().Where(e => e.FrameId == f.Id && e.Step == r.Step).ToArray();
        if (invoice is not [var m] || m.Before != r.Before || m.After != r.After || m.Count < 0)
            return false;
        if (f.PendingMovementContinuation is { } p && !IsQushiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return false;
        if (r.Stage == QushiStage.SealChildren && (r.Seal is not { } deposited || deposited.SealId != f.Id || !ValidQushiSeal(deposited) || m.Stage != r.Stage || m.Count != 1))
            return false;
        if (r.Stage is QushiStage.RemoveChildren or QushiStage.RewardChildren)
        {
            if (r.Seal is not { } s || _coreProgramEventHistoryOperations.ProgramEventHistory<QushiSealRemovedEvent>().Count(e => e.FrameId == f.Id && e.SealId == s.SealId && e.HolderSeat == s.HolderSeat && e.CardId == s.CardId && !e.HolderDeath && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(v => v.Sequence == e.Sequence && v.CardId == s.CardId && v.From == s.Location && v.To == CardLocation.DiscardPile && v.Reason.Value == QushiRemoveReason)) != 1)
                return false;
        }

        return true;
    }
    internal bool IsQushiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.Qushi is { Paid: true } r && GuoTuQushiComposition.Handles(r.Op) && e?.Op == r.Op && p == f.PendingMovementContinuation && p.BeforeCount == 0 && p.CoverageResultBind is null && p.SubjectSeat == (r.Stage is QushiStage.DrawChildren or QushiStage.RewardChildren ? f.OwnerSeat : r.HolderSeat);
    internal bool QushiMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.Qushi!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage is QushiStage.DrawChildren or QushiStage.RewardChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == QushiDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        return r.Seal is { } s && m.CardId == s.CardId && (r.Stage == QushiStage.SealChildren ? m.From == CardLocation.Hand(f.OwnerSeat) && m.To == s.Location && m.Reason.Value == QushiSealReason : m.From == s.Location && m.To == CardLocation.DiscardPile && m.Reason.Value == QushiRemoveReason);
    }

    internal bool QushiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Qushi is not { Paid: true } || !ValidQushi(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => QushiMovementMatches(f, m));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return false;
    }

    internal ProgramSkillFrame? QushiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGuoTuOlOperations.QushiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27460)]
    internal bool AdvanceQushiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QushiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenYangOlRuntime
{
    internal bool WenYangOriginalParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } c || f.TriggerId is null)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        return f.WenYang!.Op switch
        {
            SkillProgramEffectOp.ResolveLvliActualDamageChoice => WenYangDamageParent(candidate, c),
            SkillProgramEffectOp.ResolveWenYangAwakening => WenYangAwakeningParent(candidate, c),
            SkillProgramEffectOp.ResolveQingjiaoWholeHandExchange => WenYangPlayParent(candidate, c),
            _ => CurrentWenYangDebt(candidate, c)is { } d && d.FrameId == f.WenYang.DebtId
        };
    }

    internal bool ValidWenYang(ProgramSkillFrame f)
    {
        if (f.WenYang is not { } r || r.Source != WenYangSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId || f.PendingMovementContinuation is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.ParentId != f.WindowContext?.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !Enum.IsDefined(r.Stage) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !WenYangOriginalParent(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new WenYangStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ParentId, r.ActualTurn, r.TurnOwner, r.DamageId, r.DebtId, r.Awakening))
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveLvliActualDamageChoice)
        {
            if (r.DamageId is null || r.Awakening != 0 || r.DebtId is not null || r.Entities.Count != 0 || r.MovementStep != 0)
                return false;
            if (r.Stage == WenYangStage.Choice)
                return !r.Paid && r.Branch is null;
            if (r.Stage is not (WenYangStage.DrawChildren or WenYangStage.RecoveryChildren) || !r.Paid || r.Branch is not ("draw" or "recover") || r.Requested < 0 || r.ExpectedRecovery < 0 || r.HpAfter != r.HpBefore + r.ExpectedRecovery || _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangLvliIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued != new WenYangLvliIssuedEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.TurnOwner, r.DamageId.Value, r.ParentId, r.Branch, r.Requested, r.ExpectedRecovery, r.HpBefore))
                return false;
            return r.Stage == WenYangStage.RecoveryChildren || _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangDrawIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var draw] && draw.Requested == r.Requested && draw.Actual >= 0 && draw.Actual <= draw.Requested && draw.Before == r.Before && draw.After == r.After;
        }

        if (r.Op == SkillProgramEffectOp.ResolveWenYangAwakening)
            return r.Paid && r.Stage is (WenYangStage.MaximumChildren or WenYangStage.GrantChildren) && r.Awakening == (f.SkillId == "ol:choujue" ? 1 : 2) && r.MaximumBefore > 0 && r.MaximumAfter == r.MaximumBefore - 1 && r.HpAfter == Math.Min(r.HpBefore, r.MaximumAfter) && _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangAwakenedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var awake] && awake == new WenYangAwakenedEvent(f.Id, f.OwnerSeat, r.Awakening, r.MaximumBefore, r.MaximumAfter, r.HpBefore, r.HpAfter);
        if (!r.Paid || r.Stage is not (WenYangStage.HandDiscardChildren or WenYangStage.GainChildren or WenYangStage.EndingChildren) || r.Entities.Select(c => c.CardId).Distinct().Count() != r.Entities.Count || r.MovementStep <= 0 || r.Before > r.After || _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangMovementIssuedEvent>().Where(e => e.FrameId == f.Id && e.Step == r.MovementStep).ToArray()is not [var movement] || movement != new WenYangMovementIssuedEvent(f.Id, r.MovementStep, r.Stage, r.Before, r.After, r.Entities.Count))
            return false;
        if (r.Stage == WenYangStage.EndingChildren && (r.DebtId is not { } debt || _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangQingjiaoDebtConsumedEvent>().Count(e => e.DebtId == debt && e.FrameId == f.Id && e.ParentId == r.ParentId && e.ActualTurn == r.ActualTurn) != 1))
            return false;
        if (r.Stage == WenYangStage.GainChildren && (r.Entities.Select(c => c.Kind).Distinct().Count() != r.Entities.Count || r.Entities.Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind)).Select(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot).Distinct().Count() != r.Entities.Count(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind)) || r.Entities.Count > 8))
            return false;
        var reason = r.Stage == WenYangStage.GainChildren ? WenYangGain : r.Stage == WenYangStage.HandDiscardChildren ? WenYangHandDiscard : WenYangEndingDiscard;
        return r.Entities.All(c => _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangEntityMovedEvent>().Where(e => e.FrameId == f.Id && e.Step == r.MovementStep && e.CardId == c.CardId).ToArray()is [var e] && e.Kind == c.Kind && e.From == c.From && e.Sequence > r.Before && e.Sequence <= r.After && e.To == (r.Stage == WenYangStage.GainChildren ? CardLocation.Hand(f.OwnerSeat) : c.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(c.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To && m.Reason.Value == reason) == 1);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27360)]
    internal void AssertWenYang(ProgramSkillFrame f)
    {
        if (f.WenYang is not null && !ValidWenYang(f))
            throw new InvalidOperationException("Wen Yang lost his exact native parent, frozen numeric request, once-paid awakening, physical movement or issued Ending debt.");
    }

    internal bool WenYangMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.WenYang!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == WenYangStage.DrawChildren)
            return m.Reason == CardMoveReasons.Reshuffle || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == WenYangDraw;
        if (m.Reason == CardMoveReasons.WoodenOxGrainDiscard)
            return r.Stage == WenYangStage.EndingChildren && r.WoodenOxGrain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.MovementStep && e.CardId == m.CardId && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence) && m.Reason.Value == (r.Stage == WenYangStage.GainChildren ? WenYangGain : r.Stage == WenYangStage.HandDiscardChildren ? WenYangHandDiscard : WenYangEndingDiscard);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 130)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 120)]
    internal bool IsWenYangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.WenYang is not { } r || !ValidWenYang(f) || recovery.ParentFrameId != f.Id || recovery.Return != new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) || recovery.Attempt.SourceSeat != f.OwnerSeat || recovery.Attempt.TargetSeat != f.OwnerSeat)
            return false;
        return r.Stage == WenYangStage.RecoveryChildren ? recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 && recovery.Attempt.Amount == r.ExpectedRecovery && r.ExpectedRecovery > 0 : r.Stage == WenYangStage.EndingChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == WenYangEndingDiscard && recovery.Attempt.Amount == 1 && r.Entities.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat));
    }

    internal bool WenYangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WenYang is not { Paid: true } r || !ValidWenYang(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => WenYangMovementMatches(f, m));
        if (child is RecoveryReplacementFrame recovery)
            return IsWenYangRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == f.OwnerSeat && (r.Stage == WenYangStage.MaximumChildren ? hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.SourceSeat is null && hp.Change.Amount == 1 && hp.Change.HpAfter == r.HpAfter : r.Stage == WenYangStage.RecoveryChildren ? hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && r.ExpectedRecovery > 0 && hp.Change.Amount > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<RecoveryAppliedEvent>().Any(e => e.SourceSeat == f.OwnerSeat && e.TargetSeat == f.OwnerSeat && e.Amount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter) : r.Stage == WenYangStage.EndingChildren && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.Amount == 1 && r.Entities.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat)));
        if (child is DeathFrame death)
            return r.Stage == WenYangStage.MaximumChildren && r.MaximumBefore == 1 && r.MaximumAfter == 0 && r.HpAfter <= 0 && death.ParentFrameId == f.Id && death.VictimSeat == f.OwnerSeat && death.KillerSeat is null && death.ReturnKind == DeathReturnKind.ProgramSkill;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? WenYangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterWenYangOlOperations.WenYangFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 820)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 930)]
    internal bool HasWenYangDamageObserver(long id) => WenYangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 960)]
    internal bool HasWenYangBeforeDamageObserver(long id) => HasWenYangDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 790)]
    internal bool IsWenYangDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && WenYangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27360)]
    internal bool AdvanceWenYangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WenYangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileShiRuntime
{
    internal bool ValidShiLs(ProgramSkillFrame f) => f.ShiLuSu is { } r && f.InstructionIndex == 1 && r.Source == ShiLsSource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && r.WindowId == f.WindowContext?.ParentFrameId && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsStartedEvent>().Count(e => e == new ShiLsStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.WindowId)) == 1;
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 48900)]
    internal bool ShiLsRecastBan(ProgramLegalActionQuery q) => q.Action.Kind == LegalActionKind.Recast && q.Action.CardId is { } id && IsShiLsHandAccess(q.Actor, id);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 48900)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 48900)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 48900)]
    internal bool ShiLsMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ShiLuSu is { } r && ValidShiLs(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is ShiLsStage.ExchangeChildren or ShiLsStage.DiscardChildren or ShiLsStage.DrawChildren && e?.Op is SkillProgramEffectOp.ResolveMobileShiLuSuHaoshi or SkillProgramEffectOp.ResolveMobileShiLuSuDimeng or SkillProgramEffectOp.ResolveMobileShiLuSuHaoshiRefill;
    internal bool ShiLsFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidShiLs(f) || f.ShiLuSu is not { } r)
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == ShiLsStage.Drawing && draw.Context.RecipientSeat == r.Target && draw.Context.RequestedCount == r.X && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ShiLsMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame rec)
            return rec.ParentFrameId == f.Id && rec.Return.ResumeFrameId == f.Id && rec.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiLsChildSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (index) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index), (index, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(index, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index, dying));
    internal ProgramSkillFrame? ShiLsObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterLuSuMobileShiOperations.ShiLsFirstChild(f, child), (i) => _characterLuSuMobileShiOperations.ShiLsChildSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 48900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 48900)]
    internal bool ShiLsDamageChild(long id) => ShiLsObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 48900)]
    internal bool ShiLsBeforeDamageChild(long id) => ShiLsObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 48900)]
    internal bool ShiLsDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiLsObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 48900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 48900)]
    internal bool KeepShiLsPaid(ProgramSkillFrame f) => f.ShiLuSu is not null && ValidShiLs(f) || f.ShiLuSu is null && f.TriggerId == "shared-last-hand" && f.InstructionIndex is 0 or 1 && f.WindowContext is { } w && IssuedShiLsRefill(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), w);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 48900)]
    internal bool AdvanceShiLsChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiLsObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiLuSuPublicStates(CharacterState owner, string id)
    {
        if (id != ShiLsHaoshi)
            return[];
        return LiveShiLsGrants().Where(g => g.Source.OwnerSeat == owner.Seat).SelectMany(g => new[] { new ProgramPublicRuleStateSnapshot(g.Source.SkillInstanceId, "haoshi-beneficiary", ProgramPublicRuleStateKind.GameActivationProgress, g.Beneficiary), new ProgramPublicRuleStateSnapshot(g.Source.SkillInstanceId, "haoshi-refill", ProgramPublicRuleStateKind.ActivationLimit, 2, _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsRefillIssuedEvent>().Count(e => e.GrantId == g.GrantId), SkillUsageScope.Game) }).ToArray();
    }
}

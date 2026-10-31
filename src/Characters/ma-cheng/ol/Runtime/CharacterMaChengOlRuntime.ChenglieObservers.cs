#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChengOlRuntime
{
    internal static bool ChenglieChildren(ChenglieStage s) => global::CardGame.Core.RuntimeSkills.RuntimeRules_ma_cheng_ol_ChenglieObservers.ChenglieChildren(s);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 250)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 240)]
    internal bool IsMaChengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Chenglie is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement)
            return false;
        if (r.Stage == ChenglieStage.RecoveryChildren && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)?.ChenglieLease is { } lease && r.Index < lease.Cards.Count)
            return recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == lease.Cards[r.Index].HolderSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1;
        return r.Stage == ChenglieStage.GiftChildren && r.GiftId is { } id && r.GiftFrom is { Zone: CardZoneKind.Equipment, OwnerSeat: { } donor } && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && recovery.Attempt.SourceSeat == donor && recovery.Attempt.TargetSeat == donor && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == ChenglieGiftReason;
    }

    internal bool ChenglieOwnMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.Chenglie!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieCardMovedEvent>().Any(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index && e.CardId == m.CardId && e.From == m.From && e.To == m.To && e.Reason == m.Reason && e.Sequence == m.Sequence))
            return true;
        if (r.Stage == ChenglieStage.RevealChildren)
            return m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        return r.Stage == ChenglieStage.GiftChildren && r.GiftId is { } id && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.WoodenOx && r.GiftFrom is { Zone: CardZoneKind.Equipment, OwnerSeat: { } donor } && m.From == CardLocation.WoodenOxGrain(donor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }

    internal bool MaChengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Chenglie is not { } r || !ChenglieChildren(r.Stage))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsMaChengRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => ChenglieOwnMovement(f, m));
    }

    internal ProgramSkillFrame? MaChengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MaChengFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27560)]
    internal bool AdvanceMaChengSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaChengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);

    internal void CompleteChenglieTerminalCleanup(CardUseFrame use)
    {
        if (use.ChenglieLease is not { Settled: false } lease)
            return;
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            throw new InvalidOperationException("Chenglie's live whole-use outcome and cleanup must finish before native pop.");
        foreach (var card in lease.Cards)
            if (_cardZonesOperations._cardZones.GetLocation(card.CardId) == card.Location)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId), card.Location, CardLocation.DiscardPile, new(ChenglieCleanupReason));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ChenglieLease = u.ChenglieLease!with { Ready = true, Settled = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieLeaseSettledEvent(use.Id, lease.SetupFrameId, use.Id));
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengPuBoundaryMobileRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryChengPuHost.Resolve(ProgramSkillFrame f) => BeginBoundaryChengPuLihuo(f);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 56200)]
    internal int BoundaryChengPuLihuoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is global::CardGame.Core.NativeOwners.CardAttackHandle && !attack.IsDelayedJudgmentDamage && !attack.IsProgramJudgmentDamage && attack.EffectiveCardKind == CardKind.FireSlash && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) && _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsChained && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.FireSlash } action && action.ActorSeat == attack.CardUserSeat && action.ConversionChain.Any(s => s.SkillId == BoundaryChengPuComposition.Lihuo && s.BindingId == "ordinary-slash-to-fire-slash" && s.OwnerSeat == action.ActorSeat) ? 1 : 0;
    internal int BoundaryChengPuPublicActionBonus(int actorSeat, LegalAction action, int targetSeat) => action.Kind == LegalActionKind.Slash && action.PlayedCardKind == CardKind.FireSlash && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsChained && new[]
    {
        action.ConversionSource
    }.Concat(action.AdditionalConversionSources ?? []).Any(s => s is not null && s.SkillId == BoundaryChengPuComposition.Lihuo && s.BindingId == "ordinary-slash-to-fire-slash" && s.OwnerSeat == actorSeat) ? 1 : 0;
    internal int BoundaryChengPuDamageTotal(long useId) => _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == useId && e.Amount > 0).Sum(e => e.Amount);
    internal CardUseFrame? BoundaryChengPuCompletedUse(ProgramSkillFrame f) => f.SkillId == BoundaryChengPuComposition.Lihuo && f.TriggerId == "fire-slash-completion-loss" && f.InstructionIndex == 1 && f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } c } && c.ActorSeat == f.OwnerSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is { Step: ResolutionFrameStep.Completed, CardKind: CardKind.FireSlash, CausedDamage: true, Action: { Type: CardActionType.Use } a } use && a.ActionId == c.CardActionId && a.ActorSeat == f.OwnerSeat ? use : null;
    internal SkillProgramStepOutcome BeginBoundaryChengPuLihuo(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = BoundaryChengPuCompletedUse(f) ?? throw new InvalidOperationException("Lihuo requires the original completed FireSlash.");
        if (f.BoundaryChengPuLihuo is not null)
            throw new InvalidOperationException("Lihuo cannot issue its payment twice.");
        var total = BoundaryChengPuDamageTotal(use.Id);
        var loss = total / 2;
        if (loss == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var receipt = new BoundaryChengPuLihuoReceipt(use.Id, use.Action!.ActionId, total, loss, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryChengPuLihuo = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryChengPuLihuoIssuedEvent(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, use.Id, receipt.ActionId, total, loss, 0));
        ContinueBoundaryChengPuLihuo(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueBoundaryChengPuLihuo(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.BoundaryChengPuLihuo!;
        if (!r.Paid)
        {
            // Match the ordinary native LoseHp host, keeping its pool choice
            // and identity appearance on this original producer before payment.
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(f.OwnerSeat) && _characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoProgramHealthChoice(f, f.OwnerSeat, HpChangeKind.Loss, r.RequestedLoss))
                return;
            var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, r.RequestedLoss, (before, after, lost) =>
            {
                var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(live with { BoundaryChengPuLihuo = live.BoundaryChengPuLihuo!with { Paid = true, HpBefore = before, HpAfter = after } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryChengPuLihuoPaidEvent(id, r.UseId, lost, before, after));
                if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(f.OwnerSeat))
                    _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(id, f.OwnerSeat, HpChangeKind.Loss, before, after);
            });
            if (outcome == SkillProgramStepOutcome.AwaitChild)
                return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryChengPuLihuo = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 56200)]
    internal bool ResumeBoundaryChengPuLihuo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryChengPuLihuo: not null } f || f.Id != id)
            return false;
        RequireBoundaryChengPuLihuo(f);
        ContinueBoundaryChengPuLihuo(id);
        return true;
    }

    internal void RequireBoundaryChengPuLihuo(ProgramSkillFrame f)
    {
        var r = f.BoundaryChengPuLihuo!;
        var use = BoundaryChengPuCompletedUse(f);
        if (use is null || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || r.TotalDamage != BoundaryChengPuDamageTotal(use.Id) || r.RequestedLoss != r.TotalDamage / 2 || r.RequestedLoss <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryChengPuLihuoIssuedEvent>().Count(e => e == new BoundaryChengPuLihuoIssuedEvent(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, r.UseId, r.ActionId, r.TotalDamage, r.RequestedLoss, 0)) != 1)
            throw new InvalidOperationException("Lihuo lost its exact completed Use, actual total or original payment source.");
        var payments = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryChengPuLihuoPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Paid ? payments is not [var paid] || paid != new BoundaryChengPuLihuoPaidEvent(f.Id, r.UseId, r.RequestedLoss, r.HpBefore, r.HpAfter) || r.HpAfter != r.HpBefore - r.RequestedLoss || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == r.RequestedLoss && e.RemainingHp == r.HpAfter && e.CompletedCardUseResolutionId == r.UseId) != 1 : payments.Length != 0)
            throw new InvalidOperationException("Lihuo lost its once-paid actual HP loss.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 56200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 56200)]
    internal bool KeepBoundaryChengPuLihuo(ProgramSkillFrame f)
    {
        if (f.BoundaryChengPuLihuo is null)
            return false;
        RequireBoundaryChengPuLihuo(f);
        return true;
    }

    internal bool BoundaryChengPuHealthChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryChengPuLihuo is not { } r)
            return false;
        RequireBoundaryChengPuLihuo(f);
        if (!r.Paid)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying)
            return dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.ResumesProgramSkill && dying.KillerSeat is null && r.HpAfter <= 0;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Loss && hp.Change.Amount == r.RequestedLoss;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange;
    }

    internal bool BoundaryChengPuHealthSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame dying && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? BoundaryChengPuHealthRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.BoundaryChengPuHealthChild(f, child), (n) => this.BoundaryChengPuHealthSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 56200)]
    internal bool BoundaryChengPuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && BoundaryChengPuHealthRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 56200)]
    internal bool AdvanceBoundaryChengPuHealth() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryChengPuHealthRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

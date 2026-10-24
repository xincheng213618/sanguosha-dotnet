namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class BoundaryChengPuCapability(GameEngine e) : IBoundaryChengPuHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => e.BeginBoundaryChengPuLihuo(f); }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 56200)]
    private int BoundaryChengPuLihuoDamage(IDamageAttempt attack) => !attack.IsChainPropagation && attack is CardAttackHandle &&
        !attack.IsDelayedJudgmentDamage && !attack.IsProgramJudgmentDamage && attack.EffectiveCardKind == CardKind.FireSlash &&
        IsValidPlayerSeat(attack.TargetSeat) && _players[attack.TargetSeat].IsChained &&
        LifecycleCardUse(attack.ResolutionId)?.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.FireSlash } action &&
        action.ActorSeat == attack.CardUserSeat && action.ConversionChain.Any(s => s.SkillId == BoundaryChengPuComposition.Lihuo &&
            s.BindingId == "ordinary-slash-to-fire-slash" && s.OwnerSeat == action.ActorSeat) ? 1 : 0;

    private int BoundaryChengPuPublicActionBonus(int actorSeat, LegalAction action, int targetSeat) =>
        action.Kind == LegalActionKind.Slash && action.PlayedCardKind == CardKind.FireSlash && _players[targetSeat].IsChained &&
        new[] { action.ConversionSource }.Concat(action.AdditionalConversionSources ?? []).Any(s => s is not null &&
            s.SkillId == BoundaryChengPuComposition.Lihuo && s.BindingId == "ordinary-slash-to-fire-slash" && s.OwnerSeat == actorSeat) ? 1 : 0;
    // Card damage events already carry the native Use id, including propagated
    // chain damage. Replacement HP loss has no DamageAppliedEvent and adds zero.
    private int BoundaryChengPuDamageTotal(long useId) => ProgramEventHistory<DamageAppliedEvent>()
        .Where(e => e.SourceCardUseResolutionId == useId && e.Amount > 0).Sum(e => e.Amount);
    private CardUseFrame? BoundaryChengPuCompletedUse(ProgramSkillFrame f) =>
        f.SkillId == BoundaryChengPuComposition.Lihuo && f.TriggerId == "fire-slash-completion-loss" && f.InstructionIndex == 1 &&
        f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } c } && c.ActorSeat == f.OwnerSeat &&
        LifecycleCardUse(c.ParentCardUseFrameId) is { Step: ResolutionFrameStep.Completed, CardKind: CardKind.FireSlash, CausedDamage: true, Action: { Type: CardActionType.Use } a } use &&
        a.ActionId == c.CardActionId && a.ActorSeat == f.OwnerSeat ? use : null;
    private SkillProgramStepOutcome BeginBoundaryChengPuLihuo(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var use = BoundaryChengPuCompletedUse(f) ?? throw new InvalidOperationException("Lihuo requires the original completed FireSlash.");
        if (f.BoundaryChengPuLihuo is not null) throw new InvalidOperationException("Lihuo cannot issue its payment twice.");
        var total = BoundaryChengPuDamageTotal(use.Id); var loss = total / 2;
        if (loss == 0 || !_players[f.OwnerSeat].IsAlive) return SkillProgramStepOutcome.Continue;
        var receipt = new BoundaryChengPuLihuoReceipt(use.Id, use.Action!.ActionId, total, loss, 0);
        ReplaceRuntimeTop(f = f with { BoundaryChengPuLihuo = receipt });
        AdvanceEventRulesAndQueueFact(new BoundaryChengPuLihuoIssuedEvent(f.Id, new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, use.Id, receipt.ActionId, total, loss, 0));
        ContinueBoundaryChengPuLihuo(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }
    private void ContinueBoundaryChengPuLihuo(long id)
    {
        var f = GetActiveProgramFrame(id); var r = f.BoundaryChengPuLihuo!;
        if (!r.Paid)
        {
            // Match the ordinary native LoseHp host, keeping its pool choice
            // and identity appearance on this original producer before payment.
            if (HasHappyQiaoHealth(f.OwnerSeat) && TryBeginHappyQiaoProgramHealthChoice(f, f.OwnerSeat, HpChangeKind.Loss, r.RequestedLoss)) return;
            var outcome = ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, r.RequestedLoss, (before, after, lost) =>
            {
                var live = GetActiveProgramFrame(id);
                ReplaceRuntimeTop(live with { BoundaryChengPuLihuo = live.BoundaryChengPuLihuo! with { Paid = true, HpBefore = before, HpAfter = after } });
                AdvanceEventRulesAndQueueFact(new BoundaryChengPuLihuoPaidEvent(id, r.UseId, lost, before, after));
                if (HasHappyQiaoHealth(f.OwnerSeat)) CompleteHappyQiaoHealthPayment(id, f.OwnerSeat, HpChangeKind.Loss, before, after);
            });
            if (outcome == SkillProgramStepOutcome.AwaitChild) return;
        }
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return;
        f = GetActiveProgramFrame(id); ReplaceRuntimeTop(f with { BoundaryChengPuLihuo = null }); AdvanceRuntimeProgram(id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 56200)]
    private bool ResumeBoundaryChengPuLihuo(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryChengPuLihuo: not null } f || f.Id != id) return false; RequireBoundaryChengPuLihuo(f); ContinueBoundaryChengPuLihuo(id); return true; }
    private void RequireBoundaryChengPuLihuo(ProgramSkillFrame f)
    {
        var r = f.BoundaryChengPuLihuo!; var use = BoundaryChengPuCompletedUse(f);
        if (use is null || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || r.TotalDamage != BoundaryChengPuDamageTotal(use.Id) || r.RequestedLoss != r.TotalDamage / 2 || r.RequestedLoss <= 0 ||
            ProgramEventHistory<BoundaryChengPuLihuoIssuedEvent>().Count(e => e == new BoundaryChengPuLihuoIssuedEvent(f.Id, new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, r.UseId, r.ActionId, r.TotalDamage, r.RequestedLoss, 0)) != 1)
            throw new InvalidOperationException("Lihuo lost its exact completed Use, actual total or original payment source.");
        var payments = ProgramEventHistory<BoundaryChengPuLihuoPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Paid ? payments is not [var paid] || paid != new BoundaryChengPuLihuoPaidEvent(f.Id, r.UseId, r.RequestedLoss, r.HpBefore, r.HpAfter) || r.HpAfter != r.HpBefore - r.RequestedLoss ||
            ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == r.RequestedLoss && e.RemainingHp == r.HpAfter && e.CompletedCardUseResolutionId == r.UseId) != 1 : payments.Length != 0)
            throw new InvalidOperationException("Lihuo lost its once-paid actual HP loss.");
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 56200)]
    private void AssertBoundaryChengPuLihuo(ProgramSkillFrame f) { if (f.BoundaryChengPuLihuo is not null) RequireBoundaryChengPuLihuo(f); }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 56200)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 56200)]
    private bool KeepBoundaryChengPuLihuo(ProgramSkillFrame f) { if (f.BoundaryChengPuLihuo is null) return false; RequireBoundaryChengPuLihuo(f); return true; }

    private bool BoundaryChengPuHealthChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryChengPuLihuo is not { } r) return false;
        RequireBoundaryChengPuLihuo(f);
        if (!r.Paid) return HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying) return dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.ResumesProgramSkill && dying.KillerSeat is null && r.HpAfter <= 0;
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(f, hp) && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Loss && hp.Change.Amount == r.RequestedLoss;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange;
    }
    private bool BoundaryChengPuHealthSuffix(int n) => _resolutionStack[n + 1] is DyingFrame dying && n + 2 < _resolutionStack.Count && IsSuspendedOrPaidRescueObserverSuffix(n + 1, dying) ||
        HasObserverSuffix(n, static (e, i) => e.IsMovementObserverEdge(i) || e.IsSameNameHandObserverEdge(i) || e.IsYanPuDyingExitEdge(i) ||
            e.TypedDamagePostEventEdge(e._resolutionStack[i - 1], e._resolutionStack[i]) || e.RecoveryReplacementFrameRidesOn(e._resolutionStack[i], e._resolutionStack[i - 1]),
            static (e, i, d) => e.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    private ProgramSkillFrame? BoundaryChengPuHealthRoot() => FindProgramObserverRoot(static (e, f, child) => e.BoundaryChengPuHealthChild(f, child), static (e, n) => e.BoundaryChengPuHealthSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 56200)] [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 56200)]
    private bool BoundaryChengPuDying() => ActiveDying is { } d && BoundaryChengPuHealthRoot() is { } f && _resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 56200)]
    private bool AdvanceBoundaryChengPuHealth() => _pendingDecision is null && BoundaryChengPuHealthRoot() is not null && TryAdvanceObserverSubtree(true);
}

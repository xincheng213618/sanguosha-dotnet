#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePaidOwnTargetObserversRuntime
{
    internal bool PaidOwnTargetMatches(ProgramSkillFrame root, ActualUseTargetWindowFrame parent, bool requireCost)
    {
        if (root.PaidOwnTarget is not { } paid || root.WindowContext is not { ActualUseTarget: { } original } context || context.Window != SkillProgramTriggerWindow.OtherActualUseTargeted || context.ParentFrameId != parent.Id || parent.ParentFrameId != paid.Use.CardUseFrameId || paid.Use != original || root.OwnerSeat != paid.Use.TargetSeat || paid.Source != new CardConversionSource(root.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(root), root.OwnerSeat, root.SkillInstanceId) || paid.GameplayHash != root.GameplayHash || paid.ActualLost != 1 || paid.HpBefore < 1 || paid.HpAfter != paid.HpBefore - 1 || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex]) || !_coreActualUseTargetProgramsOperations.MatchesActualUseTarget(paid.Use))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(root, _contentRegistryOperations._contentRegistry.GetSkill(root.SkillId).Program!);
        if (paid.InstructionIndex != 0 || plan.Instructions.Count == 0 || plan.Instructions[0].Op != SkillProgramEffectOp.PayHpThenNullifyOwnActualUseTarget)
            return false;
        if (!requireCost)
            return true;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<PaidOwnTargetHpEvent>().Count(e => e.ProgramFrameId == root.Id && e.Receipt == (paid with { Applied = false })) == 1 && history.OfType<ProgramSkillHpLostEvent>().Count(e => e.FrameId == root.Id) == 1 && history.OfType<ProgramSkillHpLostEvent>().Any(e => e.FrameId == root.Id && e.SkillId == root.SkillId && e.TargetSeat == root.OwnerSeat && e.Amount == 1 && e.RemainingHp == paid.HpAfter) && (!paid.Applied || history.OfType<PaidOwnTargetAppliedEvent>().Count(e => e.ProgramFrameId == root.Id && e.CardUseFrameId == paid.Use.CardUseFrameId && e.ActionId == paid.Use.ActionId && e.ActorSeat == paid.Use.ActorSeat && e.TargetSeat == root.OwnerSeat && e.EffectiveKind == paid.Use.EffectiveKind && e.Source == paid.Source && e.GameplayHash == paid.GameplayHash) == 1);
    }

    internal SkillProgramStepOutcome PayOwnActualUseTarget(ProgramSkillFrame frame)
    {
        var root = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ActualUseTargetWindowFrame parent)
            throw new InvalidOperationException("An actual target payment requires its direct owning window.");
        if (root.PaidOwnTarget is null)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            var context = root.WindowContext!;
            if (!_coreActualUseTargetProgramsOperations.CanRunActualUseTarget(c, context) || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, c) || root.InstructionIndex != 1)
                throw new InvalidOperationException("The target payment lost its exact original use/candidate.");
            var before = _coreMatchParticipantsPlayersAccess._players[root.OwnerSeat].Hp;
            var receipt = new ProgramPaidOwnTargetReceipt(0, context.ActualUseTarget!, new(root.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(root), root.OwnerSeat, root.SkillInstanceId), root.GameplayHash, before, before - 1, 1);
            _coreRuntimeOperations.ReplaceRuntimeTop(root with { PaidOwnTarget = receipt, ReexecuteParticipantInstruction = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PaidOwnTargetHpEvent(root.Id, receipt));
            return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(root.Id, root.SkillId, root.OwnerSeat, 1);
        }

        if (!PaidOwnTargetMatches(root, parent, true))
            throw new InvalidOperationException("The resumed target payment lost its committed cost.");
        ApplyPaidOwnTarget(root);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(root.Id), "胜负已定，仅完成已付目标无效并取消未选择收益。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal void ApplyPaidOwnTarget(ProgramSkillFrame frame)
    {
        if (frame.PaidOwnTarget is not { Applied: false } paid)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ActualUseTargetWindowFrame parent || !PaidOwnTargetMatches(frame, parent, true))
            throw new InvalidOperationException("A paid target cannot apply to a different or completed use.");
        _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(paid.Use.CardUseFrameId, paid.Use.TargetSeat);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { PaidOwnTarget = paid with { Applied = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PaidOwnTargetAppliedEvent(frame.Id, paid.Use.CardUseFrameId, paid.Use.ActionId, paid.Use.ActorSeat, paid.Use.TargetSeat, paid.Use.EffectiveKind, paid.Source, paid.GameplayHash));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardEffectNullifiedEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, paid.Use.ActorSeat, paid.Use.CardUseFrameId, paid.Use.EffectiveKind));
    }

    internal void FinishPaidOwnTargetBeforeProgramCompletion(ProgramSkillFrame frame)
    {
        if (frame.PaidOwnTarget is { Applied: false })
            ApplyPaidOwnTarget(frame);
    }

    internal ProgramSkillFrame? ActualUseTargetObserverRoot(long useId)
    {
        var wi = _resolutionStackOperations._resolutionStack.FindIndex(f => f is ActualUseTargetWindowFrame w && w.ParentFrameId == useId);
        if (wi < 1 || _resolutionStackOperations._resolutionStack[wi] is not ActualUseTargetWindowFrame window || _resolutionStackOperations._resolutionStack[wi - 1] is not CardUseFrame use || use.Id != useId || window.Candidates.Count != window.Contexts.Count || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Contexts[window.CandidateIndex].ActualUseTarget is not { } identity || !_coreActualUseTargetProgramsOperations.MatchesActualUseTarget(identity))
            return null;
        if (wi + 1 >= _resolutionStackOperations._resolutionStack.Count)
            return null;
        if (_resolutionStackOperations._resolutionStack[wi + 1] is not ProgramSkillFrame root || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, window.Candidates[window.CandidateIndex]) || root.WindowContext?.ParentFrameId != window.Id || root.WindowContext.ActualUseTarget != identity)
            return null;
        if (wi + 2 == _resolutionStackOperations._resolutionStack.Count)
            return root;
        if (!PaidOwnTargetMatches(root, window, true) && !_coreHalfHandPhaseDebtObserversOperations.HalfHandSupportObserverRootMatches(root, window) && !_coreOwnTrickDrawTargetsOperations.OwnTrickDrawMatches(root, window))
            return null;
        if (root.OwnTrickDraw is not null && !_coreOwnTrickDrawTargetsOperations.OwnTrickDrawFirstChild(root, _resolutionStackOperations._resolutionStack[wi + 2]))
            return null;
        for (var i = wi + 2; i < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (!(root.HalfHandSupport is { Paid: true } || root.OwnTrickDraw is not null ? _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(i) : _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(i)))
                return null;
            if (_resolutionStackOperations._resolutionStack[i] is DyingFrame d && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i, d) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(i, d) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(i, d) || (root.HalfHandSupport is { Paid: true } || root.OwnTrickDraw is not null) && _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(i, d) || root.OwnTrickDraw is not null && _coreOwnTrickDrawAlcoholObserversOperations.OwnTrickDrawLegacyAlcoholRide(root, window, i, d)))
                break;
        }

        return root;
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 480)]
    internal bool IsPaidOwnTargetProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().Any(w => ActualUseTargetObserverRoot(w.ParentFrameId)is { PaidOwnTarget: not null });

    SkillProgramStepOutcome IPaidTargetEndingProgramHost.PayHpThenNullifyOwnActualUseTarget(ProgramSkillFrame f) => PayOwnActualUseTarget(f);
    void IPaidTargetEndingProgramHost.ScheduleEarnedActualEndingBenefit(ProgramSkillFrame f, string skill, string binding) => _coreEarnedActualEndingBenefitsOperations.IssueEarnedActualEnding(f, skill, binding);
    SkillProgramStepOutcome IPaidTargetEndingProgramHost.DrawLostHpThenOfferOwnedCardsUpTo(ProgramSkillFrame f) => _coreLostHpOwnedGiftsOperations.RunLostHpOwnedGift(f);
}

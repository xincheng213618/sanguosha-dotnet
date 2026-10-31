#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXunAnniversaryRuntime
{
    internal const string ZhangXunFollowupAction = "anniversary-zhang-xun-followup";
    internal CardConversionSource ZhangXunFrameSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IAnniversaryZhangXunHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.ArmAnniversaryZhangXunSuizheng ? ArmAnniversaryZhangXun(f) : ChooseAnniversaryZhangXunSlash(f);
    internal SkillProgramStepOutcome ArmAnniversaryZhangXun(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != AnniversaryZhangXunComposition.Skill || f.TriggerId != AnniversaryZhangXunComposition.Arm || f.ActivationId != f.TriggerId || f.InstructionIndex != 2 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not TurnEndingBoundaryFrame parent || parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || parent.OwnerSeat != f.OwnerSeat || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || context.OwnerSeat != f.OwnerSeat || context.SourceSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || context.OccurrenceIndex != candidate.OccurrenceIndex || context.Facts != (parent.Items[parent.ItemIndex].Facts ?? parent.Facts) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhangXunSuizhengLeaseEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Suizheng requires its exact original Ending candidate and native selected recipient.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangXunSuizhengLeaseEvent(f.Id, ZhangXunFrameSource(f), f.GameplayHash, parent.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, recipient));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome ChooseAnniversaryZhangXunSlash(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhangXunSuizheng is not null || f.SkillId != AnniversaryZhangXunComposition.Skill || f.TriggerId != AnniversaryZhangXunComposition.Followup || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Suizheng requires its original unstarted issued Play-ending choice.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w.OccurrenceIndex);
        var due = ExactZhangXunCandidate(candidate, w);
        if (due is null || !IsIssuedAnniversaryZhangXunCandidate(candidate, w))
            throw new InvalidOperationException("Suizheng lost the exact issuer, next actual turn or native Play-ending candidate.");
        var cutoff = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count();
        var victims = ZhangXunVictims(due, cutoff);
        if (victims.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var r = new ZhangXunSuizhengFollowupReceipt(ZhangXunFrameSource(f), f.GameplayHash, due.Lease.FrameId, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, due.Lease.RecipientSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, cutoff)
        {
            Victims = victims
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangXunSuizheng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangXunSuizhengFollowupStartedEvent(f.Id, r.LeaseId, r.ParentWindowId, r.ActualTurn, r.RecipientSeat, r.PhaseInstance, cutoff, victims.Count));
        foreach (var(victim, index)in victims.Select((v, i) => (v, i)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangXunSuizhengVictimIssuedEvent(f.Id, index, victim.EventIndex, victim.TargetSeat));
        PublishZhangXunFollowup(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ZhangXunFollowupChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.ZhangXunSuizheng!.Victims.Where(v => LegalZhangXunFollowupTarget(f.OwnerSeat, v.TargetSeat)).Select(v => new PromptChoice(new($"zhang-xun.{(f.Id)}.{(v.TargetSeat)}"), "视为对" + _coreMatchParticipantsPlayersAccess._players[v.TargetSeat].Name + "使用一张杀", [], [v.TargetSeat], new Dictionary<string, string> { ["program-action"] = ZhangXunFollowupAction })).ToArray());
    internal void PublishZhangXunFollowup(ProgramSkillFrame f)
    {
        var choices = ZhangXunFollowupChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, "随征：选择本回合受益角色实际造成过伤害的角色。", f.ZhangXunSuizheng!.RecipientSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ZhangXunFollowupAction)]
    internal void AnswerZhangXunFollowup(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangXunSuizheng: { SelectedTarget: null } r } f || !ValidZhangXunFollowup(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ZhangXunFollowupChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Suizheng rejects another issuer's turn, damage victim or stale native target choice.");
        var target = choice.Targets.Single();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, SelectedTargetSeats = Array.AsReadOnly(new[] { target }), ZhangXunSuizheng = r with { SelectedTarget = target } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangXunSuizhengTargetChosenEvent(f.Id, r.LeaseId, target));
        // Continue the configured UseVirtualSlash instruction; it owns the real
        // CardUse/response/damage chain and typed Program parent return.
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseAnniversaryZhangXunSuizhengSlash)]
    internal PromptChoice ZhangXunFollowupAi(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45200)]
    internal bool KeepIssuedZhangXunFollowup(ProgramSkillFrame f)
    {
        if (f.ZhangXunSuizheng is not null)
            return ValidZhangXunFollowup(f);
        return f.SkillId == AnniversaryZhangXunComposition.Skill && f.TriggerId == AnniversaryZhangXunComposition.Followup && f.InstructionIndex == 0 && f.WindowContext is { } w && IsIssuedAnniversaryZhangXunCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w.OccurrenceIndex), w);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45200)]
    internal bool KeepZhangXunAlreadyUsedSlash(ProgramSkillFrame f) => f.ZhangXunSuizheng is { SelectedTarget: not null } && ValidZhangXunFollowup(f);
}

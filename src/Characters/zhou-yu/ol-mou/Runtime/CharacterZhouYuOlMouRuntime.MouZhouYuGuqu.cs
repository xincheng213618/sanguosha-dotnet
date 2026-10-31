#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal const string MouGuquDrawReason = "skill-program.mou-zhou-yu.guqu.draw";
    SkillProgramStepOutcome global::CardGame.Core.IMouZhouYuGuquHost.Resolve(ProgramSkillFrame frame) => BeginMouGuqu(frame);
    internal static CardConversionSource MouGuquFrameSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_yu_ol_mou_MouZhouYuGuqu.MouGuquFrameSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29021, SkillProgramEffectOp.ResolveMouZhouYuGuqu)]
    internal bool CanOfferMouGuqu(ProgramTriggerLegalityContext gate) => IsIssuedMouGuquCandidate(gate.Candidate, gate.Context);
    [ProgramIssuedTrigger(70)]
    internal bool IsIssuedMouGuquCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && MouGuquCompletedClaim(c, context)is { } q && !_coreProgramEventHistoryOperations.ProgramEventHistory<GuquStartedEvent>().Any(e => e.Claim.Source == q.Source && e.Claim.ActionId == q.ActionId);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 22)]
    internal void CollectIssuedMouGuquCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasMouGuquCapability || window != SkillProgramTriggerWindow.CardUseCompleted)
            return;
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (parent?.GuquClaims is not { } claims)
            return;
        foreach (var q in claims.Where(q => q.ActionId == action.ActionId && q.ParentFrameId == parent.Id && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && ValidMouGuquClaim(q, action)))
        {
            if (result.Any(c => c.OwnerSeat == q.Source.OwnerSeat && c.SkillId == q.Source.SkillId && c.SkillInstanceId == q.Source.SkillInstanceId && c.TriggerId == q.Source.BindingId))
                continue;
            var t = _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!.Triggers.Single(t => t.Id == q.Source.BindingId);
            result.Add(new(q.Source.OwnerSeat, -1, q.Source.SkillId, q.Source.BindingId, q.GameplayHash, q.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, q.Source.OwnerSeat, -1, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat], action))));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ObservesResponseUseCompletion, 1)]
    internal bool HasIssuedMouGuquResponse(CardActionContext action) => HasMouGuquCapability && _resolutionStackOperations._resolutionStack.Any(f => f.GuquClaims?.Any(q => q.ActionId == action.ActionId && q.ParentFrameId == f.Id && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && ValidMouGuquClaim(q, action)) == true);
    internal SkillProgramStepOutcome BeginMouGuqu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, f.WindowContext?.OccurrenceIndex ?? 0);
        if (f.Guqu is not null || f.SkillId != MouGuquSkill || f.ActivationId != MouGuquBinding || f.TriggerId != MouGuquBinding || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !IsIssuedMouGuquCandidate(candidate, context) || MouGuquCompletedClaim(candidate, context)is not { } q || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, w))
            throw new InvalidOperationException("Guqu requires its original source's marked true Use claim and exact native completion window.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Guqu = new(q, w.Id, GuquStage.Drawing) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuquStartedEvent(f.Id, q, w.Id));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的【顾曲】摸两张牌。", f.OwnerSeat);
        ContinueMouGuqu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouZhouYuGuqu)]
    internal void ReturnMouGuquDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.Guqu is not { Stage: GuquStage.Drawing } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<GuquDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Guqu cannot repeat a completed native draw or replace its actual-material invoice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Guqu = r with { Stage = GuquStage.DrawChildren, DrawOutcome = o }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuquDrawReturnedEvent(f.Id, 2, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId));
        ContinueMouGuqu(f.Id);
    }

    internal void ContinueMouGuqu(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Guqu: { } r } f || f.Id != id)
            return;
        AssertMouGuqu(f);
        if (r.Stage == GuquStage.Drawing)
        {
            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(MouGuquDrawReason));
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuquCompletedEvent(f.Id, r.Claim.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { Guqu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29021)]
    internal bool ResumeMouGuqu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Guqu: not null } f || f.Id != id)
            return false;
        ContinueMouGuqu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29021)]
    internal bool ReturnMouGuquMovement(ProgramSkillFrame f)
    {
        if (f.Guqu is null || f.PendingMovementContinuation is null)
            return false;
        ContinueMouGuqu(f.Id);
        return true;
    }
}

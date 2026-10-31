#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShangxiangSpOlRuntime
{
    internal bool HasSpSunShangxiangLiangzhu => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSpSunShangxiangLiangzhu);

    internal static CardConversionSource SpSunShangxiangSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool IsSpSunShangxiangLiangzhuTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveSpSunShangxiangLiangzhu }];
    SkillProgramStepOutcome global::CardGame.Core.ISpSunShangxiangOlHost.Liangzhu(ProgramSkillFrame f) => BeginSpSunShangxiangLiangzhu(f);
    SkillProgramStepOutcome global::CardGame.Core.ISpSunShangxiangOlHost.CompleteFanxiang(ProgramSkillFrame f) => CompleteSpSunShangxiangFanxiang(f);
    internal void CaptureSpSunShangxiangRecovery(HpChangeContext change)
    {
        if (!HasSpSunShangxiangLiangzhu || change.Kind != HpChangeKind.Recovery || change.Amount <= 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != change.TargetSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.AfterHpRecovered).Where(c => IsSpSunShangxiangLiangzhuTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(c))))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangRecoveryQualifiedEvent(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, change.Id, change.ParentFrameId, change.TargetSeat, change.HpBefore, change.HpAfter, phase));
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectSpSunShangxiangRecoveryCandidates(HpChangeContext change) => _coreProgramEventHistoryOperations.ProgramEventHistory<SpSunShangxiangRecoveryQualifiedEvent>().Where(q => q.ChangeId == change.Id && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat], q.Source.SkillId, q.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash == q.GameplayHash).Select(q => new ProgramTriggerCandidate(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!.Triggers.Single(t => t.Id == q.Source.BindingId).Priority)).ToArray();
    internal SpSunShangxiangRecoveryQualifiedEvent? SpSunShangxiangRecoveryQualification(ProgramTriggerCandidate c, HpChangeContext change) => _coreProgramEventHistoryOperations.ProgramEventHistory<SpSunShangxiangRecoveryQualifiedEvent>().SingleOrDefault(q => q.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && q.GameplayHash == c.GameplayHash && q.ChangeId == change.Id && q.RecoveryParentId == change.ParentFrameId && q.RecoveredSeat == change.TargetSeat && q.HpBefore == change.HpBefore && q.HpAfter == change.HpAfter && change.Kind == HpChangeKind.Recovery && change.Amount == change.HpAfter - change.HpBefore && q.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && q.Phase.ActorSeat == change.TargetSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(q.Phase));
    internal ProgramSkillWindowContext CreateSpSunShangxiangRecoveryContext(HpChangeContext change, ProgramTriggerCandidate c)
    {
        if (SpSunShangxiangRecoveryQualification(c, change)is null)
            throw new InvalidOperationException("Liangzhu requires its frozen actual own-Play recovery.");
        return new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), HpChange: change);
    }

    internal bool CanRunSpSunShangxiangLiangzhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.AfterHpRecovered && context.OwnerSeat == c.OwnerSeat && context.HpChange is { Kind: HpChangeKind.Recovery, Amount: > 0 } change && context.ParentFrameId == change.Id && context.TargetSeat == change.TargetSeat && context.SourceSeat == change.SourceSeat && context.Amount == change.Amount && c.OccurrenceIndex == 0 && SpSunShangxiangRecoveryQualification(c, change)is not null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32300)]
    internal bool CanOfferSpSunShangxiangLiangzhu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !IsSpSunShangxiangLiangzhuTrigger(t) || CanRunSpSunShangxiangLiangzhu(c, context);
    internal SkillProgramStepOutcome BeginSpSunShangxiangLiangzhu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SpSunShangxiangLiangzhu is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !CanRunSpSunShangxiangLiangzhu(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), w))
            throw new InvalidOperationException("Liangzhu cannot repeat or borrow another native recovery occurrence.");
        var r = new SpSunShangxiangLiangzhuReceipt(SpSunShangxiangSource(f), f.GameplayHash, w.ParentFrameId, w.TargetSeat!.Value);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpSunShangxiangLiangzhu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangLiangzhuStartedEvent(f.Id, r.Source, r.GameplayHash, r.ChangeId, r.RecoveredSeat));
        PublishSpSunShangxiangLiangzhu(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SpSunShangxiangLiangzhuChoices(ProgramSkillFrame f)
    {
        var r = f.SpSunShangxiangLiangzhu!;
        var choices = new List<PromptChoice>();
        foreach (var(branch, text)in new[]
        {
            ("self", "自己摸一张牌"),
            ("help", $"令{(_coreMatchParticipantsPlayersAccess._players[r.RecoveredSeat].Name)}摸两张牌"),
            ("decline", "不发动良助")
        }

        )
        {
            if (branch == "help" && !_coreMatchParticipantsPlayersAccess._players[r.RecoveredSeat].IsAlive)
                continue;
            choices.Add(new(new($"sp-sun-shangxiang.{(f.Id)}.{(branch)}"), text, [], [], new Dictionary<string, string> { ["program-action"] = "sp-sun-shangxiang-liangzhu", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSpSunShangxiangLiangzhu(ProgramSkillFrame f)
    {
        var choices = SpSunShangxiangLiangzhuChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "良助：自己摸一张牌，或令刚于自己出牌阶段回复体力的角色摸两张牌。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("sp-sun-shangxiang-liangzhu")]
    internal void AnswerSpSunShangxiangLiangzhu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SpSunShangxiangLiangzhu: { Stage: SpSunShangxiangLiangzhuStage.Choosing } r } f || !ValidSpSunShangxiangLiangzhu(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !SpSunShangxiangLiangzhuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Liangzhu lost its exact optional source-owner choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            FinishSpSunShangxiangLiangzhu(f);
            return;
        }

        var recipient = branch == "help" ? r.RecoveredSeat : f.OwnerSeat;
        var count = branch == "help" ? 2 : 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpSunShangxiangLiangzhu = r with { Stage = SpSunShangxiangLiangzhuStage.Drawing, Branch = branch, RecipientSeat = recipient, Requested = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangLiangzhuChoiceEvent(f.Id, branch, recipient, count));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, count, new("skill-program.ol:liangzhu.draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveSpSunShangxiangLiangzhu)]
    internal void ReturnSpSunShangxiangLiangzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (!ValidSpSunShangxiangLiangzhu(f) || f.SpSunShangxiangLiangzhu is not { Stage: SpSunShangxiangLiangzhuStage.Drawing, Draw: null } r || outcome.RecipientSeat != r.RecipientSeat || outcome.RequestedCount != r.Requested)
            throw new InvalidOperationException("Liangzhu must return its accepted actual native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpSunShangxiangLiangzhu = r with { Stage = SpSunShangxiangLiangzhuStage.Children, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangLiangzhuDrawEvent(f.Id, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.AttemptFrameId));
        if (r.Branch == "help")
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangHelpedEvent(f.Id, f.OwnerSeat, r.RecoveredSeat, r.ChangeId));
        ContinueSpSunShangxiangLiangzhu(f.Id);
    }

    internal void ContinueSpSunShangxiangLiangzhu(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SpSunShangxiangLiangzhu: { } r } f || f.Id != id)
            return;
        if (!ValidSpSunShangxiangLiangzhu(f))
            throw new InvalidOperationException("Liangzhu lost its original paid native draw cursor.");
        if (r.Stage == SpSunShangxiangLiangzhuStage.Choosing)
        {
            PublishSpSunShangxiangLiangzhu(f);
            return;
        }

        if (r.Stage == SpSunShangxiangLiangzhuStage.Drawing)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishSpSunShangxiangLiangzhu(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishSpSunShangxiangLiangzhu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangLiangzhuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpSunShangxiangLiangzhu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveSpSunShangxiangLiangzhu)]
    internal PromptChoice AiSpSunShangxiangLiangzhu(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "help" && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[f.SpSunShangxiangLiangzhu!.RecoveredSeat])) ?? p.Choices.Single(c => c.Parameters["branch"] == "self");
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 90, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveSpLiangzhuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsSpSunShangxiangLiangzhuTrigger(trigger) ? CanRunSpSunShangxiangLiangzhu(candidate, context) : null;
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheXiashengRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheXiashengHost.ResolveXiasheng(ProgramSkillFrame frame, string stage) => BeginShanheXiasheng(frame, stage);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 5408)]
    internal bool ShanheXiashengRequiresTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheXiasheng);
    internal (int Red, int Black) ShanheXiashengColors(int seat)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
        return (hand.Count(card => card.Suit is Suit.Heart or Suit.Diamond), hand.Count(card => card.Suit is Suit.Spade or Suit.Club));
    }

    internal CardActionContext? ShanheXiashengAction(int owner, ProgramSkillWindowContext context) => context.CardUse is { } identity && identity.ActorSeat == owner && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId)is { } window && window.Action.ActionId == identity.CardActionId && window.Action.ActorSeat == owner && (window.Action.Type == CardActionType.Use || window.Action.Type == CardActionType.Response && window.Action.EffectiveKind is CardKind.Dodge or CardKind.Nullification) ? window.Action : null;
    internal IReadOnlyList<IReadOnlyList<int>> ShanheXiashengExtraTargets(CardUseFrame use) => _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidates(use).Where(targets => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) ? _coreRecipientCategorySlashTargetsOperations.CanBeRecipientCategorySlashTarget(use, _coreMatchParticipantsPlayersAccess._players[targets[0]]) : use.CardKind != CardKind.Snatch || _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, _coreMatchParticipantsPlayersAccess._players[targets[0]])).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5408)]
    internal bool CanOfferShanheXiasheng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheXiasheng))
            return true;
        if (candidate.SkillId != "shanhe:xiasheng-3362" || ShanheXiashengAction(candidate.OwnerSeat, context)is not { EffectiveIsRed: { } red } action)
            return false;
        var colors = ShanheXiashengColors(candidate.OwnerSeat);
        if (trigger.Effects[0].StateId == "draw")
            return !red && colors.Red > colors.Black;
        return red && colors.Black > colors.Red && _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(candidate.OwnerSeat, context, out var use, out _) && ShanheXiashengExtraTargets(use).Count > 0;
    }

    internal SkillProgramStepOutcome BeginShanheXiasheng(ProgramSkillFrame supplied, string stage)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheXiasheng is not null || frame.WindowContext is not { } context || ShanheXiashengAction(frame.OwnerSeat, context)is not { } action)
            throw new InvalidOperationException("Xiasheng requires its original current-use frame.");
        var colors = ShanheXiashengColors(frame.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheXiasheng = new(context.ParentFrameId, action.ActionId, stage, colors.Red, colors.Black) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheXiashengBranchIssuedEvent(frame.Id, frame.OwnerSeat, action.ActionId, stage, colors.Red, colors.Black));
        if (stage == "draw")
            return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, 1, new("skill-program.shanhe-xiasheng.draw"));
        if (!_coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(frame.OwnerSeat, context, out var use, out _))
            throw new InvalidOperationException("Xiasheng lost its actual designation parent.");
        if (use.CardKind == CardKind.DrawTwo && use.Action!.EffectiveDesignatedTargetSeats.Count == 0)
        {
            var targets = new[]
            {
                use.SourceSeat
            };
            var normalized = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = normalized }, normalized);
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        }

        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "shanhe-xiasheng"
        };
        var choices = ShanheXiashengExtraTargets(use).Select(targets => new PromptChoice(new($"xiasheng.{(frame.Id)}.{(string.Join(".", targets))}"), "夏晟：额外指定" + string.Join("、", targets.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)), [], targets, Parameters())).Append(new(new($"xiasheng.{(frame.Id)}.decline"), "不追加目标", [], [], Parameters())).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheXiasheng = frame.ShanheXiasheng!with { Choices = choices, OriginalTargets = use.TargetSeats }, Step = ResolutionFrameStep.AwaitingResponse });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetOfferedEvent(frame.Id, use.Id, action.ActionId, frame.InstructionIndex, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, frame.ShanheXiasheng.OriginalTargets, choices.SelectMany(choice => choice.Targets.Take(1)).Distinct().ToArray()));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "夏晟：可为此红色牌额外指定一个合法目标", [], choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = frame.ShanheXiasheng.Choices,
            IsPrivate = false,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("shanhe-xiasheng")]
    internal void AnswerShanheXiasheng(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheXiasheng: { Stage: "target" } receipt } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || frame.WindowContext?.ParentFrameId != receipt.ParentWindowId || receipt.Choices.SingleOrDefault(choice => choice.Id == selected.Id)is not { } choice || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected]) || !_coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(frame.OwnerSeat, frame.WindowContext, out var use, out var window) || use.Action!.ActionId != receipt.ActionId)
            throw new InvalidOperationException("Xiasheng lost its exact original red use or issued legal target choice.");
        if (choice.Targets.Count > 0)
        {
            if (!ShanheXiashengExtraTargets(use).Any(targets => targets.SequenceEqual(choice.Targets)))
                throw new InvalidOperationException("Xiasheng's additional target became illegal.");
            var targets = use.TargetSeats.Concat(choice.Targets).ToArray();
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            var simple = use.AdjustedSimpleContinuation;
            if (window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
                simple ??= window.SimpleContinuation ?? throw new InvalidOperationException("Xiasheng lost its native basic-card continuation.");
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, AdjustedSimpleContinuation = simple, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, choice.Targets[0]));
        }

        var result = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!.TargetSeats;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetResolvedEvent(frame.Id, use.Id, receipt.ActionId, frame.InstructionIndex, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, receipt.OriginalTargets, choice.Targets, result));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheXiasheng = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool IsShanheXiashengTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> originalTargets)
    {
        if (fact.Source.SkillId != "shanhe:xiasheng-3362" || use.Action is not { Type: CardActionType.Use } action || fact.CardUseFrameId != use.Id || fact.ActionId != action.ActionId || fact.Source.OwnerSeat != action.ActorSeat || fact.InstructionIndex != 1 || !fact.OriginalTargetSeats.SequenceEqual(originalTargets) || fact.AddedTargetSeats.Count != (use.CardKind == CardKind.BorrowedSword ? 2 : 1) || !fact.ResultTargetSeats.SequenceEqual(originalTargets.Concat(fact.AddedTargetSeats)) || _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program is not { } program || program.GameplayHash != fact.GameplayHash || program.Triggers.SingleOrDefault(trigger => trigger.Id == fact.Source.BindingId)?.Effects is not [{ Op: SkillProgramEffectOp.ResolveShanheXiasheng, StateId: "target" }])
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheXiashengBranchIssuedEvent>().Any(branch => branch.FrameId == fact.ProgramFrameId && branch.ActionId == fact.ActionId && branch.OwnerSeat == action.ActorSeat && branch.Branch == "target" && branch.BlackCount > branch.RedCount) && _coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetResolvedEvent>().Count(item => item.ProgramFrameId == fact.ProgramFrameId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetOfferedEvent>().Count(item => item.ProgramFrameId == fact.ProgramFrameId && item.CardUseFrameId == use.Id && item.ActionId == fact.ActionId && item.Source == fact.Source && item.GameplayHash == fact.GameplayHash && item.OriginalTargetSeats.SequenceEqual(originalTargets) && item.CandidateTargetSeats.Contains(fact.AddedTargetSeats[0])) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseTargetAddedEvent>().Count(item => item.FrameId == fact.ProgramFrameId && item.CardUseFrameId == use.Id && item.OwnerSeat == action.ActorSeat && item.SkillId == fact.Source.SkillId && item.TargetSeat == fact.AddedTargetSeats[0]) == 1;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheXiasheng)]
    internal void ReturnShanheXiashengDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheXiasheng is not { Stage: "draw", DrawOutcome: null } receipt || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != 1 || frame.NativeDraw is not null)
            throw new InvalidOperationException("Xiasheng lost its once-issued black-card draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheXiasheng = receipt with { Stage = "draw-children", DrawOutcome = outcome } });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 5408)]
    internal bool ResumeShanheXiasheng(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheXiasheng: { Stage: "draw-children" } } frame || frame.Id != frameId)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheXiasheng = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheXiasheng)]
    internal PromptChoice AiShanheXiasheng(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.OrderByDescending(choice => choice.Targets.Count == 0 ? 0 : choice.Targets.Sum(seat => _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[seat]) ? -1 : 2)).First();
}

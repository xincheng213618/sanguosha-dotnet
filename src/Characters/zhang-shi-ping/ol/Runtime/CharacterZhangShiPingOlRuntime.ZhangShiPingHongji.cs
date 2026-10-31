#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangShiPingOlRuntime
{
    internal bool HasHongjiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmHongjiNextPhase);

    internal static CardConversionSource ZhangShiPingSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_shi_ping_ol_ZhangShiPingHongji.ZhangShiPingSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IZhangShiPingProgramHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ArmHongjiNextPhase => BeginHongjiPromise(f),
        SkillProgramEffectOp.ResolveHongjiGrantedPhase => BeginHongjiExtraPhase(f),
        _ => BeginXingGu(f, op)};
    internal (bool Draw, bool Play) HongjiEligibility(int owner, int recipient, long? parentId)
    {
        if (!HasHongjiCapability || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || recipient != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return (false, false);
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangShiPingHongjiGrantedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "ol:hongji").ToArray();
        if (parentId is { } parent && previous.Any(e => e.PreparationParentId == parent))
            return (false, false);
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == parentId);
        var counts = window?.ParticipantFacts is { Count: > 0 } frozen ? frozen.ToDictionary(p => p.Key, p => p.Value.CurrentHandCount) : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToDictionary(p => p.Seat, p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
        if (!counts.TryGetValue(recipient, out var hand))
            return (false, false);
        return (hand == counts.Values.Min() && !previous.Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.Phase == TurnPhase.Draw), hand == counts.Values.Max() && !previous.Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.Phase == TurnPhase.Play));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26610)]
    internal bool CanOfferZhangShiPing(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is [{ Op: SkillProgramEffectOp.ArmHongjiNextPhase }])
        {
            if (context.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
                return false;
            var eligible = HongjiEligibility(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, context.ParentFrameId > 0 ? context.ParentFrameId : null);
            return eligible.Draw || eligible.Play;
        }

        if (t.Effects is [{ Op: SkillProgramEffectOp.ResolveHongjiGrantedPhase }])
            return HasIssuedHongjiCandidate(c, context);
        if (t.Effects is [{ Op: SkillProgramEffectOp.PlaceXingGuMountAndObtainDiamond }])
            return XingGuPlacementChoices(c.OwnerSeat, c.SkillInstanceId).Count > 0;
        return true;
    }

    internal bool IsHongjiPhaseCandidate(ProgramTriggerCandidate c) => c.SkillId == "ol:hongji" && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.Triggers.Any(t => t.Id == c.BindingId && t.Effects is [{ Op: SkillProgramEffectOp.ResolveHongjiGrantedPhase }]) == true;
    internal IEnumerable<ZhangShiPingHongjiGrantedEvent> OutstandingHongjiPromises(int recipient, SkillProgramTriggerWindow window)
    {
        if (!HasHongjiCapability || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || recipient != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var phase = window == SkillProgramTriggerWindow.DrawPhaseEnded ? TurnPhase.Draw : TurnPhase.Play;
        if (window is not (SkillProgramTriggerWindow.DrawPhaseEnded or SkillProgramTriggerWindow.PlayEnding) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != phase)
            return[];
        var consumed = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangShiPingHongjiConsumedEvent>().Select(e => e.PromiseFrameId).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangShiPingHongjiGrantedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.RecipientSeat == recipient && e.Phase == phase && !consumed.Contains(e.FrameId) && !HongjiPromiseReservedByPhaseParent(e));
    }

    internal bool HongjiPromiseReservedByPhaseParent(ZhangShiPingHongjiGrantedEvent promise)
    {
        for (var schedule = _programPhaseScheduleOperations._programPhaseSchedule; schedule is { Frame.ZhangShiPingHongji: { Stage: HongjiStage.ExtraPhase } receipt }; schedule = receipt.SuspendedSchedule)
        {
            if (receipt.ActualTurn != promise.ActualTurn || receipt.RecipientSeat != promise.RecipientSeat || receipt.Phase != promise.Phase || schedule.ParentFrame is not { } parent || parent.Id != receipt.ParentWindowId || parent.OwnerSeat != promise.RecipientSeat || parent.Window != (promise.Phase == TurnPhase.Draw ? SkillProgramTriggerWindow.DrawPhaseEnded : SkillProgramTriggerWindow.PlayEnding))
                continue;
            if (parent.Candidates.Skip(parent.CandidateIndex).Any(c => c.OwnerSeat == promise.Source.OwnerSeat && c.SkillId == promise.Source.SkillId && c.SkillInstanceId == promise.Source.SkillInstanceId && c.GameplayHash == promise.GameplayHash && IsHongjiPhaseCandidate(c)))
                return true;
        }

        return false;
    }

    internal bool HasOutstandingHongjiPhase(CharacterState recipient, SkillProgramTriggerWindow window) => OutstandingHongjiPromises(recipient.Seat, window).Any();
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(CharacterState turnOwner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates)
    {
        if (!HasHongjiCapability)
            return candidates;
        var result = candidates.ToList();
        foreach (var promise in OutstandingHongjiPromises(turnOwner.Seat, window))
        {
            var p = _contentRegistryOperations._contentRegistry.GetSkill(promise.Source.SkillId).Program!;
            if (p.GameplayHash != promise.GameplayHash)
                continue;
            var t = p.Triggers.Single(t => t.Window == window && t.Effects is [{ Op: SkillProgramEffectOp.ResolveHongjiGrantedPhase }]);
            var candidate = new ProgramTriggerCandidate(promise.Source.OwnerSeat, promise.Source.SkillId, t.Id, promise.Source.SkillInstanceId, promise.GameplayHash, t.Priority);
            if (!result.Any(c => c.OwnerSeat == candidate.OwnerSeat && c.SkillId == candidate.SkillId && c.BindingId == candidate.BindingId && c.SkillInstanceId == candidate.SkillInstanceId))
                result.Add(candidate);
        }

        return Array.AsReadOnly(result.OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(turnOwner.Seat, c.OwnerSeat)).ThenByDescending(c => c.Priority).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.BindingId, StringComparer.Ordinal).ToArray());
    }

    internal bool HasIssuedHongjiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsHongjiPhaseCandidate(c) || context.Window is not (SkillProgramTriggerWindow.DrawPhaseEnded or SkillProgramTriggerWindow.PlayEnding) || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || context.OwnerSeat != c.OwnerSeat || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.Window != context.Window || window.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.BindingId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return false;
        bool Matches(ZhangShiPingHongjiGrantedEvent p) => p.Source.OwnerSeat == c.OwnerSeat && p.Source.SkillId == c.SkillId && p.Source.SkillInstanceId == c.SkillInstanceId && p.GameplayHash == c.GameplayHash && p.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && p.RecipientSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && p.Phase == (context.Window == SkillProgramTriggerWindow.DrawPhaseEnded ? TurnPhase.Draw : TurnPhase.Play);
        return OutstandingHongjiPromises(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, context.Window).Any(Matches) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OwnerSeat == c.OwnerSeat && f.SkillId == c.SkillId && f.SkillInstanceId == c.SkillInstanceId && f.WindowContext == context && f.ZhangShiPingHongji is { Promise: { } p } r && r.ParentWindowId == window.Id && Matches(p) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangShiPingHongjiConsumedEvent>().Any(e => e.PromiseFrameId == p.FrameId && e.FrameId == f.Id && e.ParentWindowId == window.Id));
    }

    internal SkillProgramStepOutcome BeginHongjiPromise(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "ol:hongji" || f.InstructionIndex != 1 || f.ZhangShiPingHongji is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            throw new InvalidOperationException("Hongji requires its exact actual preparation source and zero-input instruction.");
        var eligible = HongjiEligibility(f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, context.ParentFrameId);
        if (!eligible.Draw && !eligible.Play)
            throw new InvalidOperationException("Hongji no longer has an unused qualifying preparation branch.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangShiPingHongji = new(ZhangShiPingSource(f), f.GameplayHash, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, HongjiStage.Choose, eligible.Draw, eligible.Play) });
        PublishHongji(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HongjiChoices(ProgramSkillFrame f)
    {
        var r = f.ZhangShiPingHongji!;
        var choices = new List<PromptChoice>();
        void Add(TurnPhase phase, string branch, string label) => choices.Add(new(new($"hongji.{(f.Id)}.{(branch)}"), label, [], [r.RecipientSeat], new Dictionary<string, string> { ["program-action"] = "zhang-shi-ping-hongji", ["branch"] = branch, ["phase"] = phase.ToString() }));
        if (r.DrawEligible)
            Add(TurnPhase.Draw, "draw", "下个实际摸牌阶段结束后执行额外摸牌阶段");
        if (r.PlayEligible)
            Add(TurnPhase.Play, "play", "下个实际出牌阶段结束后执行额外出牌阶段");
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishHongji(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = HongjiChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "鸿济：选择本次准备阶段的一项效果。", [], [f.ZhangShiPingHongji!.RecipientSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhang-shi-ping-hongji")]
    internal void AnswerHongji(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangShiPingHongji: { Stage: HongjiStage.Choose } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HongjiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Hongji lost its exact frozen preparation choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var phase = Enum.Parse<TurnPhase>(choice.Parameters["phase"]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingHongjiGrantedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, _coreActualRoundProgressRoundNumberAccess._roundNumber, r.RecipientSeat, phase, r.ParentWindowId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ZhangShiPingHongji = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
    }

    internal SkillProgramStepOutcome BeginHongjiExtraPhase(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhangShiPingHongji is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || !HasIssuedHongjiCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), context))
            throw new InvalidOperationException("Hongji requires its exact unconsumed issued actual-phase candidate.");
        var p = OutstandingHongjiPromises(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, context.Window).Single(p => p.Source.OwnerSeat == f.OwnerSeat && p.Source.SkillId == f.SkillId && p.Source.SkillInstanceId == f.SkillInstanceId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangShiPingHongji = new(p.Source, f.GameplayHash, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, p.RecipientSeat, HongjiStage.ExtraPhase, Phase: p.Phase, ReturnPhase: _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase, ReturnActualPhase: _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase(), Promise: p) { SuspendedSchedule = _programPhaseScheduleOperations._programPhaseSchedule } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingHongjiConsumedEvent(p.FrameId, f.Id, p.Source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, p.RecipientSeat, p.Phase, context.ParentFrameId));
        if (!TryBeginZhangShiPingHongjiPhase(f, p.Phase, p.RecipientSeat))
            throw new InvalidOperationException("Hongji could not suspend its exact native phase-ending parent.");
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ResumeZhangShiPingHongjiPhase(long frameId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (f.ZhangShiPingHongji is not { Stage: HongjiStage.ExtraPhase, Promise: { } p } r || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("Hongji phase return lost its exact consumed promise.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingHongjiReturnedEvent(p.FrameId, frameId, r.ActualTurn, r.RecipientSeat, r.Phase));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangShiPingHongji = r with { Stage = HongjiStage.Returned } });
        _coreSkillProgramsOperations.FinishProgramSkill(f with { ZhangShiPingHongji = null }, completed: true);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26610)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26610)]
    internal bool ContinueIssuedZhangShiPing(ProgramSkillFrame f) => f.ZhangShiPingXingGu is not null || f.ZhangShiPingHongji is { Stage: HongjiStage.ExtraPhase or HongjiStage.Returned } || f.ZhangShiPingHongji is null && f.TriggerId is { } binding && f.InstructionIndex is 0 or 1 && f.WindowContext is { } context && HasIssuedHongjiCandidate(new(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, 0), context);
    [ProgramAiChoice(SkillProgramEffectOp.ArmHongjiNextPhase)]
    internal PromptChoice AiHongji(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "play" && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.ZhangShiPingHongji!.RecipientSeat]).Count >= 4) ?? p.Choices.First();
    [ProgramIssuedTrigger(670, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger670(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsHongjiPhaseCandidate(candidate) ? HasIssuedHongjiCandidate(candidate, context) : null;
}

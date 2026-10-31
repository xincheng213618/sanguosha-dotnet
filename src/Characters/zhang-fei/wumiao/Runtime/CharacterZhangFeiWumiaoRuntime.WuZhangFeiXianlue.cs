#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Numerics;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiWumiaoRuntime
{
    internal const string WuZhangFeiXianlue = "wumiao:xianlue";
    internal const string WuZhangFeiXianlueDrawReason = "skill-program.wu-zhang-fei.xianlue.draw";
    internal bool HasWuZhangFeiXianlueCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWuZhangFeiXianlue);

    internal static int WuZhangFeiRankCount(int mask) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_fei_wumiao_WuZhangFeiXianlue.WuZhangFeiRankCount(mask);
    internal WuZhangFeiRankCycleChangedEvent WuZhangFeiCycle(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiRankCycleChangedEvent>().LastOrDefault(e => e.OwnerSeat == owner) ?? new(owner, 1, 0, 13, 0, "initial", 0);
    internal bool CanObserveWuZhangFeiHand(int owner, int target) => HasWuZhangFeiXianlueCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && target != owner && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiXianlueStartedEvent>().Any(e => e.Source.OwnerSeat == owner && e.TargetSeat == target && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) && _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).Any(e => e is DamageAppliedEvent { Amount: > 0 } damage && damage.TargetSeat == target || e is CardMovedEvent move && move.From.OwnerSeat == target && move.From != move.To);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 29061, SkillProgramEffectOp.ResolveWuZhangFeiXianlue)]
    internal bool CanStartWuZhangFeiXianlue(ProgramActivationLegalityContext gate) => gate.SkillId == WuZhangFeiXianlue && gate.Activation.Id == "observe-ranks" && _coreMatchParticipantsPlayersAccess._players.Any(target => CanObserveWuZhangFeiHand(gate.OwnerSeat, target.Seat));
    internal SkillProgramStepOutcome BeginWuZhangFeiXianlue(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SkillId != WuZhangFeiXianlue || f.TriggerId is not null || f.ActivationId != "observe-ranks" || f.InstructionIndex != 1 || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || !CanObserveWuZhangFeiHand(f.OwnerSeat, target) || f.WuZhangFeiXianlue is not null || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1)
            throw new InvalidOperationException("Xianlue requires one currently eligible other Hand and its unused actual Play target.");
        var cycle = WuZhangFeiCycle(f.OwnerSeat);
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(card => new WuZhangFeiXianlueObservedEntity(card.Id, card.Kind, _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[target], card))).ToArray();
        var observed = hand.Where(card => card.Rank is >= 1 and <= 13).Aggregate(0, (mask, card) => mask | 1 << (card.Rank - 1));
        var afterMask = cycle.RankMask | observed;
        var count = WuZhangFeiRankCount(afterMask);
        var beforeCount = WuZhangFeiRankCount(cycle.RankMask);
        var earned = 0;
        for (var gate = 0; gate < 3; gate++)
            if ((cycle.PaidGates & 1 << gate) == 0 && beforeCount <= 3 * (gate + 1) && count > 3 * (gate + 1))
                earned |= 1 << gate;
        var receipt = new WuZhangFeiXianlueReceipt(WuZhangFeiSource(f), f.GameplayHash, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, cycle.CycleId, cycle.RankMask, observed, earned, WuZhangFeiXianlueStage.Observing)
        {
            Hand = hand
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhangFeiXianlue = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiXianlueStartedEvent(f.Id, receipt.Source, f.GameplayHash, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, receipt.PhaseInstance, cycle.CycleId, cycle.RankMask, hand.Length));
        // Claim each gate before any draw child. Completion itself follows the
        // awarded draws, unless a real nested Dying independently clears it.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(cycle with { RankMask = afterMask, PaidGates = cycle.PaidGates | earned, Reason = "observe", ParentId = f.Id });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiXianlueObservedEvent(f.Id, f.OwnerSeat, target, cycle.CycleId, cycle.RankMask, observed & ~cycle.RankMask, earned));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = WuZhangFeiObservedChoices(f);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"显略：已观看 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的手牌，当前记录 {(count)}/{(cycle.CompletionTarget)} 种点数。", [], [target], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> WuZhangFeiObservedChoices(ProgramSkillFrame f) => [new(new($"wu-zhang-fei.observed.{(f.Id)}"), "已观看，继续结算。", [], [f.WuZhangFeiXianlue!.TargetSeat], new Dictionary<string, string> { ["program-action"] = "wu-zhang-fei-observed", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })];
    [ProgramAnswer("wu-zhang-fei-observed")]
    internal void AnswerWuZhangFeiObserved(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (f.WuZhangFeiXianlue is not { Stage: WuZhangFeiXianlueStage.Observing } r || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WuZhangFeiObservedChoices(f)))
            throw new InvalidOperationException("Xianlue observation must return through its original private acknowledgement.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuZhangFeiXianlue = r with { Stage = WuZhangFeiXianlueStage.Drawing } });
        ContinueWuZhangFeiXianlue(f.Id);
    }

    internal void ContinueWuZhangFeiXianlue(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.WuZhangFeiXianlue!;
        if (r.Stage == WuZhangFeiXianlueStage.Observing)
            return;
        if (r.Stage == WuZhangFeiXianlueStage.DrawChildren)
        {
            if (AwaitWuZhangFeiChildren(f.Id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.WuZhangFeiXianlue!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhangFeiXianlue = r with { Stage = WuZhangFeiXianlueStage.Drawing, ReturnedGates = r.ReturnedGates | 1 << r.CurrentGate!.Value, CurrentGate = null, DrawOutcome = null } });
            r = f.WuZhangFeiXianlue!;
        }

        var next = Enumerable.Range(0, 3).Where(gate => (r.EarnedGates & 1 << gate) != 0 && (r.ReturnedGates & 1 << gate) == 0).Select(gate => (int? )gate).FirstOrDefault();
        if (next is { } gate && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            if (r.CurrentGate is null)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhangFeiXianlue = r with { CurrentGate = gate } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiXianlueDrawIssuedEvent(f.Id, r.OriginCycleId, gate));
            }

            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 3, new(WuZhangFeiXianlueDrawReason));
            return;
        }

        var cycle = WuZhangFeiCycle(f.OwnerSeat);
        if (cycle.CycleId == r.OriginCycleId && WuZhangFeiRankCount(cycle.RankMask) >= cycle.CompletionTarget)
            CompleteWuZhangFeiCycle(cycle, f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiXianlueCompletedEvent(f.Id, r.OriginCycleId));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWuZhangFeiXianlue)]
    internal void ReturnWuZhangFeiXianlueDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.WuZhangFeiXianlue is not { Stage: WuZhangFeiXianlueStage.Drawing, CurrentGate: { } gate } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 3 || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiXianlueDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Gate == gate))
            throw new InvalidOperationException("Xianlue cannot repeat a claimed gate or substitute another native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhangFeiXianlue = r with { Stage = WuZhangFeiXianlueStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiXianlueDrawReturnedEvent(f.Id, r.OriginCycleId, gate, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueWuZhangFeiXianlue(f.Id);
    }

    internal void CompleteWuZhangFeiCycle(WuZhangFeiRankCycleChangedEvent cycle, long parent)
    {
        if (WuZhangFeiCycle(cycle.OwnerSeat) != cycle || WuZhangFeiRankCount(cycle.RankMask) < cycle.CompletionTarget)
            throw new InvalidOperationException("Xianlue may clear only the currently completed original cycle.");
        var before = _skillRuntimeStateOperations._skillRuntimeState.GetUsage(cycle.OwnerSeat, "wumiao:haoxian", "collect-rank-three", SkillUsageScope.Game);
        _skillRuntimeStateOperations._skillRuntimeState.ClearUsage(cycle.OwnerSeat, "wumiao:haoxian", "collect-rank-three", SkillUsageScope.Game);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiHaoxianResetEvent(cycle.OwnerSeat, cycle.CycleId, parent, "collect-rank-three", before, 0));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiRankCycleChangedEvent(cycle.OwnerSeat, checked(cycle.CycleId + 1), 0, 13, 0, "complete", parent));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29061)]
    internal void ObserveWuZhangFeiDying(IGameEvent payload)
    {
        if (!HasWuZhangFeiXianlueCapability || payload is not PlayerDyingEvent fact || fact.KillerSeat is not { } owner || fact.VictimSeat == owner || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[owner], WuZhangFeiXianlue) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DyingFrame { Continuation: DyingContinuationKind.Damage } dying || dying.Id != fact.ResolutionId || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(candidate => candidate.Id == dying.ParentFrameId)is not { Amount: > 0 } damage || damage.SourceSeat != owner || damage.TargetSeat != fact.VictimSeat || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageWasApplied: true } attack || attack.SourceSeat != owner || attack.TargetSeat != fact.VictimSeat || attack.ResolutionId != damage.ParentFrameId || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiDyingReducedEvent>().Any(e => e.DyingFrameId == dying.Id))
            return;
        var cycle = WuZhangFeiCycle(owner);
        var after = Math.Max(1, cycle.CompletionTarget - 3);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiDyingReducedEvent(dying.Id, damage.Id, owner, fact.VictimSeat, cycle.CycleId, cycle.CompletionTarget, after));
        var reduced = cycle with
        {
            CompletionTarget = after,
            Reason = "dying",
            ParentId = dying.Id
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(reduced);
        if (WuZhangFeiRankCount(reduced.RankMask) >= after)
            CompleteWuZhangFeiCycle(reduced, dying.Id);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 100)]
    internal CardSnapshot[] WuZhangFeiPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer).SelectMany(f => f.WuZhangFeiUse is { Stage: WuZhangFeiUseStage.Selecting } sample ? sample.Sample.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))) : f.WuZhangFeiXianlue is { Stage: WuZhangFeiXianlueStage.Observing } observed ? observed.Hand.Select(card => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[observed.TargetSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId))) : []).ToArray();
    internal string? WuZhangFeiPublicCycleDescription(int owner, IReadOnlyList<GeneralSkillDefinition>? primary, IReadOnlyList<GeneralSkillDefinition>? secondary)
    {
        if (!HasWuZhangFeiXianlueCapability || !(primary ?? []).Concat(secondary ?? []).Any(skill => skill.ContentId == WuZhangFeiXianlue))
            return null;
        var cycle = WuZhangFeiCycle(owner);
        return $"显略：第{(cycle.CycleId)}轮，已记录{(WuZhangFeiRankCount(cycle.RankMask))}/{(cycle.CompletionTarget)}种点数。";
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuZhangFeiXianlue)]
    internal PromptChoice SelectAiWuZhangFeiObserved(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.Single();
    SkillProgramStepOutcome global::CardGame.Core.IWuZhangFeiXianlueHost.Observe(ProgramSkillFrame frame) => BeginWuZhangFeiXianlue(frame);
    bool global::CardGame.Core.IWuZhangFeiXianlueSelection.CanObserve(int ownerSeat, int targetSeat) => CanObserveWuZhangFeiHand(ownerSeat, targetSeat);
}

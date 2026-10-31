#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualDiscardPhaseRuntime
{
    internal void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false)
    {
        if ((_corePhaseHandSeizureOperations.HasCurrentIssuedPhaseHandDebt() || _characterLvQianOlOperations.HasCurrentIssuedWeiluDebt(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat])) && _coreProgramLifecycleOperations.TryBeginPlayEndingProgramWindow(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]))
            return;
        _characterCaoChunOlOperations.ExpireCaoChunPlayDebt(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        if (_characterQiaoZhouOlOperations.TrySkipQiaoZhouDiscardPhase(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]))
        {
            _coreActualTurnsOperations.EndTurn();
            return;
        }

        if (_characterDuXiOlOperations.TrySkipDuXiDiscardPhase(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]))
        {
            _coreActualTurnsOperations.EndTurn();
            return;
        }

        if (!beforeEntryPolicyCompleted && _characterLuMengMobileOperations.TryBeginPreEntryDiscardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]))
            return;
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase = TurnPhase.Discard;
        _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Discard, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn = -1;
        _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Clear();
        _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber = -1;
        _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds.Clear();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        var current = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat];
        _coreMatchLogOperations.AddLog("PhaseChanged", $"{(current.Name)} 进入弃牌阶段。", _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseChangedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat));
        if (_coreProgramLifecycleOperations.TryBeginDiscardPhaseProgramWindow(current))
            return;
        CompleteDiscardPhaseAfterProgramWindow(current);
    }

    internal void CompleteDiscardPhaseAfterProgramWindow(CharacterState current)
    {
        // A paid starting-window child may already have ended the game and
        // retired its actual phase. Its native parent only needs to close.
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        if (_coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDiscardPhase))
        {
            if (_programPipelineHooksOperations._programPipelineHooks.TryBeginPhaseSkipped(new(current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, SkillProgramTurnPhase.Discard, (int)_coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects)))
                return;
            _coreMatchLogOperations.AddLog("DelayedCardEffect", $"{(current.Name)} 跳过弃牌阶段。", current.Seat);
            _coreActualTurnsOperations.EndTurn();
            return;
        }

        if (!_coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess._usedOrPlayedSlashDuringPlayPhase && _coreProgramCardPoliciesOperations.CardPolicies(current, SkillProgramCardPolicyKind.OfferSkipDiscard).Any(item => item.Policy.BeforePhaseEntry != true))
        {
            BeginSkipDiscardPolicyChoice(current);
        }
        else
            _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Discard, current.Seat);
    }

    internal void BeginSkipDiscardPolicyChoice(CharacterState current)
    {
        var(source, policy) = _coreProgramCardPoliciesOperations.CardPolicies(current, SkillProgramCardPolicyKind.OfferSkipDiscard).First(item => item.Policy.BeforePhaseEntry != true);
        var skillName = source.Definition.Name;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.SkipDiscardPolicy, current.Seat, $"是否发动【{(skillName)}】，跳过本回合的弃牌阶段？", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = [new PromptChoice(new ChoiceId($"discard-policy.{(source.SkillId)}.{(policy.Id)}.use"), $"发动【{(skillName)}】，跳过弃牌阶段并保留全部手牌。", [], [], new Dictionary<string, string> { ["action"] = "discard-policy-use", ["skill-id"] = source.SkillId, ["policy-id"] = policy.Id }), new PromptChoice(new ChoiceId($"discard-policy.{(source.SkillId)}.{(policy.Id)}.skip"), $"不发动【{(skillName)}】，按体力上限弃牌。", [], [], new Dictionary<string, string> { ["action"] = "discard-policy-skip", ["skill-id"] = source.SkillId, ["policy-id"] = policy.Id })]
        };
        _coreMatchOutcomeStatusWinnerAccess._status = current.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveSkipDiscardPolicyChoice(bool useSkill)
    {
        if (_characterLuMengMobileOperations.TryResolvePreEntryDiscardPolicy(useSkill))
            return;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.SkipDiscardPolicy, PlayerSeat: var playerSeat } || playerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard)
        {
            throw new InvalidOperationException("There is no discard-phase policy choice to resolve.");
        }

        var current = _coreMatchParticipantsPlayersAccess._players[playerSeat];
        var metadata = _corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices[0].Parameters;
        var skillId = metadata["skill-id"];
        var policyId = metadata["policy-id"];
        if (!_coreProgramCardPoliciesOperations.CardPolicies(current, SkillProgramCardPolicyKind.OfferSkipDiscard).Any(item => item.Policy.BeforePhaseEntry != true && item.Source.SkillId == skillId && item.Policy.Id == policyId) || _coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess._usedOrPlayedSlashDuringPlayPhase)
            throw new InvalidOperationException("The chosen discard policy is no longer enabled.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardPolicyResolvedEvent(current.Seat, skillId, policyId, useSkill));
        _coreMatchLogOperations.AddLog(useSkill ? "SkillTriggered" : "SkillSkipped", useSkill ? $"{(current.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(skillId).Name)}】，跳过弃牌阶段。" : $"{(current.Name)} 未发动【{(_contentRegistryOperations._contentRegistry.GetSkill(skillId).Name)}】。", current.Seat);
        if (useSkill)
        {
            if (_characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(0, current.Seat, skillId, $"discard-policy:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:{(policyId)}", returnKind: SkillInvocationReturnKind.SkipDiscard))
                return;
            _coreActualTurnsOperations.EndTurn();
        }
        else
            _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Discard, current.Seat);
    }

    internal void AutoDiscard(CharacterState player)
    {
        var hand = _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableHand(player);
        var handLimit = _coreNativeRuleQueriesOperations.GetHandLimit(player);
        var count = _coreTurnHandCategoryRestrictionsOperations.RequiredSelfHandLimitDiscard(player);
        if (count == 0)
        {
            return;
        }

        var discarded = hand.OrderBy(card => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, player)).ThenBy(card => card.Id).Take(count).ToArray();
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(player.Seat, player.Seat, discarded, CardLocation.Hand(player.Seat), CardMoveReasons.HandLimitDiscard);
        _coreMatchLogOperations.AddLog("CardsDiscarded", $"{(player.Name)} 自动弃置 {(discarded.Length)} 张手牌。", player.Seat);
    }
}

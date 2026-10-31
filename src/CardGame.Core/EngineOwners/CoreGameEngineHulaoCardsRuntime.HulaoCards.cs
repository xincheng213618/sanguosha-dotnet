#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHulaoCardsRuntime
{
    internal void AddHulaoAllianceFeastActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> playableCards)
    {
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.AllianceFeast, CardActionType.Use) || _coreMatchParticipantsPlayersAccess._players.Any(target => target.IsAlive && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, CardKind.AllianceFeast) && _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, CardKind.AllianceFeast)))
            return;
        foreach (var card in playableCards.Where(card => card.Kind == CardKind.AllianceFeast))
            actions.Add(new LegalAction(LegalActionKind.AllianceFeast, card.Id, null, "使用【联军盛宴】：你摸存活角色数张牌，其他角色各选择回复1点体力或摸1张牌"));
    }

    internal void ResolveAllianceFeast(CharacterState source, Card card)
    {
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: card.Id).Any(action => action.Kind == LegalActionKind.AllianceFeast && action.CardId == card.Id && action.TargetSeat is null))
            throw new InvalidOperationException("Alliance feast became illegal before native resolution.");
        var targets = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(source, Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(target => target.IsAlive && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(source.Seat, target.Seat, CardKind.AllianceFeast) && !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.AllianceFeast) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.AllianceFeast, card.Suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(source, card))).Select(target => target.Seat).ToArray());
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, targets);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, source.Seat, targets, LegalActionKind.AllianceFeast);
    }

    internal void ResolveAllianceFeastEffect(NullificationWindowFrame pending)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId) ?? throw new InvalidOperationException("Alliance feast lost its native card use.");
        var targetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("Alliance feast requires one exact nullification target.");
        if (global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.AllianceFeast || use.Action is not { } action || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != targetSeat || use.AllianceFeastChoice is not null)
            throw new InvalidOperationException("Alliance feast lost its native target cursor.");
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        if (!_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
        {
            CompleteHulaoAllianceFeastTarget(use.Id);
            return;
        }

        if (targetSeat == use.SourceSeat)
        {
            var amount = _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive);
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[targetSeat], amount, log: true);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HulaoAllianceFeastBenefitEvent(use.Id, use.SourceSeat, targetSeat, "draw", amount));
            CompleteHulaoAllianceFeastTarget(use.Id);
            return;
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { AllianceFeastChoice = new(targetSeat, use.TargetIndex, action.ActionId) });
        var choices = new[]
        {
            "recover",
            "draw"
        }.Select(benefit => new PromptChoice(new ChoiceId($"hulao.alliance-feast.{(use.Id)}.{(use.TargetIndex)}.{(benefit)}"), benefit == "recover" ? "回复 1 点体力" : "摸 1 张牌", [], [], new Dictionary<string, string> { ["program-action"] = "hulao-alliance-feast", ["frame-id"] = use.Id.ToString(CultureInfo.InvariantCulture), ["target-index"] = use.TargetIndex.ToString(CultureInfo.InvariantCulture), ["benefit"] = benefit })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, targetSeat, "【联军盛宴】请选择回复 1 点体力或摸 1 张牌。", [], [], use.SourceSeat, CardKind.AllianceFeast)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = targetSeat,
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[targetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool HasHulaoAllianceFeastPrompt() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IncomingCard: CardKind.AllianceFeast } decision && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { AllianceFeastChoice: { } choice } use && global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.AllianceFeast && use.TargetIndex == choice.TargetIndex && global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] == choice.TargetSeat && decision.PlayerSeat == choice.TargetSeat;
    [ProgramAnswer("hulao-alliance-feast")]
    internal void ResolveHulaoAllianceFeastChoice(PromptChoice selected)
    {
        if (!HasHulaoAllianceFeastPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { AllianceFeastChoice: { } choice } use || use.Action?.ActionId != choice.ActionId || selected.Cards.Count != 0 || selected.Targets.Count != 0 || selected.Parameters.GetValueOrDefault("frame-id") != use.Id.ToString(CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("target-index") != use.TargetIndex.ToString(CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("benefit")is not ("recover" or "draw") || !_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Any(option => option.Id == selected.Id))
            throw new InvalidOperationException("Alliance feast choice must name its exact native target and current prompt.");
        var benefit = selected.Parameters["benefit"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { AllianceFeastChoice = null });
        var target = _coreMatchParticipantsPlayersAccess._players[choice.TargetSeat];
        if (target.IsAlive && benefit == "draw")
            _corePhysicalDrawFacadesOperations.DrawCards(target, 1, log: true);
        else if (target.IsAlive && target.HasHpValue && target.Hp < target.MaxHp && benefit == "recover")
        {
            var amount = _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(use.Id, use.SourceSeat, target.Seat, 1);
            if (!_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(use.Id, use.SourceSeat, target.Seat, amount, new(RecoveryAttemptProducer.CardUse, CardId: use.CardId, CardKind: global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), Policies: [])))
            {
                var recoveryId = _coreNativeRecoveryFramesOperations.BeginRecovery(use.Id, use.SourceSeat, target.Seat, amount);
                try
                {
                    var actual = Math.Min(amount, target.MaxHp - target.Hp);
                    target.Hp += actual;
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(use.SourceSeat, target.Seat, actual, target.Hp));
                    _coreMatchLogOperations.AddLog("Recovered", $"{(target.Name)} 因【联军盛宴】回复 {(actual)} 点体力。", use.SourceSeat, target.Seat);
                }
                finally
                {
                    _coreRuntimeOperations.PopResolutionFrame(recoveryId, ResolutionFrameKind.Recovery);
                }
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HulaoAllianceFeastBenefitEvent(use.Id, use.SourceSeat, target.Seat, benefit, 1));
        CompleteHulaoAllianceFeastTarget(use.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 27500)]
    internal bool TryAdvanceHulaoAllianceFeastAi()
    {
        if (!HasHulaoAllianceFeastPrompt() || _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat].IsHuman)
            return false;
        var target = _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision.PlayerSeat];
        var benefit = target.HasHpValue && target.Hp < target.MaxHp ? "recover" : "draw";
        ResolveHulaoAllianceFeastChoice(_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("benefit") == benefit));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal void CompleteHulaoAllianceFeastTarget(long useId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Alliance feast lost its owner.");
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(useId, use.CardId, requireProcessing: true);
        // FinishCardUse drains native recovery replacements and HP observers before
        // its existing sequential target cursor advances. The physical trick stays
        // in Processing until every surviving target has resolved or nullified.
        _coreNativeCardUseCompletionOperations.FinishCardUse(useId, card, CardKind.AllianceFeast);
    }

    internal void AssertHulaoCardState()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(use => use.AllianceFeastChoice is not null))
        {
            var choice = use.AllianceFeastChoice!;
            if (global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.AllianceFeast || use.Action?.ActionId != choice.ActionId || use.TargetIndex != choice.TargetIndex || use.TargetIndex < 0 || use.TargetIndex >= global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != choice.TargetSeat || !HasHulaoAllianceFeastPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Count != 2 || _corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.Any(option => option.Parameters.GetValueOrDefault("program-action") != "hulao-alliance-feast"))
                throw new InvalidOperationException("Alliance feast lost its exact native target choice.");
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.IncomingCard == CardKind.AllianceFeast && _corePendingDecisionPolicyPipelineOperations._pendingDecision.Kind == DecisionKind.ProgramTrigger && !HasHulaoAllianceFeastPrompt())
            throw new InvalidOperationException("Alliance feast has a choice without its owning card use.");
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHandGuidanceRuntime
{
    internal HumanActionView GetHumanActionView()
    {
        var actions = _coreNativeLegalActionsOperations.GetHumanLegalActions();
        return new HumanActionView(_coreCommandSessionQueriesOperations.Revision, actions, BuildHumanHandGuidance(actions), _coreWushuangJinzhiOperations.HumanWushuangJinzhiMaterials(), _coreMatchDefinitionOptionsAccess._options.HumanSeat >= 0 && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat]));
    }

    internal IReadOnlyList<HandCardGuidance> BuildHumanHandGuidance(IReadOnlyList<LegalAction> legalActions)
    {
        if (_coreMatchDefinitionOptionsAccess._options.HumanSeat < 0)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat];
        var actions = legalActions.Where(action => action.CardId is not null).ToLookup(action => action.CardId!.Value);
        var ownPrompt = _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat == actor.Seat ? _corePendingDecisionPolicyPipelineOperations._pendingDecision : null;
        var skillNames = _coreNationalSkillsOperations.EnabledSkillNames(actor);
        return _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Select(card =>
        {
            HandCardGuidance Hint(HandGuidanceReason reason, string message, bool playable = false) => new(card.Id, playable, reason, message);
            if (_coreMatchOutcomeStatusAccess._status == EngineStatus.Completed)
                return Hint(HandGuidanceReason.GameFinished, "本局已经结束，可在战报中回顾对局。");
            if (ownPrompt?.Kind == DecisionKind.DiscardCards)
                return Hint(HandGuidanceReason.SelectDiscard, "点击选为弃牌；选够指定数量后，再确认弃置。");
            if (ownPrompt is not null && ownPrompt.Kind != DecisionKind.PlayCard)
                return Hint(HandGuidanceReason.ResolveCurrentPrompt, "请在中央完成当前响应；候选按钮会列出可用的牌、技能或放弃选项。");
            if (ownPrompt?.Kind != DecisionKind.PlayCard)
                return Hint(HandGuidanceReason.WaitingForTurn, "现在不是你的出牌阶段；需要响应时，中央会显示可选操作。");
            var cardActions = actions[card.Id].ToArray();
            var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).FirstOrDefault();
            if (cardActions.Length > 0)
            {
                if (cardActions.Any(action => action.Kind == LegalActionKind.Recast))
                    return Hint(HandGuidanceReason.Playable, "可选择一到两名存活角色切换连环，或不选目标，点击重铸换一张牌。", true);
                if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) && cardActions.All(action => action.PlayedCardKind == CardKind.Slash))
                    return Hint(HandGuidanceReason.ConversionOnly, $"当前可通过【{(skillNames)}】当作杀使用；选择目标后确认转化。", true);
                return Hint(HandGuidanceReason.Playable, cardActions.All(action => action.TargetSeats.All(seat => seat == actor.Seat)) ? "当前可以使用；点击选中，再确认出牌。" : "当前可以使用；选中后，合法目标或目标组合会亮起。", true);
            }

            if (_coreWenhePoisonOperations.IsWenhePhysicalPoison(actor, card))
                return Hint(HandGuidanceReason.NoLegalTarget, "本轮这张实体桃视为毒，不能当桃使用或救援；正面朝上离开手牌区时失去1点体力。");
            if (identity is null && card.Kind == CardKind.Peach && !_coreTreatedWoundedOperations.IsRuleWounded(actor))
                return Hint(HandGuidanceReason.HealthFull, "你的体力已满，当前不能用桃回复；保留后可在濒死时救援。");
            if ((global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) || identity is not null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(identity.Identity.OutputKind)) && _coreCurrentTurnStatisticsSlashCountThisTurnAccess._slashCountThisTurn >= _coreRuleQueriesOperations.GetSlashUseLimit(actor) && !_coreDirectedCardPolicyQueriesOperations.HasSlashAllowanceForAnyTarget(actor, _coreProgramCardUseRulesOperations.GetSlashUseVariants2(actor, identity?.Identity.OutputKind ?? card.Kind).Select(variant => variant.EffectiveKind)))
                return Hint(HandGuidanceReason.SlashLimitReached, "本回合可使用杀的次数已用完；可以保留它用于响应或下个回合。");
            if (identity is null && card.Kind == CardKind.Alcohol && actor.HasAlcoholEffect)
                return Hint(HandGuidanceReason.AlcoholAlreadyActive, "你已有酒的效果，等待下一张杀消耗加伤；当前不能叠加饮酒。");
            if (card.Kind is CardKind.Dodge or CardKind.Nullification)
                return Hint(HandGuidanceReason.ResponseOnly, card.Kind == CardKind.Dodge ? "闪用于受到杀或万箭齐发时响应；当前没有可用的主动转化方式。" : "无懈可击用于锦囊响应窗口，不能在出牌阶段主动打出。");
            return Hint(HandGuidanceReason.NoLegalTarget, "当前没有符合规则的目标；可在图鉴查看这张牌的作用，或使用其他牌。");
        }).ToArray();
    }
}

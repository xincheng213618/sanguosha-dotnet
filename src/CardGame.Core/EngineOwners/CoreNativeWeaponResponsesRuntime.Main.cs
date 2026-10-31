#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeWeaponResponsesRuntime
{
    internal bool TryBeginCixiongDoubleSwordsChoice(NativeOwners.CardAttackHandle attack)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!_coreNativeDamageHandlesOperations.Value.UsesFormalCixiongDoubleSwords || attack.CixiongDoubleSwordsResolved || !source.IsAlive || !target.IsAlive || source.Gender == target.Gender || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.CixiongDoubleSwords))
        {
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is not null)
        {
            throw new InvalidOperationException("Only one Cixiong Double Swords choice may be active.");
        }

        _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords = _kernelCoreMainOperations.CreateCixiongDoubleSwordsHandle(attack);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.CixiongDoubleSwords, source.Seat, $"你对异性角色 {(target.Name)} 使用了【杀】，是否发动【雌雄双股剑】？", [], [], source.Seat, attack.EffectiveCardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = target.Seat,
            Choices = [new PromptChoice(new ChoiceId("cixiong.activate"), $"发动【雌雄双股剑】，令 {(target.Name)} 弃一张手牌或令你摸一张牌。", [], [], new Dictionary<string, string> { ["action"] = "cixiong-use" }), new PromptChoice(new ChoiceId("cixiong.skip"), "不发动【雌雄双股剑】。", [], [], new Dictionary<string, string> { ["action"] = "cixiong-skip" })]
        };
        _coreMatchOutcomeStatusAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return true;
    }

    internal void ResolveCixiongDoubleSwordsChoice(PromptChoice selected)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords ?? throw new InvalidOperationException("There is no Cixiong Double Swords choice to resolve.");
        var attack = pending.Attack;
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.CixiongDoubleSwords } decision || !selected.Parameters.TryGetValue("action", out var action))
        {
            throw new InvalidOperationException("The Cixiong Double Swords continuation is inconsistent.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (pending.Stage == CixiongDoubleSwordsStage.SourceActivation)
        {
            if (decision.PlayerSeat != source.Seat || action is not ("cixiong-use" or "cixiong-skip"))
            {
                throw new InvalidOperationException("The Cixiong activation choice is not current.");
            }

            if (action == "cixiong-skip")
            {
                CompleteCixiongDoubleSwords(pending, activated: false, targetDiscarded: false, discardedCardId: null, sourceDrawCount: 0);
                return;
            }

            pending.Stage = CixiongDoubleSwordsStage.TargetChoice;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (_coreNativeEquipmentInvocationsOperations.TryBeginCixiongEquipmentInvocation(pending))
                return;
            _coreNativeEquipmentInvocationsOperations.PublishCixiongAcceptedTargetChoice(pending);
            return;
        }

        if (decision.PlayerSeat != target.Seat || pending.Stage != CixiongDoubleSwordsStage.TargetChoice)
        {
            throw new InvalidOperationException("The Cixiong target choice is not current.");
        }

        if (action == "cixiong-discard")
        {
            if (selected.Cards.Count != 1)
            {
                throw new InvalidOperationException("Cixiong requires exactly one discarded hand card.");
            }

            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(target).SingleOrDefault(candidate => candidate.Id == selected.Cards[0]) ?? throw new InvalidOperationException("The selected Cixiong discard is no longer in hand.");
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(source.Seat, target.Seat, card, CardLocation.Hand(target.Seat), CardMoveReasons.CixiongDiscard, parentFrameId: attack.ResolutionId);
            CompleteCixiongDoubleSwords(pending, activated: true, targetDiscarded: true, discardedCardId: card.Id, sourceDrawCount: 0);
            return;
        }

        if (action != "cixiong-draw" || selected.Cards.Count != 0)
        {
            throw new InvalidOperationException("The Cixiong target choice is invalid.");
        }

        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(source, 1, log: true, CardMoveReasons.CixiongDraw);
        CompleteCixiongDoubleSwords(pending, activated: true, targetDiscarded: false, discardedCardId: null, sourceDrawCount: drawn.Count);
    }

    internal void CompleteCixiongDoubleSwords(NativeOwners.CixiongDoubleSwordsHandle pending, bool activated, bool targetDiscarded, int? discardedCardId, int sourceDrawCount)
    {
        if (_characterCaoPiAnniversaryGodOperations.TryCompleteKemingCixiong(pending, activated, targetDiscarded, discardedCardId, sourceDrawCount))
            return;
        var attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        attack.MarkCixiongDoubleSwordsResolved();
        _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CixiongDoubleSwordsResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, activated, targetDiscarded, discardedCardId, sourceDrawCount));
        _coreMatchLogOperations.AddLog(activated ? "EquipmentEffect" : "EquipmentSkipped", !activated ? $"{(source.Name)} 未发动【雌雄双股剑】。" : targetDiscarded ? $"{(source.Name)} 发动【雌雄双股剑】，{(target.Name)} 弃置了一张手牌。" : $"{(source.Name)} 发动【雌雄双股剑】并摸了一张牌。", source.Seat, target.Seat);
        _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
    }
}

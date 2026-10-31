#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeHarvestRuntime
{
    internal void BeginHarvestSelection(NativeOwners.GroupCardHandle group)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.PublicDraft)
        {
            throw new InvalidOperationException("The FiveGrains draft is not the current card resolution.");
        }

        while (group.TargetIndex < group.TargetSeats.Count && (!_coreMatchParticipantsPlayersAccess._players[group.TargetSeats[group.TargetIndex]].IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(group.ResolutionId, group.TargetSeats[group.TargetIndex]) || _coreShanheAlliedTricksOperations.ShanheAlliedHarvestProhibited(group, group.TargetSeats[group.TargetIndex]) || (_coreWushuangTacticRulesOperations.HasWushuangTactic(group.SourceSeat, 2241) && group.TargetSeats[group.TargetIndex] != group.SourceSeat) || (_characterZhugeGuoLeOperations.IsLeChengyanEffectUse(group.ResolutionId) || _characterTianChouOlOperations.IsTianChouShandaoUse(group.ResolutionId) || _characterQinLangOlOperations.IsQinLangXianyingUse(group.ResolutionId) || _characterYangBiaoAnniversaryOperations.IsAnniversaryYangBiaoFiveGrainsUse(group.ResolutionId)) && group.RevealedCardIds.Count == 0))
        {
            group.TargetIndex++;
            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(group.ResolutionId, group.TargetIndex);
        }

        if (group.TargetIndex >= group.TargetSeats.Count)
        {
            FinishFiveGrains(group);
            return;
        }

        var picker = _coreMatchParticipantsPlayersAccess._players[group.TargetSeats[group.TargetIndex]];
        if (_coreGroupTargetNullificationOperations.Value.TryBeginGroupTargetNullification(group))
            return;
        if (_characterLuXunMouMobileOperations.Value.TryBeginMouLuXunGroupEffect(group))
            return;
        var processing = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing);
        var revealed = group.RevealedCardIds.Select(cardId => processing.Single(card => card.Id == cardId)).ToArray();
        if (revealed.Length == 0)
        {
            throw new InvalidOperationException("A FiveGrains picker cannot choose from an empty reveal.");
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.SelectHarvestCard, picker.Seat, $"{(_coreMatchParticipantsPlayersAccess._players[group.SourceSeat].Name)} 使用了【五谷丰登】，请选择一张公开牌。", group.RevealedCardIds.ToArray(), [], group.SourceSeat, group.Card.Kind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = revealed.Select(card => new PromptChoice(new ChoiceId($"harvest.card-{(card.Id)}"), $"选择公开牌【{(card.DisplayName)}】（{(card.RankText)}）", [card.Id], [], new Dictionary<string, string> { ["action"] = "harvest-pick" })).ToArray()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = picker.IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    internal void ResolveHarvestSelection(NativeOwners.GroupCardHandle group, CharacterState picker, int cardId)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.PublicDraft)
        {
            throw new InvalidOperationException("The FiveGrains draft is not the current card resolution.");
        }

        if (group.TargetIndex >= group.TargetSeats.Count || group.TargetSeats[group.TargetIndex] != picker.Seat || _coreShanheAlliedTricksOperations.ShanheAlliedHarvestProhibited(group, picker.Seat))
        {
            throw new InvalidOperationException("The selected FiveGrains picker is not current.");
        }

        if (!group.RevealedCardIds.Contains(cardId))
        {
            throw new InvalidOperationException("The selected card is not in the public FiveGrains reveal.");
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(candidate => candidate.Id == cardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.Hand(picker.Seat), CardMoveReasons.HarvestPick);
        group.RemoveRevealedCard(cardId);
        _coreMatchLogOperations.AddLog("CardSelected", $"{(picker.Name)} 从五谷丰登的公开牌中选择了【{(card.DisplayName)}】。", picker.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HarvestCardSelectedEvent(group.ResolutionId, picker.Seat, cardId));
        group.TargetIndex++;
        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(group.ResolutionId, group.TargetIndex);
        if (group.TargetIndex >= group.TargetSeats.Count)
        {
            FinishFiveGrains(group);
        }
        else
        {
            BeginHarvestSelection(group);
        }
    }

    internal void FinishFiveGrains(NativeOwners.GroupCardHandle group)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.PublicDraft)
        {
            throw new InvalidOperationException("The FiveGrains draft is not the current card resolution.");
        }

        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || group.TargetIndex < group.TargetSeats.Count)
        {
            throw new InvalidOperationException("FiveGrains cannot finish while a picker or child resolution is pending.");
        }

        foreach (var cardId in group.RevealedCardIds.ToArray())
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(candidate => candidate.Id == cardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.HarvestDiscard);
        }

        group.ClearRevealedCards();
        if (_coreExtraCardEffectRoundsOperations.TryBeginExtraCardEffectRound(group.ResolutionId))
            return;
        if (!_characterTianChouOlOperations.IsTianChouShandaoUse(group.ResolutionId) && !_coreTieredRoundZeroReturnsOperations.SkipTieredRoundZeroFinishedMovement(group.ResolutionId, group.Card) && !_characterDiaoChanLeOperations.SkipDiouFinishedMovement(group.ResolutionId, group.Card) && !_characterXiaoQiaoMouOlOperations.SkipMouXqJueyanFinishedMovement(group.ResolutionId, group.Card) && !_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(group.ResolutionId) && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(group.ResolutionId, group.Card.Id) && !_characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(group.ResolutionId, group.Card.Id) && !_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(group.ResolutionId, group.Card.Id) && !(_coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)is { } completedMaterialUse && _characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuUseMaterials(completedMaterialUse)))
        {
            var cardLocation = _cardZonesOperations._cardZones.GetLocation(group.Card.Id);
            if (cardLocation == CardLocation.Processing)
                _corePhysicalMovementFacadesOperations.MoveCard(group.Card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
            else if (cardLocation != CardLocation.DiscardPile)
                throw new InvalidOperationException($"A resolved {(group.Card.Kind)} left Processing through an unsupported destination: {(cardLocation)}.");
        }

        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(group.ResolutionId, group.TargetSeats.Count);
        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        _coreNativeCardUseCompletionOperations.FinishCardUse(group.ResolutionId, group.Card);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
        {
            _kernelCoreMainOperations.CompleteGame();
        }
    }
}

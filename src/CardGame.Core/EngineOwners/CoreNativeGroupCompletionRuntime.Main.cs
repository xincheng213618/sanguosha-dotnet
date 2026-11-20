#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeGroupCompletionRuntime
{
    internal void FinishGroupAttack(NativeOwners.GroupCardHandle group, bool allowYingboGift = true)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.ResponseAttack)
        {
            throw new InvalidOperationException("The group attack is not the current card resolution.");
        }

        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null)
        {
            throw new InvalidOperationException("A group card cannot finish while a target continuation is pending.");
        }

        if (_coreExtraCardEffectRoundsOperations.TryBeginExtraCardEffectRound(group.ResolutionId))
            return;
        if (allowYingboGift && _characterLuMengMouOperations.TryBeginYingboGift(group.ResolutionId, group.SourceSeat, group.Card, group.Card.Kind, group.PhysicalCards, YingboGiftContinuation.GroupAttack, group: group))
        {
            return;
        }

        List<NativeUseDiscardEntry>? finished = null;
        foreach (var physicalCard in group.PhysicalCards)
        {
            if (_nativeMaterialClaims.PreservesMaterialAtCompletion(group.ResolutionId, physicalCard.Id))
                continue;
            var cardLocation = _cardZonesOperations._cardZones.GetLocation(physicalCard.Id);
            if (cardLocation == CardLocation.Processing)
            {
                if (_coreNativeCardUseCompletionOperations.DiscardFinishedUseMaterial(physicalCard) is { } entry)
                    (finished ??= []).Add(entry);
            }
            else if (!RuntimeSkills.RuntimeRules_Core_CommonRules.IsResolvedGroupPhysicalCardDestinationAllowed(cardLocation, group.DamageClaimedPhysicalCardIds.Contains(physicalCard.Id), group.TargetSeats))
            {
                throw new InvalidOperationException($"A resolved {(group.Card.Kind)} left Processing through an unsupported destination: {(cardLocation)}.");
            }
        }

        _coreNativeCardUseCompletionOperations.StoreFinishedUseDiscards(group.ResolutionId, finished);
        if (finished is not null && _coreMatchParticipantsPlayersAccess._players.FirstOrDefault(player => player.IsAlive && player.Seat != group.SourceSeat && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ClaimResolvedGlobalCard, group.Card.Kind)) is { } claimant)
        {
            var claimed = new List<int>();
            foreach (var entry in finished)
            {
                if (_zoneEntries.CurrentEntry(CardLocation.DiscardPile, entry.CardId) != entry.Sequence)
                    continue;
                var card = group.PhysicalCards.Single(material => material.Id == entry.CardId);
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(claimant.Seat), CardMoveReasons.JuxiangGain);
                claimed.Add(card.Id);
            }

            if (claimed.Count > 0)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuxiangCardClaimedEvent(group.ResolutionId, claimant.Seat, claimed.ToArray()));
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(claimant.Name)} 获得了结算完毕的【{(group.Card.DisplayName)}】。", claimant.Seat);
            }
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

    internal void FinishGroupRecovery(NativeOwners.GroupCardHandle group)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.Recovery)
        {
            throw new InvalidOperationException("The group recovery is not the current card resolution.");
        }

        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null)
        {
            throw new InvalidOperationException("A group recovery cannot finish while another continuation is pending.");
        }

        if (_coreExtraCardEffectRoundsOperations.TryBeginExtraCardEffectRound(group.ResolutionId))
            return;
        if (!_characterTianChouOlOperations.IsTianChouShandaoUse(group.ResolutionId) && !_coreTieredRoundZeroReturnsOperations.SkipTieredRoundZeroFinishedMovement(group.ResolutionId, group.Card) && !_characterDiaoChanLeOperations.SkipDiouFinishedMovement(group.ResolutionId, group.Card) && !_characterXiaoQiaoMouOlOperations.SkipMouXqJueyanFinishedMovement(group.ResolutionId, group.Card) && !_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(group.ResolutionId) && !_nativeMaterialClaims.PreservesMaterialAtCompletion(group.ResolutionId, group.Card.Id) && !(_coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)is { } completedMaterialUse && _characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuUseMaterials(completedMaterialUse)))
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

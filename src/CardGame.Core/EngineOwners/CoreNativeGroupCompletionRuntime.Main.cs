#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeGroupCompletionRuntime
{
    internal void FinishGroupAttack(global::CardGame.Core.NativeOwners.GroupCardHandle group, bool allowYingboGift = true)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.ResponseAttack)
        {
            throw new InvalidOperationException("The group attack is not the current card resolution.");
        }

        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null)
        {
            throw new InvalidOperationException("A group card cannot finish while a target continuation is pending.");
        }

        if (_coreExtraCardEffectRoundsOperations.TryBeginExtraCardEffectRound(group.ResolutionId))
            return;
        if (allowYingboGift && _characterLuMengMouOperations.TryBeginYingboGift(group.ResolutionId, group.SourceSeat, group.Card, group.Card.Kind, group.PhysicalCards, YingboGiftContinuation.GroupAttack, group: group))
        {
            return;
        }

        foreach (var physicalCard in group.PhysicalCards)
        {
            if (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(group.ResolutionId, physicalCard.Id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(group.ResolutionId, physicalCard.Id))
                continue;
            var cardLocation = _cardZonesOperations._cardZones.GetLocation(physicalCard.Id);
            if (cardLocation == CardLocation.Processing)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(physicalCard, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
            }
            else if (!global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime.IsResolvedGroupPhysicalCardDestinationAllowed(cardLocation, group.DamageClaimedPhysicalCardIds.Contains(physicalCard.Id), group.TargetSeats))
            {
                throw new InvalidOperationException($"A resolved {(group.Card.Kind)} left Processing through an unsupported destination: {(cardLocation)}.");
            }
        }

        if (_coreMatchParticipantsPlayersAccess._players.FirstOrDefault(player => player.IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ClaimResolvedGlobalCard, group.Card.Kind))is { } claimant && group.SourceSeat != claimant.Seat)
        {
            var claimable = group.PhysicalCards.Where(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.DiscardPile).ToArray();
            foreach (var card in claimable)
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(claimant.Seat), CardMoveReasons.JuxiangGain);
            if (claimable.Length > 0)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JuxiangCardClaimedEvent(group.ResolutionId, claimant.Seat, claimable.Select(card => card.Id).ToArray()));
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

    internal void FinishGroupRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.Recovery)
        {
            throw new InvalidOperationException("The group recovery is not the current card resolution.");
        }

        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null)
        {
            throw new InvalidOperationException("A group recovery cannot finish while another continuation is pending.");
        }

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

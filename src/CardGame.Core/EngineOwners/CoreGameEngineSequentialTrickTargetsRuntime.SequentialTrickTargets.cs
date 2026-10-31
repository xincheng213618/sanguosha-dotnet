#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSequentialTrickTargetsRuntime
{
    internal static bool UsesSequentialTrickTargets(LegalActionKind kind) => RuntimeSkills.RuntimeRules_Core_SequentialTrickTargets.UsesSequentialTrickTargets(kind);
    internal IReadOnlyList<int> FreezeSequentialTrickTarget(long frameId, IReadOnlyList<int> targets, LegalActionKind kind, int? firstTargetCardId, CardKind? requiredCardKind)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame use)
            throw new InvalidOperationException("A trick target window lost its card use.");
        if (use.SequentialTrick is null && targets.Count > 1 && UsesSequentialTrickTargets(kind) && (kind != LegalActionKind.IronChain || targets.Count > 2 || _coreProgramMultiTargetDuelsOperations.HasProgramIronChainTargetExpansion(use.SourceSeat)))
        {
            use = use with
            {
                SequentialTrick = new(kind, firstTargetCardId, requiredCardKind)
            };
            _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, use);
        }

        return use.SequentialTrick is null ? targets : [CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex]];
    }

    internal bool HasRemainingSequentialTrickTargets(long frameId) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId)is { SequentialTrick: not null } use && CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Skip(use.TargetIndex + 1).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive);
    internal void MoveFinishedTrickCard(long frameId, Card card)
    {
        if (_characterZhugeGuoLeOperations.KeepLeChengyanBorrowedMaterial(frameId))
            return;
        if (_coreMantianGuohaiOperations.KeepMantianGuohaiMaterial(frameId))
            return;
        if (_coreExtraCardEffectRoundsOperations.HasUnstartedExtraCardEffect(frameId))
            return;
        if (_coreTieredRoundZeroReturnsOperations.SkipTieredRoundZeroFinishedMovement(frameId, card))
            return;
        if (card.Id == 0 && (_coreOriginalUseLightningOperations.IsOriginalUseLightning(frameId) || _characterZhugeLiangMouOperations.IsZeroEntityUse(frameId) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(frameId) || _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(frameId)))
            return;
        if (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(frameId, card.Id) || (_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(frameId, card.Id) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(frameId, card.Id) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(frameId, card.Id)) || _coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(frameId, card.Id)) || HasRemainingSequentialTrickTargets(frameId) || _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedBorrowedSwordTargets(frameId))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { } use && _characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuUseMaterials(use))
            return;
        if (_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(frameId, card.Id))
            return;
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
    }

    internal bool TryContinueSequentialTrick(long frameId, bool afterAttack = false)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame { SequentialTrick: { } continuation } use)
            return false;
        if (_characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieDelayedTail(use))
            return _characterCuiLieAnniversaryOperations.ContinueAnniversaryCuiLieDelayedPlacement(use);
        var next = use.TargetIndex + 1;
        while (next < CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count && !_coreMatchParticipantsPlayersAccess._players[CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[next]].IsAlive)
            next++;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || next == CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, use with { TargetIndex = CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count, SequentialTrick = null });
            return false;
        }

        if (!ReferenceEquals(use, _resolutionStackOperations._resolutionStack.LastOrDefault()))
            throw new InvalidOperationException("A trick must finish its child before the next target.");
        var card = _characterDongChengMouOperations.IsMouDcZeroUse(use.Id) ? _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true) : _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(use.Id) ? _coreTieredRoundZeroUsesOperations.TieredRoundZeroRepresentation(use.Id) : (_characterZhouYuMouOperations.IsMouZyZeroFireUse(use.Id) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(use.Id) || _characterDiaoChanLeOperations.IsDiouZeroUse(use.Id) || _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(use.Id)) ? _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true) : _characterWenChouMouOlOperations.IsMouWenChouZeroDuel(use.Id) || _characterXuHuangMouMobileOperations.IsMouXuHuangZeroUse(use.Id) || _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(use.Id) ? _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true) : (_coreSameTypeActualUseAidOperations.HasSameTypeAidTargetTail(use) || _coreUniqueLeaderTrickTargetsOperations.HasUniqueLeaderTrickTargetTail(use) || _coreTargetedOrdinaryTrickOperations.HasTargetedOrdinaryTrickTail(use) || _coreDamageTargetBudgetOperations.HasDamageTargetBudgetTail(use) || _coreAdjacentRedTargetsOperations.HasAdjacentRedTargetTail(use)) && use.CardId == 0 && _coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(use.Id) ? _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true) : (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, use.CardId) || (_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(use.Id, use.CardId) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(use.Id, use.CardId) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(use.Id, use.CardId)) || _coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(use.Id, use.CardId))) ? _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(use.CardId) : _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == use.CardId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, use with { TargetIndex = next, Step = ResolutionFrameStep.ResolvingEffect });
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        _coreNativeNullificationOperations.BeginNullificationWindow(frameId, card, use.SourceSeat, [CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[next]], continuation.ActionKind, null, continuation.RequiredCardKind, use.CardKind);
        return true;
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardUseCompletionRuntime
{
    internal void CompleteFinishedAttackCardUse(NativeOwners.CardAttackHandle attack)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsProgramSkillUse: true, AwaitingProviders: false } jijiang && CoreNativeDamageHandlesRuntime.SameAttackOwner(jijiang.ActiveAttack, attack))
        {
            _coreNativeFactionSlashCompletionOperations.Value.CompleteProgramFactionCardRequest(jijiang);
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsQinglongCrescentBladeUse: true, AwaitingProviders: false } qinglongFactionSlash && CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglongFactionSlash.ActiveAttack, attack))
        {
            _coreNativeFactionSlashOperations.CompleteQinglongCrescentBladeFactionSlash(qinglongFactionSlash);
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreNativeCardContinuationCursorsOperations.ActiveQinglongFollowup is null && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed && !_characterWuAnGuoOlOperations.HasWuAnGuoLiyongCompletedUseParent(attack.ResolutionId) && (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword?.ActiveAttack is not { } borrowedAttack || !CoreNativeDamageHandlesRuntime.SameAttackOwner(borrowedAttack, attack)))
        {
            _kernelCoreMainOperations.CompleteGame();
        }
    }

    internal void FinishCardUse(long frameId, Card card, CardKind? playedCardKind = null)
    {
        _characterDiaoChanLeOperations.RetireUnplacedDiouCarrier(frameId);
        _characterDengAiAnniversaryGodOperations.RetireUnplacedShenDaCarrier(frameId);
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.CardUse, card.Id, playedCardKind))
            return;
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.CardUse, card.Id, playedCardKind))
            return;
        if (_characterZhugeGuoLeOperations.TryContinueLeChengyanBorrowed(frameId))
            return;
        if (_coreOriginalUseLightningOperations.TryContinueOriginalUseLightningPlacement(frameId))
            return;
        if (_coreMantianGuohaiOperations.IsMantianGuohaiUse(frameId))
        {
            if (_coreSequentialTrickTargetsOperations.TryContinueSequentialTrick(frameId))
                return;
            if (_coreMantianGuohaiOperations.TryContinueMantianGuohaiGifts(frameId))
                return;
        }

        if (_coreExtraCardEffectRoundsOperations.TryBeginExtraCardEffectRound(frameId))
            return;
        if (_coreSequentialTrickTargetsOperations.TryContinueSequentialTrick(frameId))
            return;
        if (_coreNextCardTargetAdjustmentsOperations.TryContinueAdjustedSimpleCardUse(frameId))
            return;
        if (_coreNextCardTargetAdjustmentsOperations.TryContinueAdjustedBorrowedSwordUse(frameId))
            return;
        if (_coreSoleIncomingRangeDrawOperations.TryBeginSoleIncomingRangeDraw(frameId))
            return;
        if (_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId)is { } pendingUse && !_characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuUseMaterials(pendingUse))
        {
            foreach (var physicalCardId in pendingUse.PhysicalCardIds ?? [pendingUse.CardId])
            {
                if (_characterXinPiOlOperations.IsXinPiPhysicalClaim(frameId, physicalCardId) || _characterHuaManOlOperations.IsHuaManClaimedEntity(frameId, physicalCardId) || _coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(frameId, physicalCardId))
                    continue;
                if (_cardZonesOperations._cardZones.GetLocation(physicalCardId) == CardLocation.Processing)
                {
                    var physicalCard = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(candidate => candidate.Id == physicalCardId);
                    _corePhysicalMovementFacadesOperations.MoveCard(physicalCard, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
                }
            }
        }

        QueueCardUseFinishedWithoutPop(frameId, card, playedCardKind);
        var completedUse = _coreCurrentUseTargetCandidateRefreshOperations.NormalizeImplicitDrawTwoUseTarget(_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == frameId));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && completedUse.Action is { } action && _coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseCompleted, action.TargetSeats, ProgramCardContinuation.CompletedCard, cardUseCausedDamage: completedUse.CausedDamage))
            return;
        PopFinishedCardUse(frameId);
    }

    internal void QueueCardUseFinishedWithoutPop(long frameId, Card card, CardKind? playedCardKind)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { LeChengyanEffect: not null } originalUse && _characterZhugeGuoLeOperations.ValidLeChengyanEffect(originalUse))
            playedCardKind = originalUse.CardKind;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { NativeSlashTargetDuel: { } targetPlan } targetUse && _coreNativeSlashTargetDuelOperations.ValidNativeSlashTargetDuelPlan(targetUse, targetPlan))
            playedCardKind = targetUse.CardKind;
        _coreNativeCardUseFramesOperations.SetCardUseStep(frameId, ResolutionFrameStep.Completed);
        if (_resolutionStackOperations._resolutionStack.Count == 0 || _resolutionStackOperations._resolutionStack[^1] is not CardUseFrame cardUse || cardUse.Id != frameId)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} has unfinished child frames.");
        }

        _coreNoDamageSkillDisableOperations.Value.SettleNoDamageSkillDisable(cardUse);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(frameId, card.Id, playedCardKind ?? card.Kind));
    }

    internal void PopFinishedCardUse(long frameId)
    {
        var completedUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == frameId);
        _characterMaChengOlOperations.CompleteChenglieTerminalCleanup(completedUse);
        completedUse = _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!;
        _coreProgramNativeReturnsOperations.NativeReturns.ValidateUse(ref completedUse);
        var completion = _coreProgramNativeReturnsOperations.NativeReturns.PrepareUse(completedUse, _coreHpLossSlashMaterialsOperations.HasExactHpLossMaterialReturn(completedUse));
        _coreRuntimeOperations.PopResolutionFrame(frameId, ResolutionFrameKind.CardUse);
        _coreProgramNativeReturnsOperations.NativeReturns.ReturnUse(completion);
    }
}

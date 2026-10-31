#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardUseLifecycleRuntime
{
    internal CardUseFrame? LifecycleCardUse(long frameId) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId);
    internal void UpdateLifecycleCardUse(long frameId, Func<CardUseFrame, CardUseFrame> update)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, update(frame));
    }

    internal bool TryMarkProgramUseAccepted(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.ProgramUseAccepted)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { ProgramUseAccepted = true });
        return true;
    }

    internal bool TryMarkProgramUseCommitted(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.ProgramUseCommitted)
            return false;
        _characterLiuYouAnniversaryOperations.CaptureAnniversaryLiuYouNextSlash(frameId);
        frame = LifecycleCardUse(frameId)!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { ProgramUseCommitted = true });
        return true;
    }

    internal bool TryMarkFinalizedTrickProgramsStarted(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.FinalizedTrickProgramsStarted)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { FinalizedTrickProgramsStarted = true });
        return true;
    }

    internal bool TryMarkFinalizedSimpleProgramsStarted(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.FinalizedSimpleProgramsStarted)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { FinalizedSimpleProgramsStarted = true });
        return true;
    }

    internal bool TryMarkTargetsAdjusted(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.TargetsAdjusted)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { TargetsAdjusted = true });
        return true;
    }

    internal bool TryMarkUnlimitedUse(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.UnlimitedUse)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { UnlimitedUse = true });
        return true;
    }

    internal bool TryMarkYingboUnrespondable(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.YingboUnrespondable)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { YingboUnrespondable = true });
        return true;
    }

    internal bool TryMarkYingboRepeated(long frameId)
    {
        var frame = LifecycleCardUse(frameId) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        if (frame.YingboRepeated)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, frame with { YingboRepeated = true });
        return true;
    }

    internal bool TryBeginCommittedCardUse(long frameId, ProgramCardContinuation continuation, ProgramTrickContinuation? trick = null, ProgramSimpleCardContinuation? simple = null)
    {
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == frameId);
        if (frame.Action is not { } action || !TryMarkProgramUseCommitted(frameId))
            return false;
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, continuation, trickContinuation: trick, simpleContinuation: simple);
    }

    internal void ContinueCommittedTrickUse(ProgramCardTriggerWindowFrame frame)
    {
        var continuation = frame.TrickContinuation ?? throw new InvalidOperationException("The committed trick lost its continuation.");
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(frame.ParentFrameId, continuation.EffectCardId, requireProcessing: true);
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Id == frame.ParentFrameId).Action!;
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(frame.ParentFrameId, card, action.ActorSeat, action.TargetSeats, continuation.ActionKind, continuation.TargetCardId, continuation.RequiredCardKind, action.EffectiveKind);
    }

    internal void BeginSimpleCardUse(long frameId, ProgramSimpleCardContinuation continuation)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == frameId);
        if (continuation.Effect == SimpleCardUseEffect.Alcohol)
            _characterHuangQuanOlOperations.RecordJianjiOrdinaryAlcoholUse(frameId);
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(frameId, new(RecoveryPaidCardUseKind.Simple, use.SourceSeat, continuation.CardId, Simple: continuation)))
            return;
        if ((LifecycleCardUse(frameId)?.TargetsAdjusted == true) && use.TargetSeats.Count > 1 && continuation.Effect is SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol or SimpleCardUseEffect.GuanduIntoxication)
            UpdateLifecycleCardUse(frameId, frame => frame with { AdjustedSimpleContinuation = continuation with { RecoveryPolicySources = continuation.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null } });
        if (!TryBeginCommittedCardUse(frameId, ProgramCardContinuation.CommittedSimpleCard, simple: continuation))
            ContinueSimpleCardUse(frameId, continuation);
    }

    internal void ContinueSimpleCardUse(long frameId, ProgramSimpleCardContinuation continuation)
    {
        if (_characterWeiYanHappyGodOperations.ContinueHappyWeiYanIneffectiveSimple(frameId, continuation))
            return;
        if (_characterChengYuMouOperations.ContinueMouCyIneffectiveSimple(frameId, continuation))
            return;
        if (_characterCaoZhenBoundaryMobileOperations.ContinueBoundarySidiIneffectiveSimple(frameId, continuation))
            return;
        if (_coreFinalizedCardTargetsOperations.TryBeginFinalizedSimpleCardPrograms(frameId, continuation))
            return;
        if (_characterCaoChunOlOperations.TryBeginCaoChunSimpleDebt(frameId, continuation))
            return;
        if (_coreProgramCardUseNullificationOperations.TryBeginSimpleBeforeTargetPrograms(frameId, continuation))
            return;
        if (_characterHuangZhongAnniversaryGodOperations.ContinueAnniversaryHzIneffectiveSimple(frameId, continuation))
            return;
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == frameId);
        if (_coreGuanduCardsOperations.ContinueGuanduIntoxicationUse(frameId, continuation))
            return;
        if (_coreDyingSuitsPeachProgramsOperations.ContinueDyingSuitsPeach(frameId, continuation))
            return;
        if (_characterDongZhuoMouOlOperations.ContinueMouDongZhuoOlWine(frameId, continuation) || _characterHuanShuJunOlOperations.ContinueHuanpeiSimpleUse(frameId, continuation) || _characterDiaoChanLeOperations.ContinueDiouSimpleUse(frameId, continuation) || _characterXiaoQiaoMouOlOperations.ContinueMouXqJueyanSimpleUse(frameId, continuation))
            return;
        if (_coreShanheSourceMouDongZhuoWineOperations.ContinueShanheSourceMouWine(frameId, continuation))
            return;
        if (_characterZhouYuOlMouOperations.ContinueMouZuifengAlcoholUse(frameId, continuation))
            return;
        if (_characterGuoTuOlOperations.ContinueWeijieSimpleUse(frameId, continuation))
            return;
        if (_coreTieredRoundZeroUsesOperations.ContinueTieredRoundZeroSimpleUse(frameId, continuation))
            return;
        if (_characterZhugeLiangMouOperations.ContinueMouZhijueSimpleUse(frameId, continuation))
            return;
        if (_characterAHuiNanOlOperations.ContinueAHuiNanJuemanSimpleUse(frameId, continuation))
            return;
        if (_characterQinLangOlOperations.ContinueQinLangXianyingSimpleUse(frameId, continuation))
            return;
        if (_coreDrawFundedDistinctBasicUsesOperations.ContinueDrawFundedDistinctBasicSimpleUse(frameId, continuation))
            return;
        if (_coreDesignatedExtraTargetsOperations.ContinueDesignatedVirtualBasicEffect(frameId, continuation))
            return;
        if (_coreVirtualBasicOffersOperations.ContinueVirtualBasicEffect(frameId, continuation))
            return;
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(continuation.CardId)).Single(item => item.Id == continuation.CardId);
        var source = _coreMatchParticipantsPlayersAccess._players[frame.SourceSeat];
        _coreNativeCardUseFramesOperations.SetCardUseStep(frameId, ResolutionFrameStep.ResolvingEffect);
        if (!source.IsAlive && frame.ExtraCardEffect is null && !_characterDaQiaoLeOperations.IsZixiExtraEffectRound(frameId) && !_coreSoleIncomingEffectRepeatOperations.IsSoleIncomingExtraRound(frameId))
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(frameId, card, frame.CardKind);
            return;
        }

        switch (continuation.Effect)
        {
            case SimpleCardUseEffect.EquipmentPlacement:
                _coreNativeEquipmentUseOperations.CompleteEquipmentUse(source, card, frameId);
                break;
            case SimpleCardUseEffect.Equipment:
                _coreNativeCardUseCompletionOperations.FinishCardUse(frameId, card);
                break;
            case SimpleCardUseEffect.Alcohol:
                _coreNativeTrickUseOperations.CompleteAlcoholUse(source, card, frameId);
                break;
            case SimpleCardUseEffect.Recovery:
                _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(source, _coreMatchParticipantsPlayersAccess._players[frame.TargetSeats[frame.TargetIndex]], card, frameId, frame.CardKind, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
                break;
            default:
                throw new InvalidOperationException("Unsupported simple card effect.");
        }
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHpChangeProgramsRuntime
{
    internal HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, HpChangeKind kind)
    {
        _characterDongBaiOlOperations.ObserveDongBaiHpDecreased(targetSeat, before, after);
        var amount = kind == HpChangeKind.Setting ? Math.Abs(after - before) : kind == HpChangeKind.Loss ? before - after : after - before;
        if (!_coreMatchSetupSetupCompleteAccess._setupComplete || amount <= 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
            return null;
        var change = new HpChangeContext(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parentFrameId, sourceSeat, targetSeat, kind, amount, before, after, kind == HpChangeKind.Loss && _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashCapability ? new LossOccurrence(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase) : null, kind == HpChangeKind.Loss && _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashCapability ? Array.AsReadOnly(new[] { SkillProgramTriggerWindow.AfterHpLost, SkillProgramTriggerWindow.AfterHealthChanged }.SelectMany(window => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[targetSeat], window)).ToArray()) : null)
        {
            HappyQiaoPool = _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(targetSeat, parentFrameId)
        };
        _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Add(change);
        _characterHuangGaiMouMobileOperations.CaptureMouKurouNativeLoss(change);
        _characterZhaoZhongOlOperations.CaptureYintianRecoveryQualification(change);
        _coreQimeiProgramsOperations.CaptureQimeiHpQualification(change);
        _characterXiZhiCaiOlOperations.CaptureXianfuRecoveryQualification(change);
        _characterHuangQuanOlOperations.CaptureDianhuRecoveryQualification(change);
        _characterSunShangxiangSpOlOperations.Value.CaptureSpSunShangxiangRecovery(change);
        _characterSunJianMouOlOperations.CaptureMouSunJianOlHp(change);
        _programPipelineHooksOperations._programPipelineHooks.CaptureHealthChange(change);
        return change;
    }

    internal bool TryBeginHpChangedProgramWindow(long? resumeFrameId = null, PostEventContinuation continuation = PostEventContinuation.Boundary, int? cardId = null, CardKind? cardKind = null)
    {
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(resumeFrameId, continuation, cardId, cardKind))
            return true;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed || (resumeFrameId is null ? _resolutionStackOperations._resolutionStack.Count != 0 : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != resumeFrameId))
            return false;
        while (_coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.FirstOrDefault(change => resumeFrameId is null || change.ParentFrameId == resumeFrameId || _coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashHpChange(resumeFrameId.Value, change))is { } change)
        {
            _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Remove(change);
            var owner = _coreMatchParticipantsPlayersAccess._players[change.TargetSeat];
            var windows = change.Kind switch
            {
                HpChangeKind.Loss => new[]
                {
                    SkillProgramTriggerWindow.AfterHpLost,
                    SkillProgramTriggerWindow.AfterHealthChanged
                },
                HpChangeKind.Recovery => new[]
                {
                    SkillProgramTriggerWindow.AfterHpRecovered,
                    SkillProgramTriggerWindow.AfterHealthChanged
                },
                _ => new[]
                {
                    SkillProgramTriggerWindow.AfterHealthChanged
                }
            };
            var ordinary = (change.FrozenLossCandidates ?? windows.SelectMany(window => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, window)).ToArray()).Where(candidate => !CharacterZhaoZhongOlRuntime.IsYintianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !_characterHuanShuJunOlOperations.IsWenyiHpTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !_coreQimeiProgramsOperations.IsQimeiEqualityTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !_characterXiZhiCaiOlOperations.IsXianfuEchoTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !_characterHuangQuanOlOperations.IsDianhuDrawTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !CharacterSunShangxiangSpOlRuntime.IsSpSunShangxiangLiangzhuTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !CoreShenwuNpcProgramsRuntime.IsShenwuJicaiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && !CoreNianActivityProgramsRuntime.IsNianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && candidate.SkillId != "wushuang:skill-3336").Concat(_characterZhaoZhongOlOperations.CollectYintianRecoveryCandidates(change)).Concat(_characterHuanShuJunOlOperations.CollectWenyiHpCandidates(change)).Concat(_coreQimeiProgramsOperations.CollectQimeiHpCandidates(change)).Concat(_characterXiZhiCaiOlOperations.CollectXianfuHpCandidates(change)).Concat(_characterHuangQuanOlOperations.CollectDianhuHpCandidates(change)).Concat(_characterSunShangxiangSpOlOperations.Value.CollectSpSunShangxiangRecoveryCandidates(change)).Concat(_coreShenwuNpcProgramsOperations.CollectShenwuRecoveryCandidates(change)).Concat(_coreNianActivityProgramsOperations.CollectNianRecoveryCandidates(change)).Concat(_coreWushuangSkillsBOperations.CollectWushuangBChunlaoLossCandidates(change)).ToArray();
            var registered = _programPipelineHooksOperations._programPipelineHooks.IncludeHealthChangeCandidates(change, owner.IsAlive ? ordinary : []);
            if (registered.Count == 0)
                continue;
            var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner)with
            {
                HpChangeAmount = change.Amount,
                HpBeforeChange = change.HpBefore,
                HpAfterChange = change.HpAfter,
                HealthChangeKind = _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.HealthChangeKindIs) ? change.Kind : null
            };
            var candidates = registered.Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId)).SelectMany(candidate => Enumerable.Range(0, _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).HpChangeOccurrence == SkillProgramHpChangeOccurrence.PerPoint ? change.Amount : 1).Select(index => candidate with { OccurrenceIndex = index })).ToArray();
            if (candidates.Length == 0)
                continue;
            var contexts = candidates.Select(candidate => candidate.SkillId == "wushuang:skill-3336" ? _coreWushuangSkillsBOperations.CreateWushuangBChunlaoLossContext(change, candidate) : CharacterZhaoZhongOlRuntime.IsYintianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _characterZhaoZhongOlOperations.CreateYintianRecoveryContext(change, candidate) : _characterHuanShuJunOlOperations.IsWenyiHpTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _characterHuanShuJunOlOperations.CreateWenyiHpContext(change, candidate) : _coreQimeiProgramsOperations.IsQimeiEqualityTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _coreQimeiProgramsOperations.CreateQimeiHpContext(change, candidate) : _characterXiZhiCaiOlOperations.IsXianfuEchoTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _characterXiZhiCaiOlOperations.CreateXianfuHpContext(change, candidate) : _characterHuangQuanOlOperations.IsDianhuDrawTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _characterHuangQuanOlOperations.CreateDianhuHpContext(change, candidate) : CharacterSunShangxiangSpOlRuntime.IsSpSunShangxiangLiangzhuTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _characterSunShangxiangSpOlOperations.Value.CreateSpSunShangxiangRecoveryContext(change, candidate) : CoreShenwuNpcProgramsRuntime.IsShenwuJicaiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _coreShenwuNpcProgramsOperations.CreateShenwuRecoveryContext(change, candidate) : CoreNianActivityProgramsRuntime.IsNianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _coreNianActivityProgramsOperations.CreateNianRecoveryContext(change, candidate) : new ProgramSkillWindowContext(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Window, change.Id, candidate.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: owner.Seat, Amount: change.Amount, OccurrenceIndex: candidate.OccurrenceIndex, Facts: facts, HpChange: change)).ToArray();
            if (continuation == PostEventContinuation.AwaitedProgramMovement && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SelectedCardPayment: { } payment, SelectedCardPaymentResult: null } parent)
            {
                if (payment.ActiveChildFrameId is not null)
                    throw new InvalidOperationException("A selected-card payment already has an active child frame.");
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { SelectedCardPayment = payment with { ActiveChildFrameId = change.Id } });
            }

            if (continuation == PostEventContinuation.DrawFundedDistinctBasic)
                _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasicChild(resumeFrameId!.Value, change.Id);
            _coreRuntimeOperations.PushRuntimeFrame(new HpChangedTriggerWindowFrame(change.Id, change, candidates, contexts, continuation, resumeFrameId, cardId, cardKind));
            _coreRuntimeOperations.AdvanceRuntimeTop<HpChangedTriggerWindowFrame>();
            return true;
        }

        return false;
    }

    [NativeFrameRoute(typeof(HpChangedTriggerWindowFrame), 320)]
    internal void ContinueHpChangedProgramWindowCore() => ProgramTriggerWindowDriver.Run(_kernelCoreGameEngineProgramTriggerWindowsOperations.CreateHpChangedTriggerWindowHost());
    internal void CompleteHpChangedProgramWindow(HpChangedTriggerWindowFrame frame)
    {
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.HpChangedTriggerWindow);
        _coreProgramRecoveryReceiptsOperations.ClearProgramRecoveryApplication(frame.Change);
        if (_coreResponseCompletionHealthOperations.Value.ReturnCardResponseCompletionHpChange(frame))
            return;
        if (_coreCardSupplyCompletionOperations.ReturnCardSupplyCompletionHpChange(frame))
            return;
        if (frame.Continuation == PostEventContinuation.DrawFundedDistinctBasic)
        {
            _coreDrawFundedDistinctBasicPaymentOperations.ResumeDrawFundedDistinctBasicHpChange(frame);
            return;
        }

        if (frame.Continuation == PostEventContinuation.AwaitedProgramMovement && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SelectedCardPayment: { } payment, SelectedCardPaymentResult: null } parent)
        {
            if (payment.ActiveChildFrameId != frame.Id)
                throw new InvalidOperationException("A selected-card payment lost its HP-change child frame.");
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { SelectedCardPayment = payment with { ActiveChildFrameId = null, LastCompletedChildFrameId = frame.Id } });
        }

        if (TryBeginHpChangedProgramWindow(frame.ResumeFrameId, frame.Continuation, frame.CardId, frame.CardKind))
            return;
        switch (frame.Continuation)
        {
            case PostEventContinuation.WenyiDamageHealth:
                _coreWenyiDamageHealthOperations.Value.ReturnWenyiDamageHealth(frame);
                break;
            case PostEventContinuation.HappyQiaoDamageHealth:
                _characterDaQiaoXiaoQiaoHappyGodOperations.ReturnHappyQiaoDamageHealth(frame);
                break;
            case PostEventContinuation.HappyQiaoCardMaximumHealth:
                _characterDaQiaoXiaoQiaoHappyGodOperations.ReturnHappyQiaoCardMaximumHealth(frame);
                break;
            case PostEventContinuation.Program:
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.FriendDuelResponse:
                _coreFriendDuelResponsesOperations.ContinueFriendDuelResponse(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.AwaitedProgramMovement:
                if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                    _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.CardUse:
                var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(frame.ResumeFrameId!.Value, frame.CardId!.Value);
                _coreNativeCardUseCompletionOperations.FinishCardUse(frame.ResumeFrameId!.Value, card, frame.CardKind);
                break;
            case PostEventContinuation.VirtualBasicCardUse:
                _coreVirtualBasicOffersOperations.FinishVirtualBasicUse(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.ColdDamageReplacement:
            case PostEventContinuation.ShanheS5YimieRecovery:
                _coreRuntimeOperations.AdvanceRuntimeFrame(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.RecoveryReplacement:
                _coreRuntimeOperations.AdvanceRuntimeFrame(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.SkillInvocation:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not SkillInvocationFrame invocation || !_characterHuaTuoAnniversaryGodOperations.IsSkillInvocationHealthChild(invocation, frame))
                    throw new InvalidOperationException("A skill invocation lost its exact paid health return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(invocation.Id);
                break;
            case PostEventContinuation.KemingEcho:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not KemingSkillEchoFrame echo || echo.Id != frame.ResumeFrameId || !_characterCaoPiAnniversaryGodOperations.KemingSkillEchoFrameRidesOn(frame, echo))
                    throw new InvalidOperationException("Keming echo lost its exact health-change return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(echo.Id);
                break;
            case PostEventContinuation.AmountSupport:
                if (!_characterCaoSongOlOperations.IsAmountSupportHealthChild(_resolutionStackOperations._resolutionStack.LastOrDefault(), frame))
                    throw new InvalidOperationException("Amount support lost its exact health-change return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.WenhePoison:
                if (!_coreWenhePoisonOperations.IsWenhePoisonHealthChild(_resolutionStackOperations._resolutionStack.LastOrDefault(), frame))
                    throw new InvalidOperationException("Poison lost its exact health-change return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.RecoveryPaidCardUse:
                _coreRecoveryReplacementOperations.ContinueRecoveryPaidCardUse(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.DrawPhaseObligation:
                _coreRuntimeOperations.AdvanceRuntimeFrame(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.DenglouCardUse:
                _characterWangCanOlOperations.ContinueDenglouNativePayment(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.GuoSiCardUse:
                _characterGuoSiOlOperations.ContinueGuoSiNativePayment(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.HistoricalEndingCardUse:
                _coreEndingHistoricalUsesOperations.ContinueHistoricalEndingPayment(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.CounterspellPayment:
                _coreUnrespondableCounterspellsOperations.ContinuePolicyCounterspellPayment(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.SoleIncomingRangeDraw:
                _coreSoleIncomingRangeDrawOperations.ContinueSoleIncomingRangeDraw(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.NativeCardEffectDraw:
                _coreNativeCardEffectDrawsOperations.ContinueNativeDrawTwo(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.WenheCardEffect:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame wenhe || !_coreWenheCardsOperations.IsWenheCardEffectHpReturn(wenhe, frame))
                    throw new InvalidOperationException("A Wenhe trick lost its exact HP-child return.");
                _coreWenheCardsOperations.ContinueWenheCardEffect(wenhe.Id);
                break;
            case PostEventContinuation.GuanduCardEffect:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame guandu || !_coreGuanduCardsOperations.IsGuanduCardEffectHpReturn(guandu, frame))
                    throw new InvalidOperationException("A Guandu card lost its exact health return.");
                _coreGuanduCardsOperations.ContinueGuanduCardEffect(guandu.Id);
                break;
            case PostEventContinuation.WumiaoFlood:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame flood || !_characterGuanYuWumiaoOperations.IsWumiaoFloodHpReturn(flood, frame))
                    throw new InvalidOperationException("Wumiao Flood HP changes lost their exact card return.");
                _characterGuanYuWumiaoOperations.ContinueWumiaoFlood(flood.Id);
                break;
            case PostEventContinuation.DragonBoatCardEffect:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame dragon || !_coreDragonBoatCardsOperations.IsDragonBoatCardEffectHpReturn(dragon, frame))
                    throw new InvalidOperationException("Dragon boat HP changes lost their exact card return.");
                _coreDragonBoatCardsOperations.ContinueDragonBoatCardEffect(dragon.Id);
                break;
            case PostEventContinuation.ColorFireAttackPayment:
                _coreColorFireAttacksOperations.ContinueColorFireAttackPayment(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.HuxinMirrorPlacement:
                _coreHuxinMirrorOperations.ContinueHuxinMirrorPlacement(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.EquipmentRecast:
                _coreEquipmentRecastsOperations.ContinueEquipmentRecast(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.FactionRequestCost:
                _coreFactionRequestCostRecoveriesOperations.ContinuePaidFactionRequestCost(frame.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.GroupRecovery:
                _coreNativeGroupRecoveryOperations.Value.CompleteGroupRecoveryTarget();
                break;
            case PostEventContinuation.Boundary:
                _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow();
                break;
        }

        return;
    }

    internal void AdvanceHpChangedProgramCandidate(HpChangedTriggerWindowFrame frame, bool activated, bool completed)
    {
        var candidate = frame.Candidates[frame.CandidateIndex];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.OwnerSeat, frame.Contexts[frame.CandidateIndex].Window, activated, completed));
        AdvanceHpChangedProgramCursor(frame);
    }

    internal void AdvanceHpChangedProgramCursor(HpChangedTriggerWindowFrame frame) => _programWindowCursorOperations.Get().AdvanceHpChangedProgramCursor(frame);
}

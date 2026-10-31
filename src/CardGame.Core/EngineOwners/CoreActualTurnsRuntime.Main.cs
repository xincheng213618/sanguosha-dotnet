#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualTurnsRuntime
{
    internal void BeginTurn()
    {
        var current = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat];
        if (!current.IsAlive)
        {
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat = _coreTableSeatQueriesOperations.FindNextAliveSeat(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat);
            current = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat];
        }

        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber >= _coreMatchDefinitionOptionsAccess._options.MaxTurns)
        {
            if (_modeRuntimeOperations._modeRuntime.TryFinishAtTurnLimit(_coreMatchDefinitionOptionsAccess._options.MaxTurns))
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return;
            }

            _coreMatchOutcomesOperations.EndAsDraw($"达到最大回合数 {(_coreMatchDefinitionOptionsAccess._options.MaxTurns)}");
            return;
        }

        if (_characterLuBuOlMagicOperations.TryBeginMoLuBuOpening())
            return;
        if (_characterBeiMiHuOlOperations.TryBeginBeiMiHuActualTurnPrelude(current))
            return;
        _coreMouLuMengOperations.BeginRoundForTurn(current);
        BeginTurnAfterBeforeTurnPrelude(current);
    }

    internal void BeginTurnAfterBeforeTurnPrelude(CharacterState current)
    {
        if (current.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !current.IsAlive || _resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Actual turn initialization lost its completed before-turn boundary.");
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber++;
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression with
        {
            OwnerSeat = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat,
            TurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber
        };
        _coreCardUseModulesOperations.ExpireOwnerTurnStartDamageModifiers(current.Seat);
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._slashCountThisTurn = 0;
        _coreCardUseDebitsOperations.ResetCardUseDebitPhase();
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._usedOrPlayedSlashDuringPlayPhase = false;
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn = false;
        current.UsedPlayPhaseAlcoholThisTurn = false;
        _skillRuntimeStateOperations._skillRuntimeState.ResetTurn();
        _coreNamedUseConversionsOperations.ResetTurnProgramBooleanStates();
        foreach (var character in _coreMatchParticipantsPlayersAccess._players)
            foreach (var grant in character.SkillGrants.Grants.Where(grant => grant.TurnExpiry is null && grant.NextActualTurnStartExpiry is null && grant.SourceId.StartsWith("turn:", StringComparison.Ordinal)))
                character.SkillGrants.RemoveGrant(grant.GrantId);
        foreach (var key in _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.Keys.Where(key => key.Seat == current.Seat).ToArray())
            _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.Remove(key);
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = TurnPhase.Draw;
        _coreMatchLogOperations.AddLog("TurnStarted", $"第 {(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)} 回合：{(current.Name)}（{(_coreMatchPresentationOperations.GetPublicGeneralName(current))}）行动。", current.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnStartedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, current.Seat));
        if (_characterSunQuanAnniversaryGodOperations.TryBeginAnniversaryCangmingActualTurnStart(current))
            return;
        ContinueTurnAfterActualStart(current);
    }

    internal void ContinueTurnAfterActualStart(CharacterState current)
    {
        if (_characterSunQuanHappyGodOperations.TryBeginHappyLearningActualStart(current))
            return;
        if (_characterLuBuHappyGodOperations.TryBeginHappyLuBuActualStart(current))
            return;
        if (_characterLuBuOlMagicOperations.TryStartMoLuBuOpeningPlay(current))
            return;
        if (_coreIdentityConcealmentOperations.TryBeginIdentityAppearanceForTurn(current))
            return;
        _coreEndRefillNextTurnOperations.ConsumeEndRefillNextTurn(current);
        if (_coreRecoveryReplacementOperations.TryQueueHengyeRecovery(current))
            return;
        _characterLuMengMouOperations.ResolveHengyeTurnStart(current);
        ContinueTurnAfterHengye(current);
    }

    internal void ContinueTurnAfterHengye(CharacterState current)
    {
        if (_characterGuoHuaiMouMobileOperations.TryBeginMouGuoHuaiActualStart(current))
            return;
        if (_coreProgramLifecycleOperations.TryBeginRoundStartingProgramWindow(current))
            return;
        ContinueTurnAfterRoundPrograms(current);
    }

    internal void ContinueTurnAfterRoundPrograms(CharacterState current)
    {
        if (_coreForeignTurnContestsOperations.TryBeginForeignActualTurnStart(current))
            return;
        ContinueTurnAfterForeignActualContests(current);
    }

    internal void ContinueTurnAfterForeignActualContests(CharacterState current)
    {
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !current.IsAlive)
        {
            // Deferred offers have already paid and returned their HP/death
            // children. They may end this actor before Preparation even starts.
            _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
            if (_coreMatchOutcomeWinnerAccess._winner == Winner.None)
                FinalizeEndTurn(current);
            else
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_corePrivateCardOffersOperations.TryBeginDeferredPrivateOffers(current))
            return;
        _corePrepDiscardEndingPromisesOperations.ExpirePrepDiscardEndingPromises();
        _coreDeferredCardsOperations.ConsumeSkippedNextTurnDrawBenefits(current);
        _coreDeferredHandAlignmentsOperations.CleanupDeferredHandAlignments();
        _coreDeferredCardsOperations.CleanupLostDeferredPileSources();
        _corePublicPersistentPilesOperations.CleanupLostPublicPersistentPiles();
        if (current.IsFaceDown)
        {
            _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
            current.IsFaceDown = false;
            _coreCharacterStateProgramsOperations.RecordCharacterStateChange(current.Seat, SkillProgramTriggerWindow.CharacterTurnedFaceUp);
            _coreCharacterTurnedOverProgramsOperations.RecordCharacterTurnedOver(current.Seat, wasFaceDown: true);
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(current.Name)} 将武将牌翻回正面并跳过本回合。", current.Seat);
            if (!_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(continuation: CharacterStateContinuation.SkippedTurn))
                _coreCharacterStateProgramsOperations.CompleteFaceUpSkippedTurn(current);
            return;
        }

        _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Preparation, current.Seat);
        _coreDeferredBenefitsProgramsOperations.ApplyQueuedNextTurnRuleModifiers(current.Seat);
        _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects = global::CardGame.Core.GameEngine.DelayedTurnEffects.None;
        _characterZhangLiaoAnniversaryGodOperations.ApplyAnniversaryCuixiDrawSkip(current.Seat);
        if (_characterQiaoZhouOlOperations.TryBeginQiaoZhouPreparation(current))
            return;
        if (_coreProgramLifecycleOperations.TryBeginTurnStartProgramWindow(current))
        {
            return;
        }

        _coreProgramLifecycleOperations.BeginNormalTurnStartAfterProgramBindings(current);
    }

    internal void BeginTurnStartAfterProgramLifecycle(CharacterState current)
    {
        // The original Preparation child and its lifecycle have returned. A
        // lethal native use can already have decided the game in that child.
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return;
        if (_coreProgramRuntimeHooksOperations.RuntimeHooks.TryAdvance(ProgramRuntimePhase.BeforeJudgmentPhase))
            return;
        if (_coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Preparation, ActualPhaseEndContinuation.Preparation))
            return;
        if (_coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipJudgmentPhase))
        {
            _coreShanheS5ShensuOperations.RecordShanheS5ActualPhaseSkip(current.Seat, SkillProgramTurnPhase.Judgment);
            _characterZhuHuanBoundaryMobileOperations.RecordBoundaryZhuHuanSkippedPhase(current.Seat, SkillProgramTurnPhase.Judgment);
            CompleteTurnStart(current, _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects);
        }
        else
        {
            _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Judgment, current.Seat);
            if (!_coreCharacterStateProgramsOperations.TryBeginJudgmentPhaseStartingPrograms(current))
                BeginDelayedJudgmentOrTurnStart(current);
        }
    }

    internal void BeginDelayedJudgmentOrTurnStart(CharacterState current, int? excludedCardId = null)
    {
        if (_coreOriginalUseLightningOperations.TryBeginHeldOriginalUseLightning(current, excludedCardId))
            return;
        var delayedCard = _corePhysicalOwnedCardQueriesOperations.GetJudgment(current).FirstOrDefault(card => !_characterDaQiaoLeOperations.IsZixiQinJudgmentCard(card.Id) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(GetJudgmentEffectiveCardKind(card)) && card.Id != excludedCardId);
        if (delayedCard is not null)
        {
            var effectiveKind = GetJudgmentEffectiveCardKind(delayedCard);
            _coreDelayedNullificationOperations.BeginDelayedNullificationWindow(current, delayedCard, effectiveKind);
            return;
        }

        CompleteTurnStart(current, _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects);
    }

    internal void CompleteTurnStart(CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool skippedActualJudgment = false)
    {
        // Judgment-ending children can likewise finish the game before Draw.
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return;
        if (!skippedActualJudgment && _coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Judgment, ActualPhaseEndContinuation.Judgment, (int)delayedEffects))
            return;
        if (_characterKanZeOlOperations.ConsumeKuanshiDrawSkip(current.Seat))
        {
            delayedEffects |= global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;
            _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;
        }

        if (delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase))
            _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        else
            _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Draw, current.Seat);
        if (_coreDrawPhaseAttemptsOperations.TryBeginNormalDrawAttemptPhase(current, delayedEffects))
            return;
        if (delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase))
        {
            _coreMatchLogOperations.AddLog("DelayedCardEffect", $"{(current.Name)} 跳过摸牌阶段。", current.Seat);
        }
        else if (_coreProgramLifecycleOperations.TryBeginDrawPhaseProgramWindow(current, delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase)))
        {
            return;
        }
        else
        {
            _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, current.Seat);
            _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(current.Seat);
            _corePhysicalDrawFacadesOperations.DrawCards(current, _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(current), log: true);
        }

        CompleteTurnStartAfterDraw(current, delayedEffects);
    }

    internal int GetTurnDrawCount(CharacterState current) => global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreRuleQueriesOperations.EvaluateDrawCount(current));
    internal void CompleteDrawPhaseAfterProgramWindow(CharacterState current, bool skipPlayPhaseAfterDraw, bool normalDrawReplaced, int normalDrawAdjustment, int? frozenBaseDrawCount = null, bool confirmedActualDrawSubstitution = false)
    {
        if (confirmedActualDrawSubstitution)
        {
            CompleteTurnStartAfterDraw(current, _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects | global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase | (skipPlayPhaseAfterDraw ? global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase : global::CardGame.Core.GameEngine.DelayedTurnEffects.None));
            return;
        }

        if (_coreDrawPhaseAttemptsOperations.TryBeginNormalDrawAttemptPhase(current, skipPlayPhaseAfterDraw ? global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase : global::CardGame.Core.GameEngine.DelayedTurnEffects.None, afterStartingWindow: true, normalDrawReplaced, normalDrawAdjustment, frozenBaseDrawCount))
            return;
        _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, current.Seat);
        _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(current.Seat);
        var baseDrawCount = frozenBaseDrawCount ?? _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(current, normalDrawReplaced);
        if (!normalDrawReplaced)
            _corePhysicalDrawFacadesOperations.DrawCards(current, Math.Max(0, checked(baseDrawCount + normalDrawAdjustment)), log: true);
        CompleteTurnStartAfterDraw(current, skipPlayPhaseAfterDraw ? global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase : global::CardGame.Core.GameEngine.DelayedTurnEffects.None);
    }

    internal void CompleteTurnStartAfterDraw(CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false)
    {
        _coreNianActivityProgramsOperations.RecordNianActualPhaseSkips(current.Seat, delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase), delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase));
        if (delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase) && _programPipelineHooksOperations._programPipelineHooks.TryBeginPhaseSkipped(new(current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, SkillProgramTurnPhase.Draw, (int)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted)))
            return;
        if (!drawPhaseEndedProgramsCompleted && !actualDrawCompletionCompleted && _coreDrawPhaseObligationsOperations.TryBeginActualDrawCompletion(current, delayedEffects))
            return;
        if (!drawPhaseEndedProgramsCompleted && !delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase) && _coreProgramLifecycleOperations.TryBeginDrawPhaseEndedProgramWindow(current, delayedEffects))
            return;
        if (!delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase) && _coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Draw, ActualPhaseEndContinuation.Draw, (int)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted))
            return;
        if (!afterNormalDrawProgramsCompleted && !delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase) && _coreProgramLifecycleOperations.TryBeginAfterNormalDrawProgramWindow(current))
            return;
        if (_programPhaseScheduleOperations._programPhaseSchedule?.Phase == TurnPhase.Draw && _coreProgramLifecycleOperations.ResumeScheduledProgramPhase())
            return;
        if (delayedEffects.HasFlag(global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase))
        {
            if (_programPipelineHooksOperations._programPipelineHooks.TryBeginPhaseSkipped(new(current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, SkillProgramTurnPhase.Play, (int)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted)))
                return;
            _characterLiuQiOlOperations.ObserveLiuQiPlayBoundary(current.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, skipped: true);
            _coreActualDiscardPhaseOperations.BeginDiscardPhase();
            _coreMatchLogOperations.AddLog("DelayedCardEffect", $"{(current.Name)} 跳过出牌阶段。", current.Seat);
        }
        else
        {
            EnterPlayPhase(current);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void EnterPlayPhase(CharacterState current)
    {
        // Play-phase limits are distinct from turn-wide effects. Dangxian can
        // create two Play phases in one turn, so Slash count and phase-limited
        // actions reset here while Alcohol's once-per-turn flag remains intact.
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._slashCountThisTurn = 0;
        _coreCardUseDebitsOperations.ResetCardUseDebitPhase();
        // PlayEnding can insert Play before its normal state-reset return.
        // The new phase-grant contract resets shared instances on entry;
        // catalogs without that capability retain their existing replay flow.
        if (_corePlayPhaseSkillGrantsOperations.HasPlayPhaseSkillGrantCapability)
            _coreStrategicProgramsOperations.ResetPlayPhaseProgramStates(current.Seat);
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn = false;
        _coreSkillProgramsOperations.ResetProgramContributionUsesForPlayPhase(current.Seat);
        foreach (var key in _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.Keys.Where(key => key.Seat == current.Seat).ToArray())
            _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.Remove(key);
        _skillRuntimeStateOperations._skillRuntimeState.ResetPhase();
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = TurnPhase.Play;
        _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Play, current.Seat);
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._playPhaseKillCountByCurrentPlayer = 0;
        _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._playPhaseDamageDealtByCurrentPlayer = 0;
        _characterLiuQiOlOperations.ObserveLiuQiPlayBoundary(current.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, skipped: false);
        _coreGrantedEntityPhasesOperations.IssueGrantedEntityPhaseBoundary(current);
        _coreMatchLogOperations.AddLog("PhaseChanged", $"{(current.Name)} 进入出牌阶段。", current.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseChangedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase, current.Seat));
        _characterLvQianOlOperations.ObserveLvQianActualPlayEntry(current.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        _coreProgramLifecycleOperations.TryBeginPlayPhaseStartingBoundary(current);
    }

    internal CardKind GetJudgmentEffectiveCardKind(Card card) => _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds.GetValueOrDefault(card.Id, card.Kind);
    internal bool HasJudgmentEffectiveCard(CharacterState player, CardKind kind) => _corePhysicalOwnedCardQueriesOperations.GetJudgment(player).Any(card => _characterDaQiaoLeOperations.ZixiQinJudgmentSlot(card.Id)is { } slot ? slot == kind : GetJudgmentEffectiveCardKind(card) == kind) || kind == CardKind.Lightning && _coreOriginalUseLightningOperations.HasHeldOriginalUseLightning(player);
    internal static bool IsDelayedJudgmentContinuation(JudgmentContinuationKind continuation) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.IsDelayedJudgmentContinuation(continuation);
    internal static (string Reason, JudgmentContinuationKind Continuation) GetDelayedJudgmentInfo(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.GetDelayedJudgmentInfo(kind);
    internal static global::CardGame.Core.GameEngine.DelayedTurnEffects GetDelayedTurnEffects(CardKind kind) => kind switch
    {
        CardKind.Indulgence => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase,
        CardKind.SupplyShortage => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase,
        CardKind.Lightning => global::CardGame.Core.GameEngine.DelayedTurnEffects.None,
        _ => throw new ArgumentOutOfRangeException("kind", kind, "The card is not a delayed turn card.")};
    internal void EndTurn()
    {
        if (_characterLvQianOlOperations.TryBeginForcedWeiluPhaseEnd())
            return;
        if (_corePhaseHandSeizureOperations.TryBeginForcedPhaseHandDebtReturn())
            return;
        if (_characterLuBuOlMagicOperations.TryEndMoLuBuOpeningEarly())
            return;
        if (_coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Play, ActualPhaseEndContinuation.Discard))
            return;
        if (_coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Discard, ActualPhaseEndContinuation.Discard))
            return;
        if (_programPhaseScheduleOperations._programPhaseSchedule?.AfterTurnEndedReturn is not null)
        {
            _coreProgramLifecycleOperations.CompletePlayPhaseAfterProgramWindow();
            return;
        }

        var previous = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat];
        _coreProgramLifecycleOperations.CancelScheduledProgramPhase();
        _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Ending, previous.Seat);
        _characterHuaXiongMouMobileOperations.RecoverMouYangweiAtEnding(previous.Seat);
        if (_coreProgramLifecycleOperations.TryBeginTurnEndingBoundary(previous))
            return;
        FinalizeEndTurn(previous);
    }

    internal void FinalizeEndTurn(CharacterState previous)
    {
        if (previous.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            throw new InvalidOperationException("The end-turn owner changed before finalization.");
        if (_coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Ending, ActualPhaseEndContinuation.Ending))
            return;
        ExpireActualTurnEndEffects(previous);
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = TurnPhase.Finished;
        _coreMatchLogOperations.AddLog("TurnEnded", $"{(previous.Name)} 的回合结束。", previous.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnEndedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, previous.Seat));
        if (_coreDeferredHandAlignmentsOperations.TryBeginDeferredTurnEnd(previous, skipped: false))
            return;
        AdvanceAfterDeferredTurnEnd(previous, skipped: false);
    }

    internal void ExpireActualTurnEndEffects(CharacterState previous)
    {
        if (previous.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            throw new InvalidOperationException("Turn-end effects must expire for their actual turn owner.");
        if (previous.HasAlcoholEffect && !_characterRuanJiAnniversaryOperations.KeepsAnniversaryRuanJiAlcohol(previous.Seat))
        {
            previous.HasAlcoholEffect = false;
            _coreMatchLogOperations.AddLog("EffectExpired", $"{(previous.Name)} 的酒效在回合结束时失效。", previous.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(previous.Seat));
        }

        _characterKeBiNengOlOperations.ExpireKeBiNengPingduanAlcohol(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, previous.Seat);
        _coreCardUseModulesOperations.ExpireTurnCardUseEffects(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, previous.Seat);
        _coreStrategicProgramsOperations.ExpireProgramSuppressions(previous.Seat);
        _coreProgramHoldCardsOperations.ReturnProgramPojunHoldsAtTurnEnd();
        _corePrivateTurnHoldsOperations.ReturnPrivateTurnHoldsAtTurnEnd();
        _coreConvertingGiftsOperations.ExpireGiftRetentionObligations();
        _characterQianZhaoOlOperations.ExpireWeifuActualTurnGrants();
    }

    internal void AdvanceAfterDeferredTurnEnd(CharacterState previous, bool skipped)
    {
        if (_modeRuntimeOperations._modeRuntime.TryBeginIntermission(previous.Seat, skipped))
            return;
        ContinueAfterWushuangShopping(previous, skipped);
    }

    internal void ContinueAfterWushuangShopping(CharacterState previous, bool skipped)
    {
        if (_coreRoundEndingOperations.TryBeginRoundEndingBoundary(previous, skipped))
            return;
        CompleteActualTurnCursorAfterRoundEnding(previous, skipped);
    }

    internal void CompleteActualTurnCursorAfterRoundEnding(CharacterState previous, bool skipped)
    {
        if (_modeRuntimeOperations._modeRuntime.TryCompleteMatch(ModeCompletionBoundary.AfterTurnEnded))
            return;
        // Per-general tracking opts in declaratively through descriptor TracksActualTurnCursorReturn
        // markers; only engine-level aggregates and non-op dependencies stay listed here.
        if (_coreRoundEndingOperations.TracksRoundEnding || _coreActualPhaseEndsOperations.TracksActualPhaseEnds || _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal || _coreTreatedWoundedOperations.UsesTreatedWounded || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesLeiluanBasicUse || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveJueshengLimitedDuel) || ProgramOperationCatalog.Default.TracksActualTurnCursorReturn(_contentRegistryOperations._contentRegistry.ProgramDependencies))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnCursorReturnedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, previous.Seat));
        _coreActualTurnProgressionOperations.AdvanceActualTurnProgression(previous);
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = TurnPhase.NotStarted;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}

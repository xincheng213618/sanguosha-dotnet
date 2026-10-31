#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeAttackCompletionRuntime
{
    internal void CompleteAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null)
        {
            throw new InvalidOperationException("A card cannot finish while its dying resolution is pending.");
        }

        _characterSimaYiMobileGodOperations.ClearMobileSimaRenjieResponse(attack);
        if (attack.TryConsumeProgramDamageTransferFollowup(out var transferFollowup))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[transferFollowup.TargetSeat];
            var drawCount = transferFollowup.DrawLostHp && attack.DamageWasApplied && target.IsAlive ? Math.Max(0, target.MaxHp - target.Hp) : 0;
            if (drawCount > 0)
                _corePhysicalDrawFacadesOperations.DrawCards(target, drawCount, log: true, reason: new CardMoveReason("skill-program.damage-transfer.followup-draw"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageTransferCardsDrawnEvent(attack.ResolutionId, transferFollowup.SkillId, transferFollowup.OwnerSeat, transferFollowup.TargetSeat, drawCount));
        }

        if (!attack.IsProgramJudgmentDamage && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } group && CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, attack))
        {
            _characterLuXunWumiaoOperations.RetireWuXiongmuCardDamageCompleted(attack);
            group.CurrentAttack = null;
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
            group.TargetIndex++;
            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(group.ResolutionId, group.TargetIndex);
            _coreNativeGroupAttackOperations.BeginGroupAttackResponse(group);
            return;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreProgramAttackAttemptOperations.TryAdvanceChainedDamageTarget(attack, out var fromSeat))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedDamagePropagatedEvent(attack.ResolutionId, attack.SourceSeat, fromSeat, attack.TargetSeat, attack.DamageAmount, _coreNativeCardClassificationsOperations.GetDamageNature(attack)) { ProgramUseInstructionIndex = _coreProgramVirtualDuelOperations.DamageProgramUseInstructionIndex(attack) });
            _coreMatchLogOperations.AddLog("ChainDamage", $"连环伤害从 {(_coreMatchParticipantsPlayersAccess._players[fromSeat].Name)} 传导至 {(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)}。", attack.SourceSeat, attack.TargetSeat);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                CompleteAttack(attack);
            }

            return;
        }

        _characterLuXunWumiaoOperations.RetireWuXiongmuCardDamageCompleted(attack);
        if (_coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd is { } fangtian && CoreNativeDamageHandlesRuntime.SameAttackOwner(fangtian.CurrentAttack, attack))
        {
            fangtian.DamageWasApplied |= attack.DamageWasApplied;
            fangtian.CurrentAttack = null;
            if (fangtian.TargetIndex < fangtian.TargetSeats.Count)
                fangtian.TargetIndex++;
            while (fangtian.TargetIndex < fangtian.TargetSeats.Count && !_coreMatchParticipantsPlayersAccess._players[_coreCardActionsOperations.GetFangtianEffectiveTarget(fangtian, fangtian.TargetIndex)].IsAlive)
            {
                fangtian.TargetIndex++;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && fangtian.TargetIndex < fangtian.TargetSeats.Count)
            {
                _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
                _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
                _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
                _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(fangtian);
                return;
            }

            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(fangtian.ResolutionId, _coreOriginalTargetAdditionsOperations.CompletedFangtianOriginalTargetIndex(fangtian));
            attack.SetCardUseCausedDamage(fangtian.DamageWasApplied);
            _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = null;
        }

        if (!attack.CardUseCausedDamage)
            attack.SetCardUseCausedDamage(attack.DamageWasApplied);
        if (_characterGuanYuWumiaoOperations.TryCompleteWumiaoFloodAttack(attack))
            return;
        if (_characterXunYuMobileGodOperations.TryCompleteQizhengAttack(attack))
            return;
        if (_coreDragonBoatCardsOperations.TryCompleteDragonBoatAgainstCurrentAttack(attack))
            return;
        if (_coreSequentialTrickTargetsOperations.TryContinueSequentialTrick(attack.ResolutionId, afterAttack: true))
            return;
        if (_characterDongChengMouOperations.ContinueMouDcSlash(attack))
            return;
        if (_corePaidVirtualSlashTargetsOperations.TryContinuePaidVirtualSlashTargets(attack))
            return;
        if (_coreCurrentCardEnhancementsOperations.TryContinueEnhancedSlashTargets(attack))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(attack.ResolutionId, frame => frame with { ProgramAdjustedSlashBaseDamage = null });
        var completion = _coreCardAttackStateOperations.CaptureAttackCompletion(attack);
        if (_coreNativeSlashCompletionOperations.FinishAttack(attack))
        {
            return;
        }

        CompleteAttackAfterCardResolution(completion);
    }

    internal void CompleteAttackAfterCardResolution(GameEngine.AttackCompletionReceipt completion)
    {
        _coreOriginalUseLightningOperations.PreserveNativeDelayedAttackCompletion(completion);
        var delayedPhysicalExclusion = completion.DelayedTurn ? _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Single(f => f.Id == completion.ResolutionId).OriginalUseLightningExcludedCardId : null;
        if (_coreProgramNativeReturnsOperations.NativeReturns.ReturnAttack(NativeAttackReturnPhase.BeforeCleanup, completion))
            return;
        var borrowedSword = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } borrowedAttack } pendingBorrowedSword && borrowedAttack.ResolutionId == completion.ResolutionId ? pendingBorrowedSword : null;
        var resumesDelayedTurn = completion.DelayedTurn;
        var resumesProgramSkill = completion.ProgramFrameId is not null;
        if (completion.DelayedTurn)
            _coreRuntimeOperations.PopResolutionFrame(completion.ResolutionId, ResolutionFrameKind.Judgment);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        if (_coreProgramNativeReturnsOperations.NativeReturns.ReturnAttack(NativeAttackReturnPhase.AfterCleanup, completion))
            return;
        if (borrowedSword is not null)
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordAfterSlash(borrowedSword);
            return;
        }

        if (_coreProgramNativeReturnsOperations.NativeReturns.ReturnAttack(NativeAttackReturnPhase.AfterBorrowedSword, completion))
            return;
        if (resumesProgramSkill)
        {
            if (completion.ProgramFrameId is not { } frameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId)
                throw new InvalidOperationException("The program child attack lost its parent frame.");
            if (_coreProgramNativeReturnsOperations.NativeReturns.ReturnAttack(NativeAttackReturnPhase.BeforeParentResume, completion))
                return;
            if (completion.RequestedSlashChild && frame.WangYunPlan is null)
                _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { RequestedSlashDamagedOwner = frame.RoundOnceCardlessProgramRequest is not null ? _coreRoundOnceCardlessReturnsOperations.RoundOnceCardlessRequestedOwnerWasDamaged(frame, completion) : completion.DamageWasApplied && completion.FinalTargetSeat == frame.OwnerSeat });
            if (_coreRoundOnceCardlessProgramRequestsOperations.ResumeRoundOnceCardlessProgramRequest(frameId))
                return;
            _coreRuntimeOperations.AdvanceRuntimeProgram(frameId);
        }
        else if (resumesDelayedTurn && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
        {
            _coreNativeJudgmentOperations.ResumeAfterLightningDamage(completion.DelayedTurnSeat!.Value, delayedPhysicalExclusion);
        }
    }
}

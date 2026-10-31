#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeGroupRecoveryRuntime
{
    internal void RunOneGroupRecoveryStep()
    {
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("A group recovery step has no pending card.");
        if (group.Effect != GroupCardEffect.Recovery)
        {
            throw new InvalidOperationException("The pending group card is not a recovery effect.");
        }

        if (group.TargetIndex >= group.TargetSeats.Count)
        {
            _coreNativeGroupCompletionOperations.FinishGroupRecovery(group);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var target = _coreMatchParticipantsPlayersAccess._players[group.TargetSeats[group.TargetIndex]];
        if (target.IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(group.ResolutionId, target.Seat) && _coreGroupTargetNullificationOperations.Value.TryBeginGroupTargetNullification(group))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_characterLuXunMouMobileOperations.Value.TryBeginMouLuXunGroupEffect(group))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (target.IsAlive && target.HasHpValue && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(group.ResolutionId, target.Seat) && _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoAnyWounded(target.Seat))
        {
            var requestedRecovery = _coreShanheAlliedTricksOperations.ShanheAlliedPeachGardenRecovery(group, target.Seat, _coreWushuangTacticRulesOperations.HasWushuangTactic(group.SourceSeat, 2243) && target.Seat == group.SourceSeat ? 2 : 1);
            var recoveryAmount = _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(target.Seat) ? requestedRecovery : Math.Min(target.MaxHp - target.Hp, requestedRecovery);
            if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(group.ResolutionId, group.SourceSeat, target.Seat, recoveryAmount, new(RecoveryAttemptProducer.GroupCard)))
            {
                _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(group.ResolutionId, PostEventContinuation.GroupRecovery);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return;
            }

            var recoveryFrameId = _coreNativeRecoveryFramesOperations.BeginRecovery(group.ResolutionId, group.SourceSeat, target.Seat, recoveryAmount);
            try
            {
                target.Hp += recoveryAmount;
                _coreMatchLogOperations.AddLog("Recovered", $"{(target.Name)} 因【桃园结义】回复至 {(target.Hp)}/{(target.MaxHp)} 点体力。", group.SourceSeat, target.Seat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(group.SourceSeat, target.Seat, recoveryAmount, target.Hp));
            }
            finally
            {
                _coreRuntimeOperations.PopResolutionFrame(recoveryFrameId, ResolutionFrameKind.Recovery);
            }
        }

        if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(group.ResolutionId, PostEventContinuation.GroupRecovery))
            CompleteGroupRecoveryTarget();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void CompleteGroupRecoveryTarget()
    {
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("A recovery continuation lost its group card.");
        group.TargetIndex++;
        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(group.ResolutionId, group.TargetIndex);
        if (group.TargetIndex >= group.TargetSeats.Count)
            _coreNativeGroupCompletionOperations.FinishGroupRecovery(group);
    }
}

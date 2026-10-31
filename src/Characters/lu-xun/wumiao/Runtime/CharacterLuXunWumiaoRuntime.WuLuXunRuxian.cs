#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool HasWuLuXunRuxianCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWuLuXunRuxian);

    internal static CardConversionSource WuLuXunActionSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_xun_wumiao_WuLuXunRuxian.WuLuXunActionSource(f);
    internal int WuLuXunRuxianUsage(int ownerSeat) => !HasWuLuXunRuxianCapability ? 0 : _skillRuntimeStateOperations._skillRuntimeState.GetUsage(ownerSeat, "wumiao:ruxian", _contentRegistryOperations._contentRegistry.GetSkill("wumiao:ruxian").Program!.Activations.Single().UsageGroup, SkillUsageScope.Game);
    internal bool WuLuXunRuxianSpent(int ownerSeat) => WuLuXunRuxianUsage(ownerSeat) > 0;
    internal WuLuXunRuxianActivatedEvent? WuLuXunActiveRuxian(int ownerSeat)
    {
        if (!HasWuLuXunRuxianCapability)
            return null;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunRuxianActivatedEvent>().LastOrDefault(e => e.Source.OwnerSeat == ownerSeat);
        return issued is not null && !_coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Any(e => e.ActorSeat == ownerSeat && e.TurnNumber > issued.ActualTurn) ? issued : null;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 29003, SkillProgramEffectOp.ResolveWuLuXunRuxian)]
    internal bool CanActivateWuLuXunRuxian(ProgramActivationLegalityContext gate) => gate.Activation.Id == "all-ranks-until-next-turn" && _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None && gate.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == gate.OwnerSeat && !WuLuXunRuxianSpent(gate.OwnerSeat) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == gate.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase);
    internal SkillProgramStepOutcome ActivateWuLuXunRuxian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "wumiao:ruxian" || f.ActivationId != "all-ranks-until-next-turn" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || WuLuXunRuxianUsage(f.OwnerSeat) != 1 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunRuxianActivatedEvent>().Any(e => e.FrameId == f.Id) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            throw new InvalidOperationException("Ruxian requires its unpaid actual own Play activation; source replacement never restores its lifetime limit.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuXunRuxianActivatedEvent(f.Id, WuLuXunActionSource(f), f.GameplayHash, phase, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【儒贤】，至下回合开始前使用或打出任意点数的牌均可触发彰才。", f.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal bool HasZhaoZhongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolvePengbiFirstActualTurn) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveYintianFirstRecovery) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBiriFirstNonDrawGain);

    internal static CardConversionSource ZhaoZhongSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhao_zhong_ol_ZhaoZhongLedger.ZhaoZhongSource(f);
    internal bool ZhaoZhongActualTurnOpen => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28110)]
    internal bool CanOfferZhaoZhong(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (IsPengbiTrigger(t))
            return c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && context.OwnerSeat == c.OwnerSeat && context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && FirstPengbiActualTurn(c.OwnerSeat) == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<PengbiCommittedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat);
        if (IsBiriTrigger(t))
            return IsIssuedBiriCandidate(c, context);
        if (IsIssuedDiciChainTrigger(t))
            return IsIssuedDiciChainCandidate(c, context);
        return true;
    }
}

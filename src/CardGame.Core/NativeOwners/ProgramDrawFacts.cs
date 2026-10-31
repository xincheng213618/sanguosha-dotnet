#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDrawFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, KernelGetAttackRangeCall getAttackRange, Func<RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime, Func<RuntimeEntries.CoreActualDiscardPhaseLedgerEntries> getCoreActualDiscardPhaseLedgerRuntime, Func<RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime, Func<RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries> getCoreGameEngineProgramOwnedCardsRuntime, Func<RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, Func<RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime, Func<RuntimeEntries.CharacterWangYiBoundaryMobileEntries> getCharacterWangYiBoundaryMobileRuntime) : IProgramDrawFacts
{
    public bool HasHpValue(int seat) => matchParticipantsState.ReferenceToPlayers[seat].HasHpValue;
    public int MaxHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public string? EffectiveFaction(int seat) => getCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(matchParticipantsState.ReferenceToPlayers[seat]);
    public int AttackRange(int seat) => getAttackRange(seat);
    public long PairBenefitSequence => getCoreGameEnginePairObtainShownGiftsRuntime().PairBenefitSequence;
    public int TurnOwnerDiscardPhaseHandDiscardCount => getCoreActualDiscardPhaseLedgerRuntime().TurnOwnerDiscardPhaseHandDiscardCount;

    public int CategoryTargetTurnUsage(ProgramSkillFrame frame) => getCoreGameEngineProgramCategoryTargetLedgerRuntime().GetProgramCategoryTargetTurnUsage(frame);
    public int OwnerLostHp(ProgramSkillFrame frame) => getCoreGameEngineProgramOwnedCardsRuntime().GetProgramOwnerLostHp(frame);
    public int CurrentTurnCategoryDrawCount(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineCurrentTurnUseKindsRuntime().CurrentTurnCategoryDrawCount(frame, targetSeat);
    public int ActualTurnDeathDrawCount(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineForcedPhaseAlcoholRuntime().ActualTurnDeathDrawCount(frame, targetSeat);
    public int CommittedMijiCount(ProgramSkillFrame frame) => getCharacterWangYiBoundaryMobileRuntime().BoundaryWangYiMijiDrawCount(frame);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDrawFacts(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, global::CardGame.Core.NativeOwners.KernelGetAttackRangeCall getAttackRange, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries> getCoreActualDiscardPhaseLedgerRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries> getCoreGameEngineProgramOwnedCardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangYiBoundaryMobileEntries> getCharacterWangYiBoundaryMobileRuntime) : IProgramDrawFacts
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

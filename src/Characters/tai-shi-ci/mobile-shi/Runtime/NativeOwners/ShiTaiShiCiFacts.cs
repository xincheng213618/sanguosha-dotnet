#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiTaiShiCiFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, EngineStates.CharacterTaiShiCiMobileShiState characterTaiShiCiMobileShiState, KernelGetAttackRangeCall getAttackRange, CardAttackHandleFactory createCardAttackHandle, Func<RuntimeEntries.CoreActiveNativeAttackQueriesEntries> getCoreActiveNativeAttackQueriesRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IShiTaiShiCiFacts
{
    public ShiTaiShiCiPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].Name, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, matchParticipantsState.ReferenceToPlayers[seat].Hp, matchParticipantsState.ReferenceToPlayers[seat].MaxHp);
    public int LivingCount => matchParticipantsState.ReferenceToPlayers.Count(player => player.IsAlive);

    public bool ValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public bool HasActivationOperation(SkillProgramEffectOp operation) => getContentRegistry().ProgramDependencies.HasActivationOperation(operation);
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public IReadOnlyList<CardMovementRecord> Movements => cardMovementJournalState.ReferenceToCardMovements.AsReadOnly();
    public int LieScanSequence => characterTaiShiCiMobileShiState.ReferenceToShiTzLieScanSequence;

    public int AttackRange(int seat) => getAttackRange(seat);
    public ShiTaiShiCiAttack Attack(long frameId)
    {
        var attack = createCardAttackHandle(frameId);
        return new(attack.ResolutionId, attack.TargetSeat, attack.CardUserSeat, attack.EffectiveCardKind);
    }

    public long? ActiveAttackId => getCoreActiveNativeAttackQueriesRuntime().ActiveCardAttack?.ResolutionId;

    public IReadOnlyList<int> Targets(int owner, SkillProgramTargetKind kind) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTargetSeats(owner, kind);
    public IReadOnlyList<PromptChoice> HandDiscardChoices(long frameId, int chooser, int owner) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
    public bool Allies(int owner, int target) => EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(matchParticipantsState.ReferenceToPlayers[owner], matchParticipantsState.ReferenceToPlayers[target]);
    public int KeepValue(int owner, Card card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, matchParticipantsState.ReferenceToPlayers[owner]);
    public IReadOnlyList<IndexedSkillProgramInstance> ProgramInstances(int owner) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(matchParticipantsState.ReferenceToPlayers[owner]).ProgramInstances;
}

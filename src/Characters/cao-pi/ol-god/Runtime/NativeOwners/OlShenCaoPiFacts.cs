#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlShenCaoPiFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState,EngineStates.CoreCardMovementJournalState cardMovementJournalState,EngineStates.CoreMatchOutcomeState matchOutcomeState,Func<ContentRegistry> getContentRegistry,Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime,Func<RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime,Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime,Func<RuntimeEntries.CoreGameEnginePublicPileCashOutEntries> getCoreGameEnginePublicPileCashOutRuntime,Func<RuntimeEntries.CoreGameEngineGameActivationAwakeningEntries> getCoreGameEngineGameActivationAwakeningRuntime) : IOlShenCaoPiFacts
{
    public OlCaoPiPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, matchParticipantsState.ReferenceToPlayers[seat].MaxHp);
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<CardMovementRecord> Movements => cardMovementJournalState.ReferenceToCardMovements.AsReadOnly();

    public IReadOnlyList<SkillGrant> Grants(int seat) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grants;
    public IReadOnlyList<PublicPersistentPileSource> ReferencedPiles(int owner, string skillId, string instance) => getCoreGameEnginePublicPileSourcesRuntime().ReferencedPublicPileSources(owner, skillId, instance);
    public IReadOnlyList<Card> PileCards(PublicPersistentPileSource source) => getCoreGameEnginePublicPileSourcesRuntime().PublicPileCards(source);
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool StorageCard(int owner, string skillId, string instance, Card card, CardLocation location) => getCoreGameEnginePublicPileCashOutRuntime().PublicPileStorageCard(owner, skillId, instance, card, location);
    public bool ExactAwakeningParent(ProgramSkillFrame frame) => getCoreGameEngineGameActivationAwakeningRuntime().ExactGameActivationAwakeningParent(frame);
    public int KeepValue(int owner, Card card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, matchParticipantsState.ReferenceToPlayers[owner]);
}

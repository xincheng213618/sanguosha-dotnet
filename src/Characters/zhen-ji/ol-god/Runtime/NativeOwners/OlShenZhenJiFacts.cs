#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlShenZhenJiFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState,EngineStates.CoreCardMovementJournalState cardMovementJournalState,EngineStates.CoreMatchOutcomeState matchOutcomeState,EngineStates.CoreMatchParticipantsState matchParticipantsState,Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime,Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime,Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime,Func<ContentRegistry> getContentRegistry,Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : IOlShenZhenJiFacts
{
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public IReadOnlyList<OlShenfuPlayer> Players => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => new OlShenfuPlayer(player.Seat, player.Name, player.IsAlive, player.IsHuman, player.Hp)).ToArray());

    public OlShenfuPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].Name, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, matchParticipantsState.ReferenceToPlayers[seat].Hp);
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool HasSkillInstance(int seat, string skillId, string instance) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instance);
    public int RelativeOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public IReadOnlyList<PromptChoice> HandDiscardChoices(long frameId, int chooser, int owner) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
}

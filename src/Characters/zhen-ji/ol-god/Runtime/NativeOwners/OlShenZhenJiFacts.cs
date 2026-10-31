#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlShenZhenJiFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : IOlShenZhenJiFacts
{
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public IReadOnlyList<OlShenfuPlayer> Players => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => new OlShenfuPlayer(player.Seat, player.Name, player.IsAlive, player.IsHuman, player.Hp)).ToArray());

    public OlShenfuPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].Name, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, matchParticipantsState.ReferenceToPlayers[seat].Hp);
    public bool ValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool HasSkillInstance(int seat, string skillId, string instance) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instance);
    public int RelativeOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public IReadOnlyList<PromptChoice> HandDiscardChoices(long frameId, int chooser, int owner) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
}

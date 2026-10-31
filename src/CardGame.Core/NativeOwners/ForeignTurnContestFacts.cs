#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState,global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IForeignTurnContestFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public bool HasHpValue(int seat) => matchParticipantsState.ReferenceToPlayers[seat].HasHpValue;
    public bool IsRuleWounded(int seat) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool HasSkillInstance(int seat, string skillId, string instanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instanceId);
}

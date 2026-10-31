#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState,EngineStates.CoreMatchParticipantsState matchParticipantsState,Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime,Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IForeignTurnContestFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public bool HasHpValue(int seat) => matchParticipantsState.ReferenceToPlayers[seat].HasHpValue;
    public bool IsRuleWounded(int seat) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool HasSkillInstance(int seat, string skillId, string instanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instanceId);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PairObtainSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime) : IPairObtainSelectionQueries
{
    public string CurrentSkillInstance(int ownerSeat, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public bool CanObtainFrom(int ownerSeat, string skillId, string instanceId, int targetSeat) => getCoreGameEnginePairObtainShownGiftsRuntime().HasPayablePairObtainTarget(ownerSeat, skillId, instanceId, targetSeat);
}

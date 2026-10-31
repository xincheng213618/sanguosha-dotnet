#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PairObtainSelectionQueries(Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime) : IPairObtainSelectionQueries
{
    public string CurrentSkillInstance(int ownerSeat, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public bool CanObtainFrom(int ownerSeat, string skillId, string instanceId, int targetSeat) => getCoreGameEnginePairObtainShownGiftsRuntime().HasPayablePairObtainTarget(ownerSeat, skillId, instanceId, targetSeat);
}

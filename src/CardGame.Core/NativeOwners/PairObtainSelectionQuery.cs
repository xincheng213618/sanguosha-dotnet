#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PairObtainSelectionQuery(Func<RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IPairObtainSelectionQuery
{
    public bool CanSelect(int ownerSeat, string skillId, SkillProgramActivation activation, IReadOnlyList<int> targets) => getCoreGameEnginePairObtainShownGiftsRuntime().CanActivatePairObtain(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId), activation, targets);
}

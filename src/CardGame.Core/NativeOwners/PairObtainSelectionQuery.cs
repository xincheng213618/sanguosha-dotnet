#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PairObtainSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IPairObtainSelectionQuery
{
    public bool CanSelect(int ownerSeat, string skillId, SkillProgramActivation activation, IReadOnlyList<int> targets) => getCoreGameEnginePairObtainShownGiftsRuntime().CanActivatePairObtain(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId), activation, targets);
}

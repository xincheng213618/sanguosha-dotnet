#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DistinctFactionSelectionQuery(Func<RuntimeEntries.CoreGameEngineDistinctFactionDiscardsEntries> getCoreGameEngineDistinctFactionDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IDistinctFactionSelectionQuery
{
    public bool CanPay(int ownerSeat, string skillId) => getCoreGameEngineDistinctFactionDiscardsRuntime().HasPayableDistinctFactionOwnerHe(ownerSeat, skillId, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId));
}

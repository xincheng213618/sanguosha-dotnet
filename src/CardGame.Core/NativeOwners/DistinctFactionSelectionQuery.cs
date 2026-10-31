#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DistinctFactionSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDistinctFactionDiscardsEntries> getCoreGameEngineDistinctFactionDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IDistinctFactionSelectionQuery
{
    public bool CanPay(int ownerSeat, string skillId) => getCoreGameEngineDistinctFactionDiscardsRuntime().HasPayableDistinctFactionOwnerHe(ownerSeat, skillId, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId));
}

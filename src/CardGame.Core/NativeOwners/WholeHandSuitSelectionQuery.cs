#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WholeHandSuitSelectionQuery(Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineFriendTrioEntries> getCoreGameEngineFriendTrioRuntime, Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime) : IWholeHandSuitSelectionQuery
{
    public string SkillInstance(int ownerSeat, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public bool IsValid(int ownerSeat, IReadOnlyList<int> cards, CardConversionSource source) => getCoreGameEngineFriendTrioRuntime().ValidWholeHandSuitConversion(matchParticipantsState.ReferenceToPlayers[ownerSeat], cards.Select(getCoreGameEngineCardAttackStateRuntime().GetAttackCard).ToArray(), source);
}

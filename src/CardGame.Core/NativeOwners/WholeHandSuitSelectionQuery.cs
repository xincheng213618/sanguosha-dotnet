#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WholeHandSuitSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries> getCoreGameEngineFriendTrioRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime) : IWholeHandSuitSelectionQuery
{
    public string SkillInstance(int ownerSeat, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public bool IsValid(int ownerSeat, IReadOnlyList<int> cards, CardConversionSource source) => getCoreGameEngineFriendTrioRuntime().ValidWholeHandSuitConversion(matchParticipantsState.ReferenceToPlayers[ownerSeat], cards.Select(getCoreGameEngineCardAttackStateRuntime().GetAttackCard).ToArray(), source);
}

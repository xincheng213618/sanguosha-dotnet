#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectionWoundedQueries(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime) : ISelectionWoundedQueries
{
    public bool IdentityConcealed(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IdentityConcealed;
    public bool IsRuleWounded(int seat) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(matchParticipantsState.ReferenceToPlayers[seat]);
}

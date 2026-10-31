#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectionWoundedQueries(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime) : ISelectionWoundedQueries
{
    public bool IdentityConcealed(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IdentityConcealed;
    public bool IsRuleWounded(int seat) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(matchParticipantsState.ReferenceToPlayers[seat]);
}

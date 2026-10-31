#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BuZhiSelectionQueries(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : ISelectionRoleQueries
{
    public Role Role(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Role;
}

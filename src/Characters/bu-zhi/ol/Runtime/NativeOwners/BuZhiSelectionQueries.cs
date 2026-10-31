#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BuZhiSelectionQueries(EngineStates.CoreMatchParticipantsState matchParticipantsState) : ISelectionRoleQueries
{
    public Role Role(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Role;
}

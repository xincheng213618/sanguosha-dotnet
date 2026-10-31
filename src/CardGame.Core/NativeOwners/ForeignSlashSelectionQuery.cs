#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignSlashSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries> getCoreGameEngineForeignSelectedCardSlashRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IForeignSlashSelectionQuery
{
    public bool CanSelect(int ownerSeat, int targetSeat) => getCoreGameEngineForeignSelectedCardSlashRuntime().CanStartSelectedForeignCardSlash(matchParticipantsState.ReferenceToPlayers[ownerSeat], targetSeat);
}

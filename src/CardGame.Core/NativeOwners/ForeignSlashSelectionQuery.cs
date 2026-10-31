#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignSlashSelectionQuery(Func<RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries> getCoreGameEngineForeignSelectedCardSlashRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IForeignSlashSelectionQuery
{
    public bool CanSelect(int ownerSeat, int targetSeat) => getCoreGameEngineForeignSelectedCardSlashRuntime().CanStartSelectedForeignCardSlash(matchParticipantsState.ReferenceToPlayers[ownerSeat], targetSeat);
}

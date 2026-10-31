#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignSlashSelectionQueries(Func<RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries> getCoreGameEngineForeignSelectedCardSlashRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IForeignSlashSelectionQueries
{
    public bool CanStart(int ownerSeat, int actorSeat) => getCoreGameEngineForeignSelectedCardSlashRuntime().CanStartSelectedForeignCardSlash(matchParticipantsState.ReferenceToPlayers[ownerSeat], actorSeat);
}

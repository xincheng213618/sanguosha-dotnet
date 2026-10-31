#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignSlashSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries> getCoreGameEngineForeignSelectedCardSlashRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IForeignSlashSelectionQueries
{
    public bool CanStart(int ownerSeat, int actorSeat) => getCoreGameEngineForeignSelectedCardSlashRuntime().CanStartSelectedForeignCardSlash(matchParticipantsState.ReferenceToPlayers[ownerSeat], actorSeat);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DuelSelectionQueries(Func<RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries> getCoreGameEngineDamageColorDuelProgramsRuntime) : IDuelSelectionQueries
{
    public bool CanIssue(int ownerSeat, int actorSeat) => getCoreGameEngineDamageColorDuelProgramsRuntime().CanIssueSelectedActorDuel(ownerSeat, actorSeat);
}

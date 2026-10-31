#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DuelSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries> getCoreGameEngineDamageColorDuelProgramsRuntime) : IDuelSelectionQueries
{
    public bool CanIssue(int ownerSeat, int actorSeat) => getCoreGameEngineDamageColorDuelProgramsRuntime().CanIssueSelectedActorDuel(ownerSeat, actorSeat);
}

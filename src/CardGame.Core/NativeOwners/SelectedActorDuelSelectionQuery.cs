#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectedActorDuelSelectionQuery(Func<RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries> getCoreGameEngineDamageColorDuelProgramsRuntime) : ISelectedActorDuelSelectionQuery
{
    public bool CanSelect(int ownerSeat, int targetSeat) => getCoreGameEngineDamageColorDuelProgramsRuntime().CanIssueSelectedActorDuel(ownerSeat, targetSeat);
}

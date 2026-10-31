#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenYangOlRuntime
{
    internal int WenYangLvliLimit(int owner) => WenYangHasAwakened(owner, 1) && owner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat ? 2 : 1;
    internal int WenYangLvliUses(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenYangLvliIssuedEvent>().Count(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
}

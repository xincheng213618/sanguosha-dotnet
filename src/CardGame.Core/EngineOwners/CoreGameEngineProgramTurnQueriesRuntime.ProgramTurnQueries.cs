#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTurnQueriesRuntime
{
    int IProgramTurnQueries.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber;

    int IProgramTurnQueries.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat;

    int IProgramTurnQueries.TurnOwnerSeat => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int IProgramTurnQueries.TurnOwnerTurn => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber;
}

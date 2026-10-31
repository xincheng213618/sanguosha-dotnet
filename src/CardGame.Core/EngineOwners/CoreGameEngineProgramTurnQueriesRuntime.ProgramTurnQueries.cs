#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTurnQueriesRuntime
{
    int global::CardGame.Core.IProgramTurnQueries.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IProgramTurnQueries.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat;

    int global::CardGame.Core.IProgramTurnQueries.TurnOwnerSeat => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int global::CardGame.Core.IProgramTurnQueries.TurnOwnerTurn => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber;
}

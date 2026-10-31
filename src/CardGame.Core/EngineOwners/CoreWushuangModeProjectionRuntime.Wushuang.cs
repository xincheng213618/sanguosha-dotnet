#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreWushuangModeProjectionRuntime
{
    internal bool IsWushuangMode => _coreWushuangOperations.WushuangFeature is not null;
    internal bool IsWushuangClockRunning => _coreWushuangOperations.WushuangFeature is not null && _coreActualTurnProgressStartedTurnNumberAccess._started && _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
    internal int? WushuangRemainingMilliseconds => _coreWushuangOperations.WushuangFeature?.RemainingMilliseconds;

    internal WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat) => _coreWushuangOperations.WushuangFeature?.CreateWushuangSnapshot(viewerSeat)is { } shop ? shop with
    {
        RemainingTurns = Math.Max(0, _coreMatchDefinitionOptionsAccess._options.MaxTurns - _coreActualTurnProgressStartedTurnNumberAccess._turnNumber)
    }

    : null;
}

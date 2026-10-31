#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNianModeProjectionRuntime
{
    internal bool IsNianClockRunning => _coreNianChallengeHostOperations.NianFeature?.ClockRunning == true;
    internal int? NianRemainingMilliseconds => _coreNianChallengeHostOperations.NianFeature?.RemainingMilliseconds;
}

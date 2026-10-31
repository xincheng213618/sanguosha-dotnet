#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNianModeProjectionRuntime
{
    internal bool IsNianClockRunning => _coreNianChallengeHostOperations.Value.NianFeature?.ClockRunning == true;
    internal int? NianRemainingMilliseconds => _coreNianChallengeHostOperations.Value.NianFeature?.RemainingMilliseconds;
}

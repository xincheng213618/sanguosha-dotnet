#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNianModeProjectionRuntime
{
    internal bool IsNianClockRunning => _coreNianChallengeHostOperations.NianFeature?.ClockRunning == true;
    internal int? NianRemainingMilliseconds => _coreNianChallengeHostOperations.NianFeature?.RemainingMilliseconds;
}

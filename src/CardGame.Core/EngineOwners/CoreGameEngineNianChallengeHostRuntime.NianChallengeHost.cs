#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNianChallengeHostRuntime
{
    internal INianChallengeHost CreateNianChallengeHost() => _kernelCoreGameEngineNianChallengeHostOperations.CreateNianChallengeHost();
    internal INianChallengeFeature? NianFeature => _modeRuntimeOperations._modeRuntime.Find<INianChallengeFeature>();
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNianChallengeHostRuntime
{
    internal INianChallengeHost CreateNianChallengeHost() => _kernelCoreGameEngineNianChallengeHostOperations.CreateNianChallengeHost();
    internal INianChallengeFeature? NianFeature => _modeRuntimeOperations._modeRuntime.Find<INianChallengeFeature>();
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNianChallengeHostModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNianChallengeHostModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreNianChallengeHostKernelCoreGameEngineNianChallengeHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNianChallengeHostKernelCoreGameEngineNianChallengeHostOperations
    {
        public global::CardGame.Core.NativeOwners.NianChallengeHost CreateNianChallengeHost() => engine.ComposeNativeNianChallengeHost();
    }
}

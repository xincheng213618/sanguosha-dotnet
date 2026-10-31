#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreModeNativeFrameRuntimeModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeNativeFrameRuntimeModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }
}

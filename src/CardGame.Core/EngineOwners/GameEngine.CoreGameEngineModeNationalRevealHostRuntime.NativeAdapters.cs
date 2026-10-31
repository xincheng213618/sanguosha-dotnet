#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreModeNationalRevealHostKernelCoreGameEngineModeNationalRevealHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeNationalRevealHostKernelCoreGameEngineModeNationalRevealHostOperations
    {
        public global::CardGame.Core.NativeOwners.ModeNationalRevealHost CreateModeNationalRevealHost() => engine.ComposeNativeModeNationalRevealHost();
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreStandardBattleHostContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreStandardBattleHostContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreStandardBattleHostKernelCoreGameEngineStandardBattleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreStandardBattleHostKernelCoreGameEngineStandardBattleHostOperations
    {
        public global::CardGame.Core.NativeOwners.StandardBattleHost CreateStandardBattleHost() => engine.ComposeNativeStandardBattleHost();
    }
}

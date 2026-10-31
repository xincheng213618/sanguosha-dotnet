#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreWushuangKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWushuangKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
    }

    [ProgramCapability]
    private sealed class CoreWushuangModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWushuangModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreWushuangContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWushuangContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}

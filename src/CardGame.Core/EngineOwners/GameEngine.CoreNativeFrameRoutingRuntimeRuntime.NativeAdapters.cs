#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeFrameRoutingRuntimeKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFrameRoutingRuntimeKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CoreNativeFrameRoutingRuntimeKernelCoreGameEngineProgramContinuationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFrameRoutingRuntimeKernelCoreGameEngineProgramContinuationsOperations
    {
        public bool TryResume(global::CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, long frameId) => __nativeReceiver.TryResume(engine, frameId);
    }

    [ProgramCapability]
    private sealed class CoreNativeFrameRoutingRuntimeNativeCardGameCoreModeNativeFrameRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFrameRoutingRuntimeNativeCardGameCoreModeNativeFrameRuntimeOperations
    {
        public bool TryAdvance(global::CardGame.Core.ResolutionFrame frame) => engine.GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.TryAdvance(engine, frame);
    }
}

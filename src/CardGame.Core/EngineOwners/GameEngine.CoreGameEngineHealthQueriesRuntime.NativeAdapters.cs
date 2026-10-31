#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHealthQueriesKernelCoreGameEngineActionQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesKernelCoreGameEngineActionQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(global::CardGame.Core.NativeOwners.ActionQuery? previous = default) => engine.ComposeNativeActionQueryScope(previous);
    }

    [ProgramCapability]
    private sealed class CoreHealthQueriesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreHealthQueriesKernelCoreGameEngineProgramExpressionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesKernelCoreGameEngineProgramExpressionsOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(global::CardGame.Core.ProgramSkillFrame frame) => engine.ComposeNativeProgramExpressionWorld(frame);
    }
}

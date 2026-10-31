#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramExpressionBindingsKernelCoreGameEngineProgramExpressionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramExpressionBindingsKernelCoreGameEngineProgramExpressionsOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(global::CardGame.Core.ProgramSkillFrame frame) => engine.ComposeNativeProgramExpressionWorld(frame);
    }
}

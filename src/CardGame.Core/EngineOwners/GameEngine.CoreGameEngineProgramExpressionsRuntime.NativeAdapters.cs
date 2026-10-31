#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramExpressionsKernelCoreGameEngineProgramExpressionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramExpressionsKernelCoreGameEngineProgramExpressionsOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(global::CardGame.Core.ProgramSkillFrame frame) => engine.ComposeNativeProgramExpressionWorld(frame);
    }

    [ProgramCapability]
    private sealed class CoreProgramExpressionsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramExpressionsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}

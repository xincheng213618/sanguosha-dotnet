#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCommandInputAdaptersKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreCommandInputAdaptersResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCommandInputAdaptersKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreCommandInputAdaptersKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreManualDiscardRuntimePortsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreManualDiscardRuntimePortsKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
    }

    [ProgramCapability]
    private sealed class CoreManualDiscardRuntimePortsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreManualDiscardRuntimePortsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }
}

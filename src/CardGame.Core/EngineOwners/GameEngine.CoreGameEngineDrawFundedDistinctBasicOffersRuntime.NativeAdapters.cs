#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicOffersKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicOffersKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicOffersResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicOffersResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicOffersKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicOffersKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicOffersKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicOffersKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreDrawFundedDistinctBasicOffersContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicOffersContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}

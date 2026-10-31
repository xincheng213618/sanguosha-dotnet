#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRoundDistinctBasicPlayKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundDistinctBasicPlayKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CoreRoundDistinctBasicPlayContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundDistinctBasicPlayContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreRoundDistinctBasicPlayKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundDistinctBasicPlayKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreRoundDistinctBasicPlayKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundDistinctBasicPlayKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreRoundDistinctBasicPlayCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundDistinctBasicPlayCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}

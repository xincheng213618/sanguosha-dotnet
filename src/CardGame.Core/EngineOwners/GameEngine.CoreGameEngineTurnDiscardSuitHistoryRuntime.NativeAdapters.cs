#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTurnDiscardSuitHistoryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDiscardSuitHistoryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreTurnDiscardSuitHistoryCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDiscardSuitHistoryCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreTurnDiscardSuitHistoryKernelCoreProgramTriggerRuntimeQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDiscardSuitHistoryKernelCoreProgramTriggerRuntimeQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries() => engine.ComposeNativeProgramTriggerQueries();
    }

    [ProgramCapability]
    private sealed class CoreTurnDiscardSuitHistoryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDiscardSuitHistoryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}

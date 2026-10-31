#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSlashTargetPenaltyPaymentsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashTargetPenaltyPaymentsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSlashTargetPenaltyPaymentsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashTargetPenaltyPaymentsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSlashTargetPenaltyPaymentsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashTargetPenaltyPaymentsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSlashTargetPenaltyPaymentsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashTargetPenaltyPaymentsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}

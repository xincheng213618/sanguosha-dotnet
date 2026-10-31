#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRedDiscardRecoveryCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRedDiscardRecoveryCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreRedDiscardRecoveryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRedDiscardRecoveryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreRedDiscardRecoveryResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRedDiscardRecoveryResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRedDiscardRecoveryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRedDiscardRecoveryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHalfHandTargetSupportResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHalfHandTargetSupportResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreHalfHandTargetSupportCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHalfHandTargetSupportCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreHalfHandTargetSupportContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHalfHandTargetSupportContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreHalfHandTargetSupportKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHalfHandTargetSupportKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}

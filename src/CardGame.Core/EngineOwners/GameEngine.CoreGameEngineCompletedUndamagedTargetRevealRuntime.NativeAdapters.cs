#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}

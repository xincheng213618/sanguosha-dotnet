#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}

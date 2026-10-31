#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodCharacterCaoPiAnniversaryGodAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodCharacterCaoPiAnniversaryGodOperations
    {
        public bool KemingCixiongObserverEdge(int index) => engine.CaoPiAnniversaryGodEntries.KemingCixiongObserverEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterCaoPiAnniversaryGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoPiAnniversaryGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}

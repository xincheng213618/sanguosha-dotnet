#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicCharacterCaoCaoOlMagicAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicCharacterCaoCaoOlMagicOperations
    {
        public bool MoCaoCaoNativeEdge(int i) => engine.CaoCaoOlMagicEntries.MoCaoCaoNativeEdge(i);
        public bool MoCaoCaoFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.CaoCaoOlMagicEntries.MoCaoCaoFirstNativeChild(f, child);
        public bool MoCaoCaoNativeSuffix(int n) => engine.CaoCaoOlMagicEntries.MoCaoCaoNativeSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterCaoCaoOlMagicContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}

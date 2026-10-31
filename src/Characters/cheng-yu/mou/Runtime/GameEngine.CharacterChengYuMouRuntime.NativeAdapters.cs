#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterChengYuMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterChengYuMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterChengYuMouCharacterChengYuMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouCharacterChengYuMouOperations
    {
        public bool MouCyEdge(int i) => engine.ChengYuMouEntries.MouCyEdge(i);
        public bool MouCyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ChengYuMouEntries.MouCyFirstChild(f, child);
        public bool MouCySuffix(int n) => engine.ChengYuMouEntries.MouCySuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterChengYuMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterChengYuMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterChengYuMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}

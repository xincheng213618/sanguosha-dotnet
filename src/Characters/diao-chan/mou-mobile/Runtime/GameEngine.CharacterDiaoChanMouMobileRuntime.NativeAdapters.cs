#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterDiaoChanMouMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDiaoChanMouMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterDiaoChanMouMobileCharacterDiaoChanMouMobileAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDiaoChanMouMobileCharacterDiaoChanMouMobileOperations
    {
        public bool MouDiaoChanObserverEdge(int n) => engine.DiaoChanMouMobileEntries.MouDiaoChanObserverEdge(n);
        public bool MouDiaoChanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.DiaoChanMouMobileEntries.MouDiaoChanFirstChild(f, child);
        public bool MouDiaoChanObserverSuffix(int index) => engine.DiaoChanMouMobileEntries.MouDiaoChanObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterDiaoChanMouMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDiaoChanMouMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterDiaoChanMouMobileCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDiaoChanMouMobileCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterDiaoChanMouMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDiaoChanMouMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterDiaoChanMouMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDiaoChanMouMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}

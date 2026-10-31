#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiCharacterSunChenMobileShiAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiCharacterSunChenMobileShiOperations
    {
        public bool ShiScChildEdge(int i) => engine.SunChenMobileShiEntries.ShiScChildEdge(i);
        public bool ShiScFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SunChenMobileShiEntries.ShiScFirstChild(f, child);
        public bool ShiScChildSuffix(int n) => engine.SunChenMobileShiEntries.ShiScChildSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSunChenMobileShiCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunChenMobileShiCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterSunJunMobileShiCharacterSunJunMobileShiAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunJunMobileShiCharacterSunJunMobileShiOperations
    {
        public bool ShiSjEdge(int i) => engine.SunJunMobileShiEntries.ShiSjEdge(i);
        public bool ShiSjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SunJunMobileShiEntries.ShiSjFirstChild(f, child);
        public bool ShiSjSuffix(int n) => engine.SunJunMobileShiEntries.ShiSjSuffix(n);
    }
}

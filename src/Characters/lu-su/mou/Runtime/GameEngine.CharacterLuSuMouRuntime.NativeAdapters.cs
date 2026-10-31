#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLuSuMouCharacterLuSuMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouCharacterLuSuMouOperations
    {
        public bool MouLuSuEdge(int i) => engine.LuSuMouEntries.MouLuSuEdge(i);
        public bool MouLuSuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.LuSuMouEntries.MouLuSuFirstChild(f, child);
        public bool MouLuSuSuffix(int n) => engine.LuSuMouEntries.MouLuSuSuffix(n);
        public global::CardGame.Core.NativeOwners.MouLuSuAnniversaryFacts CreateMouLuSuAnniversaryFacts() => engine.ComposeNativeMouLuSuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMouKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLuSuMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuSuMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }
}

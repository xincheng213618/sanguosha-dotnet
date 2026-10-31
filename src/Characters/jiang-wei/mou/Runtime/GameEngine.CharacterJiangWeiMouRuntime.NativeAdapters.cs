#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterJiangWeiMouRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouCharacterJiangWeiMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouCharacterJiangWeiMouOperations
    {
        public bool MouJiangWeiAnniversaryEdge(int i) => engine.JiangWeiMouEntries.MouJiangWeiAnniversaryEdge(i);
        public bool MouJiangWeiAnniversaryFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.JiangWeiMouEntries.MouJiangWeiAnniversaryFirstChild(f, child);
        public bool MouJiangWeiAnniversarySuffix(int n) => engine.JiangWeiMouEntries.MouJiangWeiAnniversarySuffix(n);
        public global::CardGame.Core.NativeOwners.MouJiangWeiAnniversaryFacts CreateMouJiangWeiAnniversaryFacts() => engine.ComposeNativeMouJiangWeiAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterJiangWeiMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiangWeiMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}

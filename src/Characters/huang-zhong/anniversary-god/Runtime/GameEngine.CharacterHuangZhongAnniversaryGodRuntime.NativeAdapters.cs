#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodCoreProgramHealthChildQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodCoreProgramHealthChildQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramHealthChildQueries CreateProgramHealthChildQueries() => engine.ComposeNativeProgramHealthChildQueries();
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodCoreShanheSourceMouDongZhuoWineAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodCoreShanheSourceMouDongZhuoWineOperations
    {
        public bool IsShanheSourceMouWine(long id) => engine.ShanheSourceMouDongZhuoWineEntries.IsShanheSourceMouWine(id);
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodCharacterHuangZhongAnniversaryGodAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodCharacterHuangZhongAnniversaryGodOperations
    {
        public bool AnniversaryHzTianchongHealthChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.HuangZhongAnniversaryGodEntries.AnniversaryHzTianchongHealthChild(root, child);
        public bool AnniversaryHzTianchongSuffix(int rootIndex) => engine.HuangZhongAnniversaryGodEntries.AnniversaryHzTianchongSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterHuangZhongAnniversaryGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangZhongAnniversaryGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}

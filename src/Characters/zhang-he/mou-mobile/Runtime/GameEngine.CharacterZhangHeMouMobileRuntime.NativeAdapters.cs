#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileProgramPhaseScheduleAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileProgramPhaseScheduleOperations
    {
        public ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule => ref engine._programPhaseSchedule;
    }

    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhangHeMouMobileCharacterZhangHeMouMobileAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangHeMouMobileCharacterZhangHeMouMobileOperations
    {
        public bool MouQiaobianNativeEdge(int i) => engine.ZhangHeMouMobileEntries.MouQiaobianNativeEdge(i);
        public bool MouQiaobianFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ZhangHeMouMobileEntries.MouQiaobianFirstNativeChild(f, child);
        public bool MouQiaobianNativeSuffix(int n) => engine.ZhangHeMouMobileEntries.MouQiaobianNativeSuffix(n);
        public global::CardGame.Core.NativeOwners.MouZhangHeFacts CreateMouZhangHeFacts() => engine.ComposeNativeMouZhangHeFacts();
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
        public global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsCoreNativeEquipmentInvocationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsCoreNativeEquipmentInvocationsOperations
    {
        public bool NativeEquipmentObserverEdge(int index) => engine.NativeEquipmentInvocationsEntries.NativeEquipmentObserverEdge(index);
    }

    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeEquipmentInvocationsCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeEquipmentInvocationsCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}

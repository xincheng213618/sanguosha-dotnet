#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoActiveCardMovementBatchIdsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoActiveCardMovementBatchIdsOperations
    {
        public ref readonly global::System.Collections.Generic.Stack<long> _activeCardMovementBatchIds => ref engine._activeCardMovementBatchIds;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCharacterSunHongOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCharacterSunHongOlOperations
    {
        public bool ZenrunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SunHongOlEntries.ZenrunFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCoreRecoveryReplacementDamageRidesOperations2
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCharacterGuanYuWumiaoAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCharacterGuanYuWumiaoOperations
    {
        public bool WuYixianFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.GuanYuWumiaoEntries.WuYixianFirstChild(f, child);
        public bool WuYixianObserverSuffix(int index) => engine.GuanYuWumiaoEntries.WuYixianObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoKernelCoreMainAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoKernelCoreMainOperations2
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGuanYuWumiaoResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanYuWumiaoResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }
}

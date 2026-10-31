#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterChenLinMouKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class CharacterChenLinMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterChenLinMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterChenLinMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterChenLinMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterChenLinMouCharacterChenLinMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouCharacterChenLinMouOperations
    {
        public bool MouChenLinEdge(int i) => engine.ChenLinMouEntries.MouChenLinEdge(i);
        public bool MouChenLinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ChenLinMouEntries.MouChenLinFirstChild(f, child);
        public bool MouChenLinSuffix(int n) => engine.ChenLinMouEntries.MouChenLinSuffix(n);
        public global::CardGame.Core.NativeOwners.MouChenLinFacts CreateMouChenLinFacts() => engine.ComposeNativeMouChenLinFacts();
    }

    [ProgramCapability]
    private sealed class CharacterChenLinMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterChenLinMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }
}

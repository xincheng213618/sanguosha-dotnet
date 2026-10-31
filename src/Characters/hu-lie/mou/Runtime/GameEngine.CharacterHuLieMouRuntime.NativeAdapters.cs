#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterHuLieMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterHuLieMouKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class CharacterHuLieMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHuLieMouCharacterHuLieMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouCharacterHuLieMouOperations
    {
        public bool MouHuLieEdge(int i) => engine.HuLieMouEntries.MouHuLieEdge(i);
        public bool MouHuLieFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.HuLieMouEntries.MouHuLieFirstChild(f, child);
        public bool MouHuLieSuffix(int n) => engine.HuLieMouEntries.MouHuLieSuffix(n);
        public global::CardGame.Core.NativeOwners.MouHuLieFacts CreateMouHuLieFacts() => engine.ComposeNativeMouHuLieFacts();
    }

    [ProgramCapability]
    private sealed class CharacterHuLieMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuLieMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterHuLieMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuLieMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}

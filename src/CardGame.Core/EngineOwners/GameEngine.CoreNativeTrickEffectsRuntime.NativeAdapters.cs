#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeTrickEffectsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.GroupCardEffect effect, global::CardGame.Core.CardKind? requiredCardKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default) => engine.ComposeNativeGroupCardHandle(resolutionId, sourceSeat, card, targetSeats, effect, requiredCardKind, physicalCards);
        public global::CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, global::CardGame.Core.Card card) => engine.ComposeNativeBorrowedSwordHandle(resolutionId, sourceSeat, weaponOwnerSeat, slashTargetSeat, card);
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
    }

    [ProgramCapability]
    private sealed class CoreNativeTrickEffectsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeTrickEffectsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreNativeTrickEffectsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}

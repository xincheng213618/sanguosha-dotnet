#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreTieredRoundZeroUsesKernelCoreMainAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesKernelCoreMainOperations2
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }
}

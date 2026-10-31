#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramFactionActionRewardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramFactionActionRewardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureFactionAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasJudgmentEffectiveCard(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCardUseDistanceLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().GetCardUseDistanceLimit(player, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int GetAdjacentLivingSeat(int from, int step) => this._targetFactory().GetAdjacentLivingSeat(from, step);
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
    public void MoveTableSeatBetween(int owner, int previous, int next, long parentFrameId) => this._targetFactory().MoveTableSeatBetween(owner, previous, next, parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
    public bool TryMarkProgramUseCommitted(long frameId) => this._targetFactory().TryMarkProgramUseCommitted(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureProgramAdjustedSlashBaseDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BuildProgramOrdinaryTrickUseOptions(source, outputKind, physicalSuit, excludeOwner, enforceUsePermission, beneficiaryShieldSuit, actualEffectiveColor, hasActualColor, physicalCardIds, includeNextActualUseAdjustment, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginSlashTargetResolution(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreShownEntityTurnPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreShownEntityTurnPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _targetFactory = targetFactory;
    public bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider) => this._targetFactory().ShownEntityUseActorMatches(use, originalActor, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreShenwuEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreShenwuEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> _targetFactory = targetFactory;
    public bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind kind) => this._targetFactory().IsShenwuRobeTargetProhibited(actorSeat, targetSeat, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreTrustedCardZoneDiagnosticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreSkillOwnershipWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreSkillOwnershipWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginAdvancedSkillsChanged(long parentFrameId) => this._targetFactory().TryBeginAdvancedSkillsChanged(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void PendProgramExtraTurn(global::CardGame.Core.ProgramSkillFrame frame, int? targetSeat) => this._targetFactory().PendProgramExtraTurn(frame, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCardDistributionPresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCardDistributionPresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PresentCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string stage, int maximum = 2147483647, global::System.Func<global::CardGame.Core.PromptChoice, string>? destination = default, global::System.Func<global::CardGame.Core.PromptChoice, string>? label = default, int? scopeInstruction = default) => this._targetFactory().PresentCardDistribution(frame, choices, stage, maximum, destination, label, scopeInstruction);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    public bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => this._targetFactory().HasCardArmorBypass(actor, target, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreProgramNativeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreProgramNativeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreAlcoholSlashSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreAlcoholSlashSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _targetFactory = targetFactory;
    public void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor) => this._targetFactory().CaptureProgramAlcoholConsumption(cardUseFrameId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCharacterDengAiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCharacterDengAiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime> _targetFactory = targetFactory;
    public string? ShenDaTuoYuId(global::CardGame.Core.CharacterState owner) => this._targetFactory().ShenDaTuoYuId(owner);
    public int[] ShenDaAllocationCards(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ShenDaAllocationCards(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CharacterDengAiAnniversaryGodCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDengAiAnniversaryGodCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void PopFinishedCardUse(long frameId) => this._targetFactory().PopFinishedCardUse(frameId);
    public void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteFinishedAttackCardUse(attack);
}

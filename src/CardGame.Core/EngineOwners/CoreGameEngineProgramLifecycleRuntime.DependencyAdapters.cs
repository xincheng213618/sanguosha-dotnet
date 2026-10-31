#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public bool TryChooseKenshangAi(global::CardGame.Core.PendingDecision p, out global::CardGame.Core.PromptChoice choice) => this._targetFactory().TryChooseKenshangAi(p, out choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime))]
internal sealed class CoreProgramLifecycleCoreLostHpOwnedGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreLostHpOwnedGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _targetFactory = targetFactory;
    public void ResolveLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveLostHpOwnedGift(frame, effect, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime))]
internal sealed class CoreProgramLifecycleCoreDamageJudgmentSuitPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreDamageJudgmentSuitPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> _targetFactory = targetFactory;
    public void ResolveDamageJudgmentSuitPaymentChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveDamageJudgmentSuitPaymentChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramLifecycleCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreProgramLifecycleCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime> _targetFactory = targetFactory;
    public bool IsClaimableProgramOptionStillAvailable(global::CardGame.Core.PromptChoice selected) => this._targetFactory().IsClaimableProgramOptionStillAvailable(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreProgramLifecycleCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetLivingFactionCount() => this._targetFactory().GetLivingFactionCount();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseSchedulingRuntime))]
internal sealed class CoreProgramLifecycleProgramPhaseSchedulingAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramPhaseSchedulingOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseSchedulingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public bool TryChooseTousuiAi(global::CardGame.Core.PendingDecision p, out global::CardGame.Core.PromptChoice choice) => this._targetFactory().TryChooseTousuiAi(p, out choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreProgramLifecycleCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public bool TryFinishMoLuBuOpeningPlay() => this._targetFactory().TryFinishMoLuBuOpeningPlay();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreProgramLifecycleCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default) => this._targetFactory().HasTurnCardTargetRestriction(actorSeat, restriction, targetSeat);
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreTeamActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreTeamActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target) => this._targetFactory().PublicTeamEnemy(owner, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime))]
internal sealed class CoreProgramLifecycleCoreColdDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreColdDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> _targetFactory = targetFactory;
    public bool TryChooseColdDamageAi(global::CardGame.Core.PendingDecision decision, out global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryChooseColdDamageAi(decision, out selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreProgramLifecycleCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime))]
internal sealed class CoreProgramLifecycleCoreSlashSuitDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreSlashSuitDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _targetFactory = targetFactory;
    public bool IsProgramSlashSuitDiscardChoiceLegal(global::CardGame.Core.PromptChoice choice) => this._targetFactory().IsProgramSlashSuitDiscardChoiceLegal(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramBindingRuntime))]
internal sealed class CoreProgramLifecycleProgramBindingAdapter(global::System.Func<global::CardGame.Core.ProgramBindingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramBindingOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramBindingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramBindingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreProgramLifecycleCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreAlternativePhaseCostProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreAlternativePhaseCostProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> _targetFactory = targetFactory;
    public bool IsAlternativePhaseCostChoiceLegal(global::CardGame.Core.PromptChoice choice) => this._targetFactory().IsAlternativePhaseCostChoiceLegal(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreProgramLifecycleCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public int DamageInstancesTakenThisTurn(int ownerSeat) => this._targetFactory().DamageInstancesTakenThisTurn(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTargetSelectionRuntime))]
internal sealed class CoreProgramLifecycleProgramTargetSelectionAdapter(global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTargetSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetSelectionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterQingheGongzhuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterQingheGongzhuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateActualDodgeZengouForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().EstimateActualDodgeZengouForAi(owner, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramPersistentStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramPersistentStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _targetFactory = targetFactory;
    public bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId) => this._targetFactory().GetProgramBooleanState(ownerSeat, skillId, skillInstanceId, stateId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime))]
internal sealed class CoreProgramLifecycleCoreActualDiscardPhaseLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActualDiscardPhaseLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> _targetFactory = targetFactory;
    public int TurnOwnerDiscardPhaseHandDiscardCount { get => this._targetFactory().TurnOwnerDiscardPhaseHandDiscardCount; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramOwnedZoneDiscardRuntime))]
internal sealed class CoreProgramLifecycleProgramOwnedZoneDiscardAdapter(global::System.Func<global::CardGame.Core.ProgramOwnedZoneDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramOwnedZoneDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramOwnedZoneDiscardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramOwnedZoneDiscardRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreProgramLifecycleCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime))]
internal sealed class CoreProgramLifecycleCoreTargetedTrickHandHpAiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreTargetedTrickHandHpAiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedTrickHandHpAiRuntime> _targetFactory = targetFactory;
    public double GetTargetedTrickHandHpPublicScore(int owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().GetTargetedTrickHandHpPublicScore(owner, effects, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed class CoreProgramLifecycleCoreAttributedMarkerEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreAttributedMarkerEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramAiPublicContext CreateAttributedPaymentAiContext(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().CreateAttributedPaymentAiContext(owner, trigger);
    public bool HasAttributedEventOperations() => this._targetFactory().HasAttributedEventOperations();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairDrawsRuntime))]
internal sealed class CoreProgramLifecycleCoreEndingPairDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreEndingPairDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairDrawsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? PublishedEndingPairActorForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().PublishedEndingPairActorForAi(owner, effects, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void CompleteDying(global::CardGame.Core.DyingFrame dying, bool survived) => this._targetFactory().CompleteDying(dying, survived);
    public void ExposeHumanDyingPrompt() => this._targetFactory().ExposeHumanDyingPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime))]
internal sealed class CoreProgramLifecycleCoreCurrentTurnHandExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreCurrentTurnHandExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> _targetFactory = targetFactory;
    public void ResolveCurrentTurnHandExchangeChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice supplied) => this._targetFactory().ResolveCurrentTurnHandExchangeChoice(frame, supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterXiZhiCaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterXiZhiCaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXianfuEchoTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsXianfuEchoTrigger(trigger);
    public global::CardGame.Core.ProgramSkillWindowContext CreateXianfuDamageContext(global::CardGame.Core.DamageTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateXianfuDamageContext(window, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreProgramLifecycleCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public void ClearActualDiscardRecoveryPhase() => this._targetFactory().ClearActualDiscardRecoveryPhase();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramLifecycleCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime))]
internal sealed class CoreProgramLifecycleCoreActualEndedTurnEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActualEndedTurnEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> _targetFactory = targetFactory;
    public void ResolveActualEndedEquipmentChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveActualEndedEquipmentChoice(f, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime))]
internal sealed class CoreProgramLifecycleCorePlayPhaseSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePlayPhaseSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> _targetFactory = targetFactory;
    public void CloseCurrentPlayPhaseSkillGrants(string reason) => this._targetFactory().CloseCurrentPlayPhaseSkillGrants(reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramPhaseHandDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramPhaseHandDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramPhaseHandDiscardChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramPhaseHandDiscardChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreProgramLifecycleCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuJinMouMobileRuntime))]
internal sealed class CoreProgramLifecycleCharacterYuJinMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterYuJinMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMouMobileRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouXiayuanForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext context, global::CardGame.Core.ProgramSkillWindowContext? window) => this._targetFactory().EstimateMouXiayuanForAi(owner, context, window);
    public bool ValidMouXiayuan(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidMouXiayuan(f);
    public global::CardGame.Core.PromptChoice SelectAiMouXiayuanCost(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SelectAiMouXiayuanCost(p, f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramOwnCardDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramOwnCardDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramOwnCardDiscardChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramOwnCardDiscardChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime))]
internal sealed class CoreProgramLifecycleCoreActualPhaseEndsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActualPhaseEndsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> _targetFactory = targetFactory;
    public bool TryBeginActualPhaseEnd(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, global::CardGame.Core.ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default) => this._targetFactory().TryBeginActualPhaseEnd(kind, continuation, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted, parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed class CoreProgramLifecycleCoreActualDiscardPhaseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActualDiscardPhaseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _targetFactory = targetFactory;
    public void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false) => this._targetFactory().BeginDiscardPhase(beforeEntryPolicyCompleted);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed class CoreProgramLifecycleCorePublicDamagePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePublicDamagePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateAiSkillContext(global::CardGame.Core.CharacterState player, int viewerSeat, global::CardGame.Core.GameSnapshot? snapshot = default) => this._targetFactory().CreateAiSkillContext(player, viewerSeat, snapshot);
    public global::System.Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat) => this._targetFactory().CreatePublicSourceOccurrenceDamageBonus(viewerSeat);
    public global::CardGame.Core.PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, global::CardGame.Core.LegalAction action, int targetSeat) => this._targetFactory().PredictPublicActionDamage(viewerSeat, action, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreProgramLifecycleCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryChooseKemingEchoAi(global::CardGame.Core.PendingDecision decision, out global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryChooseKemingEchoAi(decision, out selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramLifecycleCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramOwnedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramOwnedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramOwnedCardSelection(frame, effect, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterLiJueOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterLiJueOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LangxiChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().LangxiChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime))]
internal sealed class CoreProgramLifecycleCoreCardUseDebitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreCardUseDebitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> _targetFactory = targetFactory;
    public bool IsCardUseDebitActive(long actionId) => this._targetFactory().IsCardUseDebitActive(actionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateZhaohuoDamageContext(global::CardGame.Core.DamageTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateZhaohuoDamageContext(window, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreProgramLifecycleCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramOwnedCardDistributionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramOwnedCardDistributionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramOwnedCardDistribution(frame, effect, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime))]
internal sealed class CoreProgramLifecycleCoreFixedRecipientBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreFixedRecipientBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? PublishedFixedRecipientForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().PublishedFixedRecipientForAi(owner, effects, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void AddThought(global::CardGame.Core.AiThoughtRecord thought) => this._targetFactory().AddThought(thought);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreProgramLifecycleCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void BeginTurnStartAfterProgramLifecycle(global::CardGame.Core.CharacterState current) => this._targetFactory().BeginTurnStartAfterProgramLifecycle(current);
    public void FinalizeEndTurn(global::CardGame.Core.CharacterState previous) => this._targetFactory().FinalizeEndTurn(previous);
    public int GetTurnDrawCount(global::CardGame.Core.CharacterState current) => this._targetFactory().GetTurnDrawCount(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerRuntime))]
internal sealed class CoreProgramLifecycleProgramTriggerAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTriggerOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentAttack(frame);
    public global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame) => this._targetFactory().GetDamageTriggerAttack(frame);
    public global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame) => this._targetFactory().GetDyingAttack(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreDyingEntryProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreDyingEntryProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying) => this._targetFactory().TryBeginDyingEntryProgramWindow(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramBoundaryInvariantRuntime))]
internal sealed class CoreProgramLifecycleProgramBoundaryInvariantAdapter(global::System.Func<global::CardGame.Core.ProgramBoundaryInvariantRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramBoundaryInvariantOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramBoundaryInvariantRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramBoundaryInvariantRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramRandomCardAcquisitionRuntime))]
internal sealed class CoreProgramLifecycleProgramRandomCardAcquisitionAdapter(global::System.Func<global::CardGame.Core.ProgramRandomCardAcquisitionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramRandomCardAcquisitionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramRandomCardAcquisitionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRandomCardAcquisitionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime))]
internal sealed class CoreProgramLifecycleCorePhaseHandSeizureReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePhaseHandSeizureReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> _targetFactory = targetFactory;
    public bool IsPhaseHandDebtChoiceLegal(global::CardGame.Core.PromptChoice choice) => this._targetFactory().IsPhaseHandDebtChoiceLegal(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime))]
internal sealed class CoreProgramLifecycleCorePublicFactionCountsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePublicFactionCountsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime> _targetFactory = targetFactory;
    public int GetPublicLivingFactionCount() => this._targetFactory().GetPublicLivingFactionCount();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreProgramLifecycleCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool UsesTreatedWounded { get => this._targetFactory().UsesTreatedWounded; }

    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDamageCardClaimRuntime))]
internal sealed class CoreProgramLifecycleProgramDamageCardClaimAdapter(global::System.Func<global::CardGame.Core.ProgramDamageCardClaimRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramDamageCardClaimOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDamageCardClaimRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageCardClaimRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramLifecycleCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreProgramLifecycleProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPindianRuntime))]
internal sealed class CoreProgramLifecycleProgramPindianAdapter(global::System.Func<global::CardGame.Core.ProgramPindianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramPindianOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPindianRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPindianRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterZhugeGuoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterZhugeGuoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QirangExtraChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().QirangExtraChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramRevealsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramRevealsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramRevealCardSelection(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramRevealCardSelection(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTurnWindowEntryRuntime))]
internal sealed class CoreProgramLifecycleProgramTurnWindowEntryAdapter(global::System.Func<global::CardGame.Core.ProgramTurnWindowEntryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTurnWindowEntryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTurnWindowEntryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTurnWindowEntryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerFactProjector))]
internal sealed class CoreProgramLifecycleProgramTriggerFactProjectorAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTriggerFactProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerFactProjector Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramJudgmentRuntime))]
internal sealed class CoreProgramLifecycleProgramJudgmentAdapter(global::System.Func<global::CardGame.Core.ProgramJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramJudgmentRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramCardUseNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramCardUseNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _targetFactory = targetFactory;
    public double GetWholeCardUseNullificationAiScore(int ownerSeat, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().GetWholeCardUseNullificationAiScore(ownerSeat, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramOtherOwnedCardDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramOtherOwnedCardDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramOtherOwnedCardDiscardChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramOtherOwnedCardDiscardChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreDiamondDelayedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreDiamondDelayedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> _targetFactory = targetFactory;
    public bool IsDiamondDelayedChoiceLegal(global::CardGame.Core.PromptChoice choice) => this._targetFactory().IsDiamondDelayedChoiceLegal(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDrawRuntime))]
internal sealed class CoreProgramLifecycleProgramDrawAdapter(global::System.Func<global::CardGame.Core.ProgramDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDrawRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramAttackRangeAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramAttackRangeAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> _targetFactory = targetFactory;
    public void ResolveProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramAttackRangeAid(frame, effect, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseBoundaryRuntime))]
internal sealed class CoreProgramLifecycleProgramPhaseBoundaryAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramPhaseBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreProgramLifecycleCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramSlashRequestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramSlashRequestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> _targetFactory = targetFactory;
    public void ResolveProgramRequestSlashChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramRequestSlashChoice(selected);
    public void ResolveProgramRequestSlashByNearestChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramRequestSlashByNearestChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime))]
internal sealed class CoreProgramLifecycleProgramDrawDiscardWindowEntryAdapter(global::System.Func<global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramDrawDiscardWindowEntryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime))]
internal sealed class CoreProgramLifecycleCoreNearestLegalSlashRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNearestLegalSlashRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> _targetFactory = targetFactory;
    public void ResolveNearestLegalSlashChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveNearestLegalSlashChoice(selected);
    public void ResolveUnlimitedVirtualSlashChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveUnlimitedVirtualSlashChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramParticipantPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramParticipantPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat) => this._targetFactory().GetPendingDecisionForActor(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime))]
internal sealed class CoreProgramLifecycleCorePrepDiscardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePrepDiscardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> _targetFactory = targetFactory;
    public void ResolvePrepDiscardChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolvePrepDiscardChoice(f, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreProgramLifecycleCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public void ResolvePairObtainChoice(global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolvePairObtainChoice(choice);
    public void ResolveShownPairGiftChoice(global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveShownPairGiftChoice(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime))]
internal sealed class CoreProgramLifecycleCoreHpDamageShieldsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreHpDamageShieldsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> _targetFactory = targetFactory;
    public double GetHpDamageShieldAiTargetValue(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetHpDamageShieldAiTargetValue(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreStrategicProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreStrategicProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> _targetFactory = targetFactory;
    public void ResetPlayPhaseProgramStates(int ownerSeat) => this._targetFactory().ResetPlayPhaseProgramStates(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterBaoXinOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterBaoXinOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateBaoXinYimouContext(global::CardGame.Core.DamageTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary) => this._targetFactory().CreateBaoXinYimouContext(frame, candidate, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramLifecycleCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime))]
internal sealed class CoreProgramLifecycleCoreCurrentTurnUseKindsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreCurrentTurnUseKindsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _targetFactory = targetFactory;
    public bool TracksCurrentTurnUseKinds { get => this._targetFactory().TracksCurrentTurnUseKinds; }

    public (int HandSuits, int Categories) CurrentTurnUseKinds(int actorSeat) => this._targetFactory().CurrentTurnUseKinds(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime))]
internal sealed class CoreProgramLifecycleCoreEndRefillNextTurnAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreEndRefillNextTurnOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> _targetFactory = targetFactory;
    public double EndRefillNextTurnPublicScore(int owner) => this._targetFactory().EndRefillNextTurnPublicScore(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramHealthStateRuntime))]
internal sealed class CoreProgramLifecycleProgramHealthStateAdapter(global::System.Func<global::CardGame.Core.ProgramHealthStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramHealthStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramHealthStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramHealthStateRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramHoldCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramHoldCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramHoldCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramHoldCardSelection(frame, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime))]
internal sealed class CoreProgramLifecycleCoreTurnDrawDebtsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreTurnDrawDebtsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> _targetFactory = targetFactory;
    public bool TracksTurnDrawDebt { get => this._targetFactory().TracksTurnDrawDebt; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramBoundCardRuntime))]
internal sealed class CoreProgramLifecycleProgramBoundCardAdapter(global::System.Func<global::CardGame.Core.ProgramBoundCardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramBoundCardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramBoundCardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramBoundCardRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramLifecycleCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPairedColorResponseAiRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramPairedColorResponseAiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPairedColorResponseAiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramPairedColorResponseAiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPairedColorResponseAiRuntime> _targetFactory = targetFactory;
    public double GetPairedColorDispositionAiScore(global::CardGame.Core.SkillProgramTrigger trigger, int owner, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().GetPairedColorDispositionAiScore(trigger, owner, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreQuotaTopProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreQuotaTopProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> _targetFactory = targetFactory;
    public int ComputeTurnCriterionQuota(int ownerSeat) => this._targetFactory().ComputeTurnCriterionQuota(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed class CoreProgramLifecycleCoreSoleTargetProtectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreSoleTargetProtectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _targetFactory = targetFactory;
    public void ResolveSoleProtectionChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveSoleProtectionChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime))]
internal sealed class CoreProgramLifecycleCoreDyingRescueContinuationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreDyingRescueContinuationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingRescueContinuationsRuntime> _targetFactory = targetFactory;
    public bool ProgramPaidDyingRescue(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.DyingFrame dying) => this._targetFactory().ProgramPaidDyingRescue(frame, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramActivationWindowRuntime))]
internal sealed class CoreProgramLifecycleProgramActivationWindowAdapter(global::System.Func<global::CardGame.Core.ProgramActivationWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramActivationWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramActivationWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActivationWindowRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreProgramLifecycleCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public bool WouldEquipmentRemovalReduceCoverage(int subjectSeat, int equipmentCardId) => this._targetFactory().WouldEquipmentRemovalReduceCoverage(subjectSeat, equipmentCardId);
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
    public void ResolveSelectAndMoveOwnedCardChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveSelectAndMoveOwnedCardChoice(frame, effect, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime))]
internal sealed class CoreProgramLifecycleCoreActivityOrdinaryTricksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreActivityOrdinaryTricksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ContentSkillDefinition ActivityRuntimeSkillDefinition(string id) => this._targetFactory().ActivityRuntimeSkillDefinition(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed class CoreProgramLifecycleCoreRoundOnceCardlessProgramRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreRoundOnceCardlessProgramRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _targetFactory = targetFactory;
    public bool TryResolveRoundOnceCardlessProgramSlash(global::CardGame.Core.PromptChoice choice, bool advance) => this._targetFactory().TryResolveRoundOnceCardlessProgramSlash(choice, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreProgramLifecycleCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouKurouGiftForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext publicContext) => this._targetFactory().EstimateMouKurouGiftForAi(owner, publicContext);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreProgramLifecycleCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public bool IsDianhuDrawTrigger(global::CardGame.Core.SkillProgramTrigger t) => this._targetFactory().IsDianhuDrawTrigger(t);
    public global::CardGame.Core.ProgramSkillWindowContext CreateDianhuDamageContext(global::CardGame.Core.DamageTriggerWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateDianhuDamageContext(w, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeDyingFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeDyingFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame UpdateDyingFrame(long frameId, global::System.Func<global::CardGame.Core.DyingFrame, global::CardGame.Core.DyingFrame> update) => this._targetFactory().UpdateDyingFrame(frameId, update);
    public void SetDyingFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetDyingFrameStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime))]
internal sealed class CoreProgramLifecycleCorePhaseHandExtraTurnAiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePhaseHandExtraTurnAiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandExtraTurnAiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramAiEstimate IncludePhaseHandExtraTurnDeclarationForAi(global::CardGame.Core.ProgramAiEstimate baseline, global::CardGame.Core.CharacterState owner, string skillId, string skillInstanceId, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger declaration, global::CardGame.Core.ProgramAiPublicContext context, global::CardGame.Core.ProgramSkillWindowContext windowContext) => this._targetFactory().IncludePhaseHandExtraTurnDeclarationForAi(baseline, owner, skillId, skillInstanceId, program, declaration, context, windowContext);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime))]
internal sealed class CoreProgramLifecycleCoreEquipmentDonationProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreEquipmentDonationProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> _targetFactory = targetFactory;
    public void ResolveEquipmentDonationChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveEquipmentDonationChoice(f, choice);
}

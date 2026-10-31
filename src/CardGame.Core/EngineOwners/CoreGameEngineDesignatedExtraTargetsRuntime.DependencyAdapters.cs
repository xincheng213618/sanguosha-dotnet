#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public bool CanBeGuanduAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeGuanduAdditionalTarget(use, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => this._targetFactory().IsCurrentUsePhysicalCardClaim(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasWeifuTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default) => this._targetFactory().TryQueueRecoveryReplacement(parentFrameId, sourceSeat, targetSeat, amount, completion, happyQiaoRecoverToOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public bool ValidBeiMiHuTargetMutation(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ValidBeiMiHuTargetMutation(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreDesignatedExtraTargetsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public bool HasMoQuanyuAddedTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasMoQuanyuAddedTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public bool CanBeWenheAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeWenheAdditionalTarget(use, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDesignatedExtraTargetsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreProgramCardUseRolesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreProgramCardUseRolesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _targetFactory = targetFactory;
    public void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => this._targetFactory().UpdateProgramRoleCardUse(use, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor) => this._targetFactory().RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasChenglieTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreCardRecoveryModifiersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreCardRecoveryModifiersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> _targetFactory = targetFactory;
    public int ApplyIssuedCardRecoveryModifiers(long useId, int actorSeat, int targetSeat, int baseAmount) => this._targetFactory().ApplyIssuedCardRecoveryModifiers(useId, actorSeat, targetSeat, baseAmount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasPijingTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool CanBeDragonBoatAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeDragonBoatAdditionalTarget(use, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreNativeRecoveryFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreNativeRecoveryFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> _targetFactory = targetFactory;
    public long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount) => this._targetFactory().BeginRecovery(parentFrameId, sourceSeat, targetSeat, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsShenDaCuixinUse(long id) => this._targetFactory().IsShenDaCuixinUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CanUseSlashTarget(source, target, slashCard, conversionSource, effectiveKind, ignoreDistance, existingUseFrameId, noEffectiveRank, specificEffectiveRank, physicalCardIds, excludedEquipmentId, excludedEquipmentSlot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => this._targetFactory().CreateCardActionProgramContext(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public bool HasTargetCard(global::CardGame.Core.CharacterState player) => this._targetFactory().HasTargetCard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterWuYiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterWuYiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryWuYiTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> preceding) => this._targetFactory().IsBoundaryWuYiTargetFact(use, fact, preceding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
    public void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueSimpleCardUse(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasBingzhongTargetTail(use);
    public bool TryGetSuyiVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().TryGetSuyiVirtualPrimaryReturn(use, parent, out targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreVirtualOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreVirtualOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public bool IsProgramVirtualOrdinaryTrickUse(long frameId) => this._targetFactory().IsProgramVirtualOrdinaryTrickUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId) => this._targetFactory().IsProgramAlternativeCost(action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool IsTieredRoundZeroUse(long id) => this._targetFactory().IsTieredRoundZeroUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWeapon(player);
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreExtraCardEffectRoundsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreExtraCardEffectRoundsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _targetFactory = targetFactory;
    public void ApplyExtraCapableAlcoholEffect(long id, global::CardGame.Core.CharacterState target) => this._targetFactory().ApplyExtraCapableAlcoholEffect(id, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreAdjacentRedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreAdjacentRedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _targetFactory = targetFactory;
    public bool HasAdjacentRedTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasAdjacentRedTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreRecipientCategorySlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreRecipientCategorySlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> _targetFactory = targetFactory;
    public bool HasRecipientCategorySlashTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasRecipientCategorySlashTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsSelfTargetForbiddenAction(actor, kind, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsExchangedCardClaim(long actionId, int cardId) => this._targetFactory().IsExchangedCardClaim(actionId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreDamageTargetObtainProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreDamageTargetObtainProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _targetFactory = targetFactory;
    public bool IsIssuedZeroEntityDuel(long id) => this._targetFactory().IsIssuedZeroEntityDuel(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreForeignPublicPileSlashesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreForeignPublicPileSlashesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> _targetFactory = targetFactory;
    public bool IsForeignPublicPileSlashUse(long resolutionId) => this._targetFactory().IsForeignPublicPileSlashUse(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreVirtualBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreVirtualBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _targetFactory = targetFactory;
    public void FinishVirtualBasicUse(long id) => this._targetFactory().FinishVirtualBasicUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed class CoreDesignatedExtraTargetsCharacterChengYuMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCharacterChengYuMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouLidanTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasMouLidanTargetTail(use);
    public bool IsMouDanchiUse(long id) => this._targetFactory().IsMouDanchiUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreDamageTargetBudgetAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreDamageTargetBudgetOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _targetFactory = targetFactory;
    public bool HasDamageTargetBudgetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasDamageTargetBudgetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiashengRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreShanheXiashengAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiashengRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreShanheXiashengOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiashengRuntime> _targetFactory = targetFactory;
    public bool IsShanheXiashengTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => this._targetFactory().IsShanheXiashengTargetFact(use, fact, originalTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreProgramCardTargetEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreProgramCardTargetEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _targetFactory = targetFactory;
    public bool IsCardEffectIneffective(long resolutionId, int targetSeat) => this._targetFactory().IsCardEffectIneffective(resolutionId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreDesignatedExtraTargetsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDesignatedExtraTargetsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

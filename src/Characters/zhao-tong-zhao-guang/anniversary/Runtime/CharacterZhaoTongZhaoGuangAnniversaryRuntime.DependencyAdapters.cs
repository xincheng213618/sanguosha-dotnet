#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool HasSelfCardTargetProhibition(int seat) => this._targetFactory().HasSelfCardTargetProhibition(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreProgramNativeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreProgramNativeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreSkillOwnershipWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreSkillOwnershipWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginAdvancedSkillsChanged(long parentFrameId) => this._targetFactory().TryBeginAdvancedSkillsChanged(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCharacterGuoJiaMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCharacterGuoJiaMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> _targetFactory = targetFactory;
    public bool HasShenGuoJiaAwakeningCondition(int seat, string skill, string? binding = default) => this._targetFactory().HasShenGuoJiaAwakeningCondition(seat, skill, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind EffectiveConversionInputKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveConversionInputKind(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    public string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description) => this._targetFactory().DescribeConversion(source, description);
    public global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source) => this._targetFactory().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default) => this._targetFactory().ResolveSlashCore(source, target, slash, playedCardKind, physicalOwnerSeat, factionRequest, borrowedSword, physicalCards, countsTowardSlashLimit, usesZhuqueFan, conversionSource, additionalConversionSources, programSkillCardUseFrameId, physicalSourceLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor) => this._targetFactory().RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreGuanHaiBridgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreGuanHaiBridgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardSuit(owner, selection);
    public bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardColor(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
    public bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().HasRuntimeSkill(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreObserverSubtreesOperations2
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => this._targetFactory().HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreTurnTargetCardQuotaAllowancesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreTurnTargetCardQuotaAllowancesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> _targetFactory = targetFactory;
    public bool HasTargetCardQuotaAllowance(int actor, int target) => this._targetFactory().HasTargetCardQuotaAllowance(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreAdvancedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreAdvancedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasNextUnlimitedCard(owner);
    public bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasFriendQihuiAllowance(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().CanUseJianjiOrdinaryAlcohol(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreRoundGainedSourceUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreRoundGainedSourceUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _targetFactory = targetFactory;
    public bool HasRoundGainedBasicBonus(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().HasRoundGainedBasicBonus(owner, kind, ids);
    public bool HasIssuedRoundGainedBasicBonus(long useId, int actor) => this._targetFactory().HasIssuedRoundGainedBasicBonus(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandError? ValidateProgramSelection(global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().ValidateProgramSelection(action, cards, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreProgramSkillChangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreProgramSkillChangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _targetFactory = targetFactory;
    public void CommitProgramSkillChangeFact(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.IGameEvent fact) => this._targetFactory().CommitProgramSkillChangeFact(frame, fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasMouWujingAllowance(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreAfterTurnEndedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreAfterTurnEndedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _targetFactory = targetFactory;
    public bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().MatchesAfterTurnEndedChild(parent, child, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().BeginSimpleCardUse(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

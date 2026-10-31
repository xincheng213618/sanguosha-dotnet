#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CharacterPangTongFriendCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CharacterPangTongFriendCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CharacterPangTongFriendCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CharacterPangTongFriendCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CharacterPangTongFriendCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CharacterPangTongFriendCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    public bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => this._targetFactory().HasCardArmorBypass(actor, target, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CharacterPangTongFriendCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsResponseEntityRestricted(int seat, int id) => this._targetFactory().IsResponseEntityRestricted(seat, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CharacterPangTongFriendCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureProgramAdjustedSlashBaseDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CharacterPangTongFriendCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed class CharacterPangTongFriendCoreAlcoholSlashSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreAlcoholSlashSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _targetFactory = targetFactory;
    public void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor) => this._targetFactory().CaptureProgramAlcoholConsumption(cardUseFrameId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CharacterPangTongFriendCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
    public void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().BeginSimpleCardUse(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CharacterPangTongFriendCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public bool EnsureDrawPile() => this._targetFactory().EnsureDrawPile();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CharacterPangTongFriendCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CharacterPangTongFriendCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public void SelectUseConversion(global::CardGame.Core.LegalAction action) => this._targetFactory().SelectUseConversion(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime))]
internal sealed class CharacterPangTongFriendCoreOutsidePhaseDrawDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreOutsidePhaseDrawDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> _targetFactory = targetFactory;
    public bool ValidNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationProof proof) => this._targetFactory().ValidNativeDrawInvocation(proof);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CharacterPangTongFriendCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().HasRuntimeSkill(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CharacterPangTongFriendCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool HasSelfCardTargetProhibition(int seat) => this._targetFactory().HasSelfCardTargetProhibition(seat);
    public bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsSelfTargetForbiddenAction(actor, kind, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed class CharacterPangTongFriendCoreFriendTrioAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreFriendTrioOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> _targetFactory = targetFactory;
    public string FriendCategoryValue(global::CardGame.Core.CardKind kind) => this._targetFactory().FriendCategoryValue(kind);
    public bool HasFriendTrioAlly(global::CardGame.Core.CharacterState owner, string id) => this._targetFactory().HasFriendTrioAlly(owner, id);
    public bool DrainFriendTrioChildren(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().DrainFriendTrioChildren(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CharacterPangTongFriendCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CharacterPangTongFriendCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CharacterPangTongFriendCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeEquipmentUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeEquipmentUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> _targetFactory = targetFactory;
    public void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId) => this._targetFactory().CompleteEquipmentUse(source, equipment, resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CharacterPangTongFriendCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CharacterPangTongFriendCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasJudgmentEffectiveCard(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime))]
internal sealed class CharacterPangTongFriendCoreDeckProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreDeckProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> _targetFactory = targetFactory;
    public bool DeckProgramSourceValid(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().DeckProgramSourceValid(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginSlashTargetResolution(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CharacterPangTongFriendCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CanUseSlashTarget(source, target, slashCard, conversionSource, effectiveKind, ignoreDistance, existingUseFrameId, noEffectiveRank, specificEffectiveRank, physicalCardIds, excludedEquipmentId, excludedEquipmentSlot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterPangTongFriendCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed class CharacterPangTongFriendCoreProgramOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreProgramOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BuildProgramOrdinaryTrickUseOptions(source, outputKind, physicalSuit, excludeOwner, enforceUsePermission, beneficiaryShieldSuit, actualEffectiveColor, hasActualColor, physicalCardIds, includeNextActualUseAdjustment, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed class CharacterPangTongFriendCoreFinalizedCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreFinalizedCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _targetFactory = targetFactory;
    public bool TryBeginEquipmentTargetPrograms(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long frameId) => this._targetFactory().TryBeginEquipmentTargetPrograms(source, equipment, frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CharacterPangTongFriendCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary) => this._targetFactory().EventsSinceLastBoundary(boundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetSeatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetSeatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CharacterPangTongFriendCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNextCardTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNextCardTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public void AddNextCardTargetAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => this._targetFactory().AddNextCardTargetAdjustmentActions(actions, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreActualTurnDamageClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreActualTurnDamageClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> _targetFactory = targetFactory;
    public bool MatchesDeclaredActualDamageUse(global::CardGame.Core.CardUseFrame use, int actor, int provider) => this._targetFactory().MatchesDeclaredActualDamageUse(use, actor, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreProgramOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreProgramOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BuildProgramOrdinaryTrickUseOptions(source, outputKind, physicalSuit, excludeOwner, enforceUsePermission, beneficiaryShieldSuit, actualEffectiveColor, hasActualColor, physicalCardIds, includeNextActualUseAdjustment, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterWoLongFengChuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterWoLongFengChuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> PaidEquipmentSlotNamedZeroRules(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind? output, bool responseUse, bool dyingUse) => this._targetFactory().PaidEquipmentSlotNamedZeroRules(owner, output, responseUse, dyingUse);
    public bool IsEquipmentSlotNamedActionPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().IsEquipmentSlotNamedActionPayable(actor, action);
    public bool TryBeginEquipmentSlotNamedPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryBeginEquipmentSlotNamedPlay(actor, action);
    public bool IsEquipmentSlotNamedForcedSlashPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.CharacterState target, bool qinglong) => this._targetFactory().IsEquipmentSlotNamedForcedSlashPayable(actor, source, kind, target, qinglong);
    public bool TryBeginEquipmentSlotNamedForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.PromptChoice choice, bool qinglong) => this._targetFactory().TryBeginEquipmentSlotNamedForcedSlash(actor, source, kind, choice, qinglong);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCardUseDebitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCardUseDebitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> _targetFactory = targetFactory;
    public void RecordSlashUseDebit(long cardUseFrameId, int actorSeat) => this._targetFactory().RecordSlashUseDebit(cardUseFrameId, actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().CanUseJianjiOrdinaryAlcohol(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public void AddNextActualUseAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => this._targetFactory().AddNextActualUseAdjustmentActions(actions, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public bool TryMarkProgramUseCommitted(long frameId) => this._targetFactory().TryMarkProgramUseCommitted(frameId);
    public void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().BeginSimpleCardUse(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureProgramAdjustedSlashBaseDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().FinishCardUse(frameId, card, playedCardKind);
    public void PopFinishedCardUse(long frameId) => this._targetFactory().PopFinishedCardUse(frameId);
    public void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteFinishedAttackCardUse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeTrickUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeTrickUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _targetFactory = targetFactory;
    public void CompleteAlcoholUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card alcohol, long resolutionId) => this._targetFactory().CompleteAlcoholUse(source, alcohol, resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreRedAdditionalTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreRedAdditionalTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> _targetFactory = targetFactory;
    public bool IsTargetAdjustmentAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().IsTargetAdjustmentAction(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public void ResolveDragonBoatCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().ResolveDragonBoatCard(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreTurnTargetCardQuotaAllowancesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreTurnTargetCardQuotaAllowancesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> _targetFactory = targetFactory;
    public bool HasTargetCardQuotaAllowance(int actor, int target) => this._targetFactory().HasTargetCardQuotaAllowance(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public bool MatchesTieredRoundZeroFireAttackAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().MatchesTieredRoundZeroFireAttackAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreAdvancedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreAdvancedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasNextUnlimitedCard(owner);
    public bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasFriendQihuiAllowance(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreGuanduBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreGuanduBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> _targetFactory = targetFactory;
    public void AddGuanduZeroBasicActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source) => this._targetFactory().AddGuanduZeroBasicActions(actions, actor, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreProgramTieredRoundConversionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreProgramTieredRoundConversionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime> _targetFactory = targetFactory;
    public bool CanUseTieredRoundConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool responseUse, bool dyingUse) => this._targetFactory().CanUseTieredRoundConversion(actor, instance, rule, responseUse, dyingUse);
    public int TieredRoundTier(int ownerSeat, string stateId) => this._targetFactory().TieredRoundTier(ownerSeat, stateId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => this._targetFactory().NotifyAiOfSlash(source, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeQinglongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeQinglongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _targetFactory = targetFactory;
    public bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().CanUseQinglongCrescentBladeTarget(source, target, effectiveKind, effectiveColor, allowAnyColor, physicalSuit, effectiveRank, physicalCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreQinglongInsertionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreQinglongInsertionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> _targetFactory = targetFactory;
    public void SuspendContinuationForQinglongFollowup(long outerResolutionId, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? followupRequest = default) => this._targetFactory().SuspendContinuationForQinglongFollowup(outerResolutionId, followupRequest);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWeapon(player);
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertCurrentSlashFirePolicy(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor) => this._targetFactory().RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
    public void CompleteRecoveryCardUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, long resolutionId, global::CardGame.Core.CardKind playedCardKind, int recoveryAmount, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource> recoveryPolicySources) => this._targetFactory().CompleteRecoveryCardUse(source, target, card, resolutionId, playedCardKind, recoveryAmount, recoveryPolicySources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreActivityOrdinaryTricksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreActivityOrdinaryTricksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgram ActivityRuntimeProgram(global::CardGame.Core.SkillProgram original) => this._targetFactory().ActivityRuntimeProgram(original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginSlashTargetResolution(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreDragonBoatBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreDragonBoatBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> _targetFactory = targetFactory;
    public void AddDragonBoatZeroBasicActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => this._targetFactory().AddDragonBoatZeroBasicActions(actions, actor, source, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreAlcoholSlashSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreAlcoholSlashSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _targetFactory = targetFactory;
    public void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor) => this._targetFactory().CaptureProgramAlcoholConsumption(cardUseFrameId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool HasSelfCardTargetProhibition(int seat) => this._targetFactory().HasSelfCardTargetProhibition(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterKeBiNengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterKeBiNengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _targetFactory = targetFactory;
    public bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsKeBiNengPingduanBasicQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterKongRongMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterKongRongMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouKrLiwenUseQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsMouKrLiwenUseQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreTieredRoundZeroUsesCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterMaTieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterMaTieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryMaTieSlashQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsAnniversaryMaTieSlashQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeMultiTargetActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeMultiTargetActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0) => this._targetFactory().GetFangtianOrderedSlashTargets(actor, slash, conversionSource, ignoresSlashLimit, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreTieredRoundZeroUsesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public void ResolveGuanduCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().ResolveGuanduCard(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasMouWujingAllowance(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreProgramCardUseRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreProgramCardUseRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> _targetFactory = targetFactory;
    public void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddProgramTargetCountSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description) => this._targetFactory().DescribeConversion(source, description);
    public void SelectUseConversion(global::CardGame.Core.LegalAction action) => this._targetFactory().SelectUseConversion(action);
    public string ProgramConversionName(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ProgramConversionName(source);
    public void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice) => this._targetFactory().CaptureSelectedResponseConversion(choice);
    public bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IgnoresProgramSlashLimit(owner, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterZhouYuOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterZhouYuOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _targetFactory = targetFactory;
    public bool IsMouGuquCardlessAllowed(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool forResponse, bool dyingUse = false) => this._targetFactory().IsMouGuquCardlessAllowed(actor, kind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreTieredRoundZeroUsesCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouLuSuUseQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsMouLuSuUseQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreTieredRoundZeroUsesCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroUsesCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => this._targetFactory().HasCardArmorBypass(actor, target, kind);
}

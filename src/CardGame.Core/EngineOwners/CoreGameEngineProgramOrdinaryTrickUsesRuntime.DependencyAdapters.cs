#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PhysicalGroupColor(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCharacterHuangfuSongWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCharacterHuangfuSongWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuHfOrDixianMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.CardConversionSource? source = default) => this._targetFactory().HasWuHfOrDixianMaterialPrivilege(actor, kind, ids, source);
    public bool HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, long? useId = default) => this._targetFactory().HasWuHfOrWuyouMaterialPrivilege(actor, kind, ids, useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreProgramMultiTargetDuelsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreProgramMultiTargetDuelsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IReadOnlyList<int>> EnumerateProgramDuelTargets(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> candidates, int minimum = 1) => this._targetFactory().EnumerateProgramDuelTargets(actor, candidates, minimum);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCharacterSunQuanAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCharacterSunQuanAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryChouxiSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsAnniversaryChouxiSource(owner, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public bool HasPhaseSuitAllowance(int seat, global::CardGame.Core.Suit? suit) => this._targetFactory().HasPhaseSuitAllowance(seat, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public bool IsNextActualUseTrickForbidden(int seat, global::CardGame.Core.CardKind kind) => this._targetFactory().IsNextActualUseTrickForbidden(seat, kind);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> NextActualUseOrdinaryTrickOptions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ordinary, global::CardGame.Core.Suit? suit, bool? color, global::CardGame.Core.Suit? shieldSuit, bool excludeOwner) => this._targetFactory().NextActualUseOrdinaryTrickOptions(actor, ordinary, suit, color, shieldSuit, excludeOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCardRestricted(player, card);
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool CanUseGlobalCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool excludeOwner = false) => this._targetFactory().CanUseGlobalCard(actor, kind, excludeOwner);
    public bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IReadOnlyList<int>> GuanduTargetChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit, bool? color, bool excludeOwner = false) => this._targetFactory().GuanduTargetChoices(actor, kind, suit, color, excludeOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedPlayPhaseUseBan(int seat) => this._targetFactory().HasIssuedPlayPhaseUseBan(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool DragonBoatCardAllies(int first, int second) => this._targetFactory().DragonBoatCardAllies(first, second);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsSelfTargetForbiddenAction(actor, kind, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed) => this._targetFactory().IsBlackTrickTargetProhibited(target, kind, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWeapon(player);
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreProgramIronChainTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreProgramIronChainTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IReadOnlyList<int>> EnumerateProgramIronChainTargets(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> candidates) => this._targetFactory().EnumerateProgramIronChainTargets(actor, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> HellReturnSoulTargets(int actorSeat) => this._targetFactory().HellReturnSoulTargets(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public bool HasTargetCard(global::CardGame.Core.CharacterState player) => this._targetFactory().HasTargetCard(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetJudgment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreShenwuEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreShenwuEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> _targetFactory = targetFactory;
    public bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind kind) => this._targetFactory().IsShenwuRobeTargetProhibited(actorSeat, targetSeat, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> WenheOrdinaryTrickKinds { get => this._targetFactory().WenheOrdinaryTrickKinds; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool SiruiIgnoresTrickDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<int>? materialIds = default) => this._targetFactory().SiruiIgnoresTrickDistance(actor, source, materialIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed class CoreProgramOrdinaryTrickUsesCoreProvenanceCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCoreProvenanceCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds) => this._targetFactory().HasProvenanceUseDistance(owner, physicalIds);
}

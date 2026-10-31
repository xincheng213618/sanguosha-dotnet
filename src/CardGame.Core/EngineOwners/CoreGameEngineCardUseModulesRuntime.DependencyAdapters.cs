#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime))]
internal sealed class CoreCardUseModulesCorePhaseStartControlPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCorePhaseStartControlPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsOtherPlayRestrictionUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => this._targetFactory().IsOtherPlayRestrictionUseForbidden(actor, kind, type);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime))]
internal sealed class CoreCardUseModulesCoreProgramPhaseColorRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreProgramPhaseColorRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsPlayPhasePhysicalCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsPlayPhasePhysicalCardRestricted(player, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime))]
internal sealed class CoreCardUseModulesCharacterSimaYiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCharacterSimaYiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> _targetFactory = targetFactory;
    public bool AllowsHappySimaPeach(int actor) => this._targetFactory().AllowsHappySimaPeach(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime))]
internal sealed class CoreCardUseModulesCharacterYuanTanYuanShangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCharacterYuanTanYuanShangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> _targetFactory = targetFactory;
    public bool IsNeifaUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => this._targetFactory().IsNeifaUseForbidden(actor, kind, type);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreCardUseModulesCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime))]
internal sealed class CoreCardUseModulesCoreFixedDistanceOneDebtAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreFixedDistanceOneDebtOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> _targetFactory = targetFactory;
    public void ExpireFixedDistanceOneGrants(int turn, int seat) => this._targetFactory().ExpireFixedDistanceOneGrants(turn, seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed class CoreCardUseModulesCoreAlcoholSlashSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreAlcoholSlashSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _targetFactory = targetFactory;
    public void ExpireCurrentTurnOwnSkillSuppressions(int turnNumber, int turnOwnerSeat) => this._targetFactory().ExpireCurrentTurnOwnSkillSuppressions(turnNumber, turnOwnerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreCardUseModulesCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public bool IsNextActualUseCounterspellForbidden(int seat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => this._targetFactory().IsNextActualUseCounterspellForbidden(seat, kind, type);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime))]
internal sealed class CoreCardUseModulesCharacterSunQuanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCharacterSunQuanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyLearningPlaySelfOnly(int actor) => this._targetFactory().HasHappyLearningPlaySelfOnly(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreCardUseModulesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPlayableCards(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime))]
internal sealed class CoreCardUseModulesCoreOriginalTargetAdditionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreOriginalTargetAdditionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> _targetFactory = targetFactory;
    public void ExpireOriginalTargetAdditionGrants(int turn, int seat) => this._targetFactory().ExpireOriginalTargetAdditionGrants(turn, seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreCardUseModulesCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsExclusiveTurnPeachUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => this._targetFactory().IsExclusiveTurnPeachUseForbidden(actorSeat, kind, type);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime))]
internal sealed class CoreCardUseModulesCoreProgramBooleanStateExpirationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreProgramBooleanStateExpirationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> _targetFactory = targetFactory;
    public void ExpireProgramBooleanStateLeases(int turnNumber, int turnSeat) => this._targetFactory().ExpireProgramBooleanStateLeases(turnNumber, turnSeat);
    public void ExpireProgramBooleanStateLeasesAtOwnerTurnStart(int ownerSeat, bool ownerDied = false) => this._targetFactory().ExpireProgramBooleanStateLeasesAtOwnerTurnStart(ownerSeat, ownerDied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreCardUseModulesCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsProgramResponseCardUse(responder, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime))]
internal sealed class CoreCardUseModulesCoreParticipantAndReserveProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreParticipantAndReserveProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> _targetFactory = targetFactory;
    public void ExpireAttributedNatureMarkers(int ownerSeat) => this._targetFactory().ExpireAttributedNatureMarkers(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreCardUseModulesCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
    public void ExpireDirectedTurnCardPolicies(int turnNumber, int turnSeat) => this._targetFactory().ExpireDirectedTurnCardPolicies(turnNumber, turnSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime))]
internal sealed class CoreCardUseModulesCharacterZhangLingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCharacterZhangLingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime> _targetFactory = targetFactory;
    public bool IsCategoryTalismanUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType type) => this._targetFactory().IsCategoryTalismanUseForbidden(actorSeat, effectiveKind, type);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCardUseModulesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreCardUseModulesCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextSlashProgramsRuntime))]
internal sealed class CoreCardUseModulesCoreNextSlashProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextSlashProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreNextSlashProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextSlashProgramsRuntime> _targetFactory = targetFactory;
    public void ExpireNextSlashDamage(int turnNumber, int turnSeat) => this._targetFactory().ExpireNextSlashDamage(turnNumber, turnSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreCardUseModulesCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsSelfTargetForbiddenAction(actor, kind, targets);
    public global::System.Collections.Generic.IReadOnlyList<int> GetSelfProhibitionPolicyTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().GetSelfProhibitionPolicyTargets(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed class CoreCardUseModulesCoreCurrentTurnSkillSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreCurrentTurnSkillSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _targetFactory = targetFactory;
    public void ExpireCurrentTurnNonLockedSkillSuppressions(int turnNumber, int turnOwnerSeat) => this._targetFactory().ExpireCurrentTurnNonLockedSkillSuppressions(turnNumber, turnOwnerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CoreCardUseModulesCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignTurnAlcoholUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => this._targetFactory().IsForeignTurnAlcoholUseForbidden(actorSeat, kind, type);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreCardUseModulesCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCategoryRestricted(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCategoryRestricted(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreCardUseModulesCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public bool IsMouSmPlayedActor(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsMouSmPlayedActor(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardUseModulesCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramHandPhysicalRestricted(actorSeat, cardId);
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime))]
internal sealed class CoreCardUseModulesCoreBoundDiscardSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreBoundDiscardSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> _targetFactory = targetFactory;
    public void ResolveFirstRoundGameUsageRefunds(int turn, int turnSeat) => this._targetFactory().ResolveFirstRoundGameUsageRefunds(turn, turnSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreCardUseModulesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind) => this._targetFactory().GetProgramCardSet(frame, bind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreCardUseModulesCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedPlayPhaseUseBan(int seat) => this._targetFactory().HasIssuedPlayPhaseUseBan(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreCardUseModulesCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsResponseEntityRestricted(int seat, int id) => this._targetFactory().IsResponseEntityRestricted(seat, id);
}

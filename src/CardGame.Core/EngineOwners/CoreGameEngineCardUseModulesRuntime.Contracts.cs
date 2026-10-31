#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardUseModulesCorePhaseStartControlPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOtherPlayRestrictionUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreCardUseModulesCoreProgramPhaseColorRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPlayPhasePhysicalCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreCardUseModulesCharacterSimaYiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AllowsHappySimaPeach(int actor);
}

internal interface ICoreCardUseModulesCharacterYuanTanYuanShangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNeifaUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreCardUseModulesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardUseModulesCoreFixedDistanceOneDebtOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireFixedDistanceOneGrants(int turn, int seat);
}

internal interface ICoreCardUseModulesCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireCurrentTurnOwnSkillSuppressions(int turnNumber, int turnOwnerSeat);
}

internal interface ICoreCardUseModulesCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNextActualUseCounterspellForbidden(int seat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreCardUseModulesTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreCardUseModulesCharacterSunQuanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyLearningPlaySelfOnly(int actor);
}

internal interface ICoreCardUseModulesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCardUseModulesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardUseModulesCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireOriginalTargetAdditionGrants(int turn, int seat);
}

internal interface ICoreCardUseModulesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardUseModulesCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExclusiveTurnPeachUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreCardUseModulesCoreProgramBooleanStateExpirationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireProgramBooleanStateLeases(int turnNumber, int turnSeat);
    void ExpireProgramBooleanStateLeasesAtOwnerTurnStart(int ownerSeat, bool ownerDied = false);
}

internal interface ICoreCardUseModulesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardUseModulesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreCardUseModulesCoreParticipantAndReserveProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireAttributedNatureMarkers(int ownerSeat);
}

internal interface ICoreCardUseModulesCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
    void ExpireDirectedTurnCardPolicies(int turnNumber, int turnSeat);
}

internal interface ICoreCardUseModulesCharacterZhangLingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCategoryTalismanUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreCardUseModulesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCardUseModulesModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreCardUseModulesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreCardUseModulesCoreNextSlashProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireNextSlashDamage(int turnNumber, int turnSeat);
}

internal interface ICoreCardUseModulesCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::System.Collections.Generic.IReadOnlyList<int> GetSelfProhibitionPolicyTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreCardUseModulesCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireCurrentTurnNonLockedSkillSuppressions(int turnNumber, int turnOwnerSeat);
}

internal interface ICoreCardUseModulesCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignTurnAlcoholUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreCardUseModulesCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCategoryRestricted(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardUseModulesCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouSmPlayedActor(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreCardUseModulesCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId);
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

internal interface ICoreCardUseModulesCoreBoundDiscardSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFirstRoundGameUsageRefunds(int turn, int turnSeat);
}

internal interface ICoreCardUseModulesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
}

internal interface ICoreCardUseModulesCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedPlayPhaseUseBan(int seat);
}

internal interface ICoreCardUseModulesCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseEntityRestricted(int seat, int id);
}

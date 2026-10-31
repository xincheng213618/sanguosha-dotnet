#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTargetSelectionRuntimePortsCoreTeamActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SamePublicTeam(global::CardGame.Core.CharacterState a, global::CardGame.Core.CharacterState b);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreOwnedExactCountAndNamedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome SelectProgramNamedBoundTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
    void ResolveProgramNamedBoundTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetProgramCardUseRoleTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateTurnEndingProgramContext(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTriggerFacts? candidateFacts = default);
    int GetProgramPlannedNormalDrawCount(global::CardGame.Core.ProgramSkillFrame frame, bool allowReplacedNoOp = false);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreShanheSourceCaoSongTrioOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? CaoSongSourceTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind kind, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target);
    bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreDeathProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetMarkerSourceCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.PlayerMarkerKind marker, int skillOwnerSeat);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCharacterZhugeJinMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] OrderMobileZhugeJinHongyuanTargets(global::CardGame.Core.ProgramSkillFrame frame, int[] targets);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreTriggeredCardProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramDiscardedDelayedTrickTarget(int ownerSeat, global::CardGame.Core.CharacterState target, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramPopulationAndArrowBarrageOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetCurrentArrowBarrageTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramEquipmentExchangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDiscardableHeBy(int actorSeat, int ownerSeat);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramAttackEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramSlashRedirectTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat, bool paymentCompleted = false);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetResolverRegistry ProgramTargetResolvers { get; }
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreShanheS5ZhulingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheS5JixianTarget(int owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactHellPreparationTargetContext(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context);
}

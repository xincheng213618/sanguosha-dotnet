#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetSelectionRuntimePortsRuntime
{
    bool global::CardGame.Core.IProgramTargetResolvers.TryResolve(ProgramTargetResolverContext context, out IReadOnlyList<int> targets) => _coreProgramTargetResolversOperations.ProgramTargetResolvers.TryResolve(context, out targets);
    int global::CardGame.Core.IProgramTargetResolvers.MinimumTargets(SkillProgramTargetKind kind) => _coreProgramTargetResolversOperations.ProgramTargetResolvers.MinimumTargets(kind);
    string global::CardGame.Core.IProgramTargetResolvers.EmptySelectionLabel(SkillProgramTargetKind kind) => _coreProgramTargetResolversOperations.ProgramTargetResolvers.EmptySelectionLabel(kind);
    IReadOnlyList<int> global::CardGame.Core.IProgramTargetFacts.VirtualDuelTargets(CharacterState owner) => _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(owner, CardKind.Duel, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).SelectMany(option => option.TargetSeats).Distinct().ToArray();
    bool global::CardGame.Core.IProgramTargetFacts.IsWithinAttackRange(int sourceSeat, int targetSeat) => _coreProgramDodgeResponsesOperations.IsWithinAttackRange(sourceSeat, targetSeat);
    ProgramSkillWindowContext global::CardGame.Core.IProgramTargetFacts.CreateCardActionProgramContext(ProgramCardTriggerWindowFrame window, ProgramCardTriggerCandidate candidate) => _coreCardActionsOperations.CreateCardActionProgramContext(window, candidate);
    ProgramSkillWindowContext global::CardGame.Core.IProgramTargetFacts.CreateTurnEndingProgramContext(TurnEndingBoundaryFrame window, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? facts) => _coreProgramLifecycleOperations.CreateTurnEndingProgramContext(window, candidate, facts);
    IReadOnlyList<int>? global::CardGame.Core.IProgramTargetFacts.CaoSongSourceTargetSeats(int ownerSeat, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _coreShanheSourceCaoSongTrioOperations.CaoSongSourceTargetSeats(ownerSeat, kind, context);
    bool global::CardGame.Core.IProgramTargetFacts.IsExactHellPreparationTargetContext(int ownerSeat, ProgramSkillWindowContext? context) => _coreHellActivitiesOperations.IsExactHellPreparationTargetContext(ownerSeat, context);
    DyingFrame? global::CardGame.Core.IProgramTargetFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    IReadOnlyList<int> global::CardGame.Core.IProgramTargetFacts.GetProgramCardUseRoleTargets(int ownerSeat, ProgramSkillWindowContext? context) => _coreProgramCardUseRolesOperations.GetProgramCardUseRoleTargets(ownerSeat, context);
    IReadOnlyList<int> global::CardGame.Core.IProgramTargetFacts.GetCurrentArrowBarrageTargets(int ownerSeat, ProgramSkillWindowContext? context) => _coreProgramPopulationAndArrowBarrageOperationsOperations.GetCurrentArrowBarrageTargets(ownerSeat, context);
    int global::CardGame.Core.IProgramTargetFacts.ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    int global::CardGame.Core.IProgramTargetFacts.GetMarkerSourceCount(CharacterState player, PlayerMarkerKind marker, int sourceSeat) => _coreDeathProgramsOperations.GetMarkerSourceCount(player, marker, sourceSeat);
    bool global::CardGame.Core.IProgramTargetFacts.PublicTeamEnemy(int ownerSeat, CharacterState target) => _coreTeamActivityProgramsOperations.PublicTeamEnemy(ownerSeat, target);
    bool global::CardGame.Core.IProgramTargetFacts.SamePublicTeam(CharacterState owner, CharacterState target) => _coreTeamActivitiesOperations.SamePublicTeam(owner, target);
    bool global::CardGame.Core.IProgramTargetFacts.IsRuleWounded(CharacterState target) => _coreTreatedWoundedOperations.IsRuleWounded(target);
    bool global::CardGame.Core.IProgramTargetFacts.OtherRevealedPublicTeamAlly(CharacterState owner, CharacterState target) => _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(owner, target);
    bool global::CardGame.Core.IProgramTargetFacts.IsShanheS5JixianTarget(int ownerSeat, CharacterState target) => _coreShanheS5ZhulingOperations.IsShanheS5JixianTarget(ownerSeat, target);
    bool global::CardGame.Core.IProgramTargetFacts.HasWeaponAbility(CharacterState target, CardKind kind) => _coreAdvancedWeaponsOperations.HasWeaponAbility(target, kind);
    bool global::CardGame.Core.IProgramTargetFacts.CanUseProvidedSlashTarget(CharacterState owner, CharacterState target) => _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(owner, target);
    bool global::CardGame.Core.IProgramTargetFacts.IsSlashRedirectTarget(int ownerSeat, int targetSeat) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.TargetSeat == ownerSeat && _coreProgramAttackEffectsOperations.IsProgramSlashRedirectTarget(attack, ownerSeat, targetSeat);
    bool global::CardGame.Core.IProgramTargetFacts.IsCardUseForbidden(int ownerSeat, CardKind kind, CardActionType type) => _coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, kind, type);
    bool global::CardGame.Core.IProgramTargetFacts.HasTurnCardTargetRestriction(int ownerSeat, SkillProgramCardTargetRestriction restriction) => _coreCardUseModulesOperations.HasTurnCardTargetRestriction(ownerSeat, restriction);
    bool global::CardGame.Core.IProgramTargetFacts.IsDirectedCardTargetProhibited(int ownerSeat, int targetSeat, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, targetSeat, kind);
    bool global::CardGame.Core.IProgramTargetFacts.IsSlashProhibited(CharacterState target) => _coreNativeDamagePoliciesOperations.IsSlashProhibited(target);
    bool global::CardGame.Core.IProgramTargetFacts.IsProgramDiscardedDelayedTrickTarget(int ownerSeat, CharacterState target, ProgramSkillWindowContext? context) => _coreTriggeredCardProgramsOperations.IsProgramDiscardedDelayedTrickTarget(ownerSeat, target, context);
    string? global::CardGame.Core.IProgramTargetFacts.GetEffectiveFactionId(CharacterState target) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(target);
    bool global::CardGame.Core.IProgramTargetFacts.HasDiscardableHeBy(int ownerSeat, int targetSeat) => _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(ownerSeat, targetSeat);
    bool global::CardGame.Core.IProgramTargetFacts.IsEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat) => _coreProgramEquipmentExchangesOperations.IsProgramEquipmentExchangePair(ownerSeat, firstSeat, secondSeat);
    int global::CardGame.Core.IProgramTargetFacts.GetCombatDistance(int ownerSeat, int targetSeat) => _coreNativeRuleQueriesOperations.GetCombatDistance(ownerSeat, targetSeat);
    int global::CardGame.Core.IProgramTargetFacts.PlannedNormalDrawCount(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramPlannedNormalDrawCount(frame);
    bool global::CardGame.Core.IProgramTargetFacts.IsPindianSourceSelectionInstruction(IReadOnlyList<SkillProgramEffect> instructions, int index) => global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime.IsWuLuKangPindianTargetInstruction(instructions, index);
    void global::CardGame.Core.IProgramTargetSelectionProtocols.Present(PendingDecision decision, int ownerSeat)
    {
        // Keep publication before the owner-control read, as in the original prompt boundary.
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = decision;
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    void global::CardGame.Core.IProgramTargetSelectionProtocols.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    SkillProgramStepOutcome global::CardGame.Core.IProgramTargetSelectionProtocols.SelectNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreOwnedExactCountAndNamedTargetsOperations.SelectProgramNamedBoundTargets(frame, effect);
    void global::CardGame.Core.IProgramTargetSelectionProtocols.ResolveNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _coreOwnedExactCountAndNamedTargetsOperations.ResolveProgramNamedBoundTargets(frame, effect, selected);
    int[] global::CardGame.Core.IProgramTargetSelectionProtocols.OrderSupportTargets(ProgramSkillFrame frame, int[] targets) => _characterZhugeJinMobileOperations.OrderMobileZhugeJinHongyuanTargets(frame, targets);
}

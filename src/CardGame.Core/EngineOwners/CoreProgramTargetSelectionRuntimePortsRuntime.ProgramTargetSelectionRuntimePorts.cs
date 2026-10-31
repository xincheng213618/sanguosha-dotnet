#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetSelectionRuntimePortsRuntime
{
    bool IProgramTargetResolvers.TryResolve(ProgramTargetResolverContext context, out IReadOnlyList<int> targets) => _coreProgramTargetResolversOperations.ProgramTargetResolvers.TryResolve(context, out targets);
    int IProgramTargetResolvers.MinimumTargets(SkillProgramTargetKind kind) => _coreProgramTargetResolversOperations.ProgramTargetResolvers.MinimumTargets(kind);
    string IProgramTargetResolvers.EmptySelectionLabel(SkillProgramTargetKind kind) => _coreProgramTargetResolversOperations.ProgramTargetResolvers.EmptySelectionLabel(kind);
    IReadOnlyList<int> IProgramTargetFacts.VirtualDuelTargets(CharacterState owner) => _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(owner, CardKind.Duel, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).SelectMany(option => option.TargetSeats).Distinct().ToArray();
    bool IProgramTargetFacts.IsWithinAttackRange(int sourceSeat, int targetSeat) => _coreProgramDodgeResponsesOperations.IsWithinAttackRange(sourceSeat, targetSeat);
    ProgramSkillWindowContext IProgramTargetFacts.CreateCardActionProgramContext(ProgramCardTriggerWindowFrame window, ProgramCardTriggerCandidate candidate) => _coreCardActionsOperations.CreateCardActionProgramContext2(window, candidate);
    ProgramSkillWindowContext IProgramTargetFacts.CreateTurnEndingProgramContext(TurnEndingBoundaryFrame window, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? facts) => _coreProgramLifecycleOperations.CreateTurnEndingProgramContext(window, candidate, facts);
    IReadOnlyList<int>? IProgramTargetFacts.CaoSongSourceTargetSeats(int ownerSeat, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _coreShanheSourceCaoSongTrioOperations.Value.CaoSongSourceTargetSeats(ownerSeat, kind, context);
    bool IProgramTargetFacts.IsExactHellPreparationTargetContext(int ownerSeat, ProgramSkillWindowContext? context) => _coreHellActivitiesOperations.IsExactHellPreparationTargetContext(ownerSeat, context);
    DyingFrame? IProgramTargetFacts.ActiveDying => _coreDyingCursorRuntimeOperations.Value.ActiveDying;

    IReadOnlyList<int> IProgramTargetFacts.GetProgramCardUseRoleTargets(int ownerSeat, ProgramSkillWindowContext? context) => _coreProgramCardUseRolesOperations.GetProgramCardUseRoleTargets2(ownerSeat, context);
    IReadOnlyList<int> IProgramTargetFacts.GetCurrentArrowBarrageTargets(int ownerSeat, ProgramSkillWindowContext? context) => _coreProgramPopulationAndArrowBarrageOperationsOperations.Value.GetCurrentArrowBarrageTargets(ownerSeat, context);
    int IProgramTargetFacts.ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    int IProgramTargetFacts.GetMarkerSourceCount(CharacterState player, PlayerMarkerKind marker, int sourceSeat) => _coreDeathProgramsOperations.GetMarkerSourceCount(player, marker, sourceSeat);
    bool IProgramTargetFacts.PublicTeamEnemy(int ownerSeat, CharacterState target) => _coreTeamActivityProgramsOperations.PublicTeamEnemy(ownerSeat, target);
    bool IProgramTargetFacts.SamePublicTeam(CharacterState owner, CharacterState target) => _coreTeamActivitiesOperations.SamePublicTeam(owner, target);
    bool IProgramTargetFacts.IsRuleWounded(CharacterState target) => _coreTreatedWoundedOperations.IsRuleWounded(target);
    bool IProgramTargetFacts.OtherRevealedPublicTeamAlly(CharacterState owner, CharacterState target) => _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(owner, target);
    bool IProgramTargetFacts.IsShanheS5JixianTarget(int ownerSeat, CharacterState target) => _coreShanheS5ZhulingOperations.Value.IsShanheS5JixianTarget(ownerSeat, target);
    bool IProgramTargetFacts.HasWeaponAbility(CharacterState target, CardKind kind) => _coreAdvancedWeaponsOperations.HasWeaponAbility(target, kind);
    bool IProgramTargetFacts.CanUseProvidedSlashTarget(CharacterState owner, CharacterState target) => _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(owner, target);
    bool IProgramTargetFacts.IsSlashRedirectTarget(int ownerSeat, int targetSeat) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.TargetSeat == ownerSeat && _coreProgramAttackEffectsOperations.Value.IsProgramSlashRedirectTarget(attack, ownerSeat, targetSeat);
    bool IProgramTargetFacts.IsCardUseForbidden(int ownerSeat, CardKind kind, CardActionType type) => _coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, kind, type);
    bool IProgramTargetFacts.HasTurnCardTargetRestriction(int ownerSeat, SkillProgramCardTargetRestriction restriction) => _coreCardUseModulesOperations.HasTurnCardTargetRestriction(ownerSeat, restriction);
    bool IProgramTargetFacts.IsDirectedCardTargetProhibited(int ownerSeat, int targetSeat, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, targetSeat, kind);
    bool IProgramTargetFacts.IsSlashProhibited(CharacterState target) => _coreNativeDamagePoliciesOperations.IsSlashProhibited(target);
    bool IProgramTargetFacts.IsProgramDiscardedDelayedTrickTarget(int ownerSeat, CharacterState target, ProgramSkillWindowContext? context) => _coreTriggeredCardProgramsOperations.Value.IsProgramDiscardedDelayedTrickTarget(ownerSeat, target, context);
    string? IProgramTargetFacts.GetEffectiveFactionId(CharacterState target) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(target);
    bool IProgramTargetFacts.HasDiscardableHeBy(int ownerSeat, int targetSeat) => _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(ownerSeat, targetSeat);
    bool IProgramTargetFacts.IsEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat) => _coreProgramEquipmentExchangesOperations.Value.IsProgramEquipmentExchangePair(ownerSeat, firstSeat, secondSeat);
    int IProgramTargetFacts.GetCombatDistance(int ownerSeat, int targetSeat) => _coreNativeRuleQueriesOperations.GetCombatDistance(ownerSeat, targetSeat);
    int IProgramTargetFacts.PlannedNormalDrawCount(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramPlannedNormalDrawCount(frame);
    bool IProgramTargetFacts.IsPindianSourceSelectionInstruction(IReadOnlyList<SkillProgramEffect> instructions, int index) => CharacterLuKangWumiaoRuntime.IsWuLuKangPindianTargetInstruction(instructions, index);
    void IProgramTargetSelectionProtocols.Present(PendingDecision decision, int ownerSeat)
    {
        // Keep publication before the owner-control read, as in the original prompt boundary.
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = decision;
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    void IProgramTargetSelectionProtocols.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    SkillProgramStepOutcome IProgramTargetSelectionProtocols.SelectNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreOwnedExactCountAndNamedTargetsOperations.SelectProgramNamedBoundTargets(frame, effect);
    void IProgramTargetSelectionProtocols.ResolveNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _coreOwnedExactCountAndNamedTargetsOperations.ResolveProgramNamedBoundTargets(frame, effect, selected);
    int[] IProgramTargetSelectionProtocols.OrderSupportTargets(ProgramSkillFrame frame, int[] targets) => _characterZhugeJinMobileOperations.Value.OrderMobileZhugeJinHongyuanTargets(frame, targets);
}

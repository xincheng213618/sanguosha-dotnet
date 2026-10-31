#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreTeamActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreTeamActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> _targetFactory = targetFactory;
    public bool SamePublicTeam(global::CardGame.Core.CharacterState a, global::CardGame.Core.CharacterState b) => this._targetFactory().SamePublicTeam(a, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreOwnedExactCountAndNamedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreOwnedExactCountAndNamedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome SelectProgramNamedBoundTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().SelectProgramNamedBoundTargets(frame, effect);
    public void ResolveProgramNamedBoundTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramNamedBoundTargets(frame, effect, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => this._targetFactory().CreateCardActionProgramContext(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramCardUseRolesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramCardUseRolesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetProgramCardUseRoleTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().GetProgramCardUseRoleTargets(ownerSeat, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateTurnEndingProgramContext(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTriggerFacts? candidateFacts = default) => this._targetFactory().CreateTurnEndingProgramContext(frame, candidate, candidateFacts);
    public int GetProgramPlannedNormalDrawCount(global::CardGame.Core.ProgramSkillFrame frame, bool allowReplacedNoOp = false) => this._targetFactory().GetProgramPlannedNormalDrawCount(frame, allowReplacedNoOp);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreShanheSourceCaoSongTrioAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreShanheSourceCaoSongTrioOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? CaoSongSourceTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind kind, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().CaoSongSourceTargetSeats(ownerSeat, kind, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreTeamActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreTeamActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target) => this._targetFactory().PublicTeamEnemy(owner, target);
    public bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target) => this._targetFactory().OtherRevealedPublicTeamAlly(owner, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreDeathProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreDeathProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> _targetFactory = targetFactory;
    public int GetMarkerSourceCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.PlayerMarkerKind marker, int skillOwnerSeat) => this._targetFactory().GetMarkerSourceCount(player, marker, skillOwnerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeJinMobileRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCharacterZhugeJinMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCharacterZhugeJinMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMobileRuntime> _targetFactory = targetFactory;
    public int[] OrderMobileZhugeJinHongyuanTargets(global::CardGame.Core.ProgramSkillFrame frame, int[] targets) => this._targetFactory().OrderMobileZhugeJinHongyuanTargets(frame, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreTriggeredCardProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreTriggeredCardProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> _targetFactory = targetFactory;
    public bool IsProgramDiscardedDelayedTrickTarget(int ownerSeat, global::CardGame.Core.CharacterState target, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().IsProgramDiscardedDelayedTrickTarget(ownerSeat, target, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramPopulationAndArrowBarrageOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramPopulationAndArrowBarrageOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetCurrentArrowBarrageTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().GetCurrentArrowBarrageTargets(ownerSeat, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramEquipmentExchangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramEquipmentExchangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> _targetFactory = targetFactory;
    public bool IsProgramEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat) => this._targetFactory().IsProgramEquipmentExchangePair(ownerSeat, firstSeat, secondSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasDiscardableHeBy(int actorSeat, int ownerSeat) => this._targetFactory().HasDiscardableHeBy(actorSeat, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
    public bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default) => this._targetFactory().HasTurnCardTargetRestriction(actorSeat, restriction, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramAttackEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramAttackEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> _targetFactory = targetFactory;
    public bool IsProgramSlashRedirectTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat, bool paymentCompleted = false) => this._targetFactory().IsProgramSlashRedirectTarget(attack, redirectorSeat, targetSeat, paymentCompleted);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BuildProgramOrdinaryTrickUseOptions(source, outputKind, physicalSuit, excludeOwner, enforceUsePermission, beneficiaryShieldSuit, actualEffectiveColor, hasActualColor, physicalCardIds, includeNextActualUseAdjustment, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().CanUseProvidedSlashTarget(owner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetResolverRegistry ProgramTargetResolvers { get => this._targetFactory().ProgramTargetResolvers; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreAdvancedWeaponsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreAdvancedWeaponsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _targetFactory = targetFactory;
    public bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasWeaponAbility(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreShanheS5ZhulingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreShanheS5ZhulingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> _targetFactory = targetFactory;
    public bool IsShanheS5JixianTarget(int owner, global::CardGame.Core.CharacterState target) => this._targetFactory().IsShanheS5JixianTarget(owner, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsSlashProhibited(global::CardGame.Core.CharacterState target) => this._targetFactory().IsSlashProhibited(target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreHellActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreHellActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsExactHellPreparationTargetContext(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().IsExactHellPreparationTargetContext(ownerSeat, context);
}

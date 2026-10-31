#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreSoleTargetProtectionCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreProgramCardUseRolesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreProgramCardUseRolesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _targetFactory = targetFactory;
    public void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => this._targetFactory().UpdateProgramRoleCardUse(use, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreSoleTargetProtectionCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId) => this._targetFactory().IsProgramAlternativeCost(action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCompletedCategoryUseProofAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCompletedCategoryUseProofOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> _targetFactory = targetFactory;
    public bool HasExactAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext completed) => this._targetFactory().HasExactAcceptedActualHandGainUse(use, completed);
    public bool ActualHandGainMutationBinding(long frameId, string skill, int owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramEffectOp operation) => this._targetFactory().ActualHandGainMutationBinding(frameId, skill, owner, window, operation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed class CoreSoleTargetProtectionCoreSoleTargetRedirectAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreSoleTargetRedirectOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _targetFactory = targetFactory;
    public void QueueSoleTargetRedirect(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetProtectionReceipt receipt) => this._targetFactory().QueueSoleTargetRedirect(use, receipt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed) => this._targetFactory().IsBlackTrickTargetProhibited(target, kind, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasJudgmentEffectiveCard(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => this._targetFactory().HasCardArmorBypass(actor, target, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => this._targetFactory().IsCurrentUsePhysicalCardClaim(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCurrentUseTargetCandidateRefreshAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCurrentUseTargetCandidateRefreshOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> _targetFactory = targetFactory;
    public void RefreshCurrentUseAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> addedMainTargets) => this._targetFactory().RefreshCurrentUseAddedTargetCandidates(use, addedMainTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreSoleTargetProtectionCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWeapon(player);
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public void MoveProgramLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default) => this._targetFactory().MoveProgramLogicalDiscard(frame, executorSeat, ids, reason, afterPhysicalRecords);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreSoleTargetProtectionCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsExchangedCardClaim(long actionId, int cardId) => this._targetFactory().IsExchangedCardClaim(actionId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsCurrentSlashFireChangedUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreSoleTargetProtectionCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreSoleTargetProtectionCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreSoleTargetProtectionCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public void RefreshSoleProtectionObservers(global::CardGame.Core.CardUseFrame use) => this._targetFactory().RefreshSoleProtectionObservers(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreSoleTargetProtectionCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreSoleTargetProtectionCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreSoleTargetProtectionCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreDesignatedExtraTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreDesignatedExtraTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _targetFactory = targetFactory;
    public bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().DesignatedExtraTargetCandidateMatches(candidate, context, window);
    public bool CanBeDistanceFreeDesignatedTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeDistanceFreeDesignatedTarget(use, target);
    public global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().CloneDesignatedExtraTargetAction(action, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreSoleTargetProtectionCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreSoleTargetProtectionCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreSoleTargetProtectionCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
    public void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
}

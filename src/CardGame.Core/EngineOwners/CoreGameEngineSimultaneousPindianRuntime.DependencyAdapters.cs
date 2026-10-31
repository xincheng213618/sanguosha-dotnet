#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreSimultaneousPindianCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreSimultaneousPindianCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner) => this._targetFactory().CanWuLuKangDiscardForPindian(owner);
    public bool TryBeginWuLuKangPostComparison(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.PindianResult result, int comparison) => this._targetFactory().TryBeginWuLuKangPostComparison(parent, result, comparison);
    public bool IsWuLuKangPrivateGroup(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsWuLuKangPrivateGroup(f);
    public bool ValidWuLuKangGroupReveal(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidWuLuKangGroupReveal(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreSimultaneousPindianCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreSimultaneousPindianCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreSimultaneousPindianCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime))]
internal sealed class CoreSimultaneousPindianCharacterJianYongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCharacterJianYongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TryBeginBoundaryJianYongSimultaneousZongshi(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().TryBeginBoundaryJianYongSimultaneousZongshi(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreSimultaneousPindianCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreSimultaneousPindianCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreSimultaneousPindianCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreSimultaneousPindianCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed class CoreSimultaneousPindianCorePindianRevealWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePindianRevealWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _targetFactory = targetFactory;
    public void BeginPindianRevealWindow(long parent, bool simultaneous, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> cards) => this._targetFactory().BeginPindianRevealWindow(parent, simultaneous, cards);
    public void AddRaoshePindianRankAndRestoreGushe(global::CardGame.Core.ProgramSkillFrame f, string skillId) => this._targetFactory().AddRaoshePindianRankAndRestoreGushe(f, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreSimultaneousPindianCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreSimultaneousPindianCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default) => this._targetFactory().BuildOwnedCardPaymentChoices(frameId, chooserSeat, cardOwnerSeat, zones, intent, cardCategories, cardKinds, suits, canSelect, destination, destinationSeat, prohibitReplacingEquipment, correspondingZoneSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime))]
internal sealed class CoreSimultaneousPindianCorePindianPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePindianPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> _targetFactory = targetFactory;
    public int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectivePindianRank(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime))]
internal sealed class CoreSimultaneousPindianCoreSimultaneousPindianClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreSimultaneousPindianClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> _targetFactory = targetFactory;
    public int[] AvailableSimultaneousClaimCards(global::CardGame.Core.PindianPolicyClaim claim) => this._targetFactory().AvailableSimultaneousClaimCards(claim);
    public void ResolveSimultaneousClaim(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveSimultaneousClaim(f, choice);
    public global::CardGame.Core.PindianPolicyClaims FreezeSimultaneousClaims(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.PindianResult result) => this._targetFactory().FreezeSimultaneousClaims(root, result);
    public bool OfferSimultaneousClaim(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().OfferSimultaneousClaim(f);
    public void AssertSimultaneousClaims(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertSimultaneousClaims(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed class CoreSimultaneousPindianCoreEquipmentSlotGroupProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreEquipmentSlotGroupProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _targetFactory = targetFactory;
    public bool CanBePindianTarget(int sourceSeat, int targetSeat) => this._targetFactory().CanBePindianTarget(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed class CoreSimultaneousPindianCoreSkillOwnershipWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreSkillOwnershipWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginAdvancedSkillsChanged(long parentFrameId) => this._targetFactory().TryBeginAdvancedSkillsChanged(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreSimultaneousPindianCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreSimultaneousPindianCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed class CoreSimultaneousPindianCoreAttributedMarkerEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreAttributedMarkerEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _targetFactory = targetFactory;
    public void MutateParticipantMarker(global::CardGame.Core.ProgramSkillFrame frame, int seat, global::CardGame.Core.PlayerMarkerKind marker, int delta, bool activeOnly = false) => this._targetFactory().MutateParticipantMarker(frame, seat, marker, delta, activeOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreSimultaneousPindianCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime))]
internal sealed class CoreSimultaneousPindianCorePindianRandomSelectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCorePindianRandomSelectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRandomCandidate> CollectPindianRandomPolicies(int source, int opponent) => this._targetFactory().CollectPindianRandomPolicies(source, opponent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreSimultaneousPindianCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime))]
internal sealed class CoreSimultaneousPindianCoreNativePlayerDeathAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreNativePlayerDeathOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> _targetFactory = targetFactory;
    public void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default) => this._targetFactory().BeginPlayerDeath(parentFrameId, victim, killer, attack, dying, causingProgramSkillFrameId, causingCardDamageReplacementId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CoreSimultaneousPindianCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginDouyuSimultaneousWinDraws(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PindianResult result) => this._targetFactory().TryBeginDouyuSimultaneousWinDraws(f, result);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> GetDouyuOriginalSimultaneousCards(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetDouyuOriginalSimultaneousCards(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime))]
internal sealed class CoreSimultaneousPindianCoreSimultaneousPindianSelectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreSimultaneousPindianSelectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> _targetFactory = targetFactory;
    public void ContinueSimultaneousRandomPolicies(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ContinueSimultaneousRandomPolicies(f);
    public bool CanOfferSimultaneousTop(global::CardGame.Core.ProgramSkillFrame f, int seat) => this._targetFactory().CanOfferSimultaneousTop(f, seat);
    public void ResolveSimultaneousRandomPolicy(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveSimultaneousRandomPolicy(f, choice);
    public void CommitSimultaneousSelection(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.PromptChoice choice) => this._targetFactory().CommitSimultaneousSelection(f, seat, choice);
    public void ContinueSimultaneousSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ContinueSimultaneousSelection(f);
    public global::CardGame.Core.CardLocation SimultaneousOriginalLocation(global::CardGame.Core.SimultaneousPindianReceipt r, global::CardGame.Core.PindianRevealedCard card) => this._targetFactory().SimultaneousOriginalLocation(r, card);
    public void AssertSimultaneousSelectionPolicies(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertSimultaneousSelectionPolicies(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreSimultaneousPindianCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public void MoveLogicalDiscard(int initiatorSeat, int executorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, long? parentFrameId = default) => this._targetFactory().MoveLogicalDiscard(initiatorSeat, executorSeat, card, from, reason, beforeFact, parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CoreSimultaneousPindianCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginMobileZhitiSimultaneousPrograms(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.PindianResult result) => this._targetFactory().TryBeginMobileZhitiSimultaneousPrograms(parent, result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreSimultaneousPindianCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

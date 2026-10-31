#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSimultaneousPindianCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreSimultaneousPindianCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner);
    bool TryBeginWuLuKangPostComparison(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.PindianResult result, int comparison);
    bool IsWuLuKangPrivateGroup(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidWuLuKangGroupReveal(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSimultaneousPindianCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreSimultaneousPindianCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreSimultaneousPindianCharacterJianYongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBoundaryJianYongSimultaneousZongshi(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreSimultaneousPindianCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreSimultaneousPindianCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreSimultaneousPindianKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreSimultaneousPindianCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSimultaneousPindianCorePindianRevealWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginPindianRevealWindow(long parent, bool simultaneous, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> cards);
    void AddRaoshePindianRankAndRestoreGushe(global::CardGame.Core.ProgramSkillFrame f, string skillId);
}

internal interface ICoreSimultaneousPindianCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreSimultaneousPindianCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICoreSimultaneousPindianContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSimultaneousPindianCorePindianPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreSimultaneousPindianResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSimultaneousPindianCoreSimultaneousPindianClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] AvailableSimultaneousClaimCards(global::CardGame.Core.PindianPolicyClaim claim);
    void ResolveSimultaneousClaim(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice);
    global::CardGame.Core.PindianPolicyClaims FreezeSimultaneousClaims(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.PindianResult result);
    bool OfferSimultaneousClaim(global::CardGame.Core.ProgramSkillFrame f);
    void AssertSimultaneousClaims(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBePindianTarget(int sourceSeat, int targetSeat);
}

internal interface ICoreSimultaneousPindianCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreSimultaneousPindianCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreSimultaneousPindianCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreSimultaneousPindianCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSimultaneousPindianCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MutateParticipantMarker(global::CardGame.Core.ProgramSkillFrame frame, int seat, global::CardGame.Core.PlayerMarkerKind marker, int delta, bool activeOnly = false);
}

internal interface ICoreSimultaneousPindianCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreSimultaneousPindianCorePindianRandomSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRandomCandidate> CollectPindianRandomPolicies(int source, int opponent);
}

internal interface ICoreSimultaneousPindianCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreSimultaneousPindianCoreNativePlayerDeathOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default);
}

internal interface ICoreSimultaneousPindianCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDouyuSimultaneousWinDraws(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PindianResult result);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> GetDouyuOriginalSimultaneousCards(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianCoreSimultaneousPindianSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSimultaneousRandomPolicies(global::CardGame.Core.ProgramSkillFrame f);
    bool CanOfferSimultaneousTop(global::CardGame.Core.ProgramSkillFrame f, int seat);
    void ResolveSimultaneousRandomPolicy(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice);
    void CommitSimultaneousSelection(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.PromptChoice choice);
    void ContinueSimultaneousSelection(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.CardLocation SimultaneousOriginalLocation(global::CardGame.Core.SimultaneousPindianReceipt r, global::CardGame.Core.PindianRevealedCard card);
    void AssertSimultaneousSelectionPolicies(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveLogicalDiscard(int initiatorSeat, int executorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, long? parentFrameId = default);
}

internal interface ICoreSimultaneousPindianCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMobileZhitiSimultaneousPrograms(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.PindianResult result);
}

internal interface ICoreSimultaneousPindianCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

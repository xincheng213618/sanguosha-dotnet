#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNearestLegalSlashRequestsCoreProgramTurnPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramVirtualCardUse(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.CardKind cardKind, bool ignoreDistance);
}

internal interface ICoreNearestLegalSlashRequestsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNearestLegalSlashRequestsCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundOnceCardlessProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default);
    bool RoundOnceCardlessProgramRequestRidesOn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent);
}

internal interface ICoreNearestLegalSlashRequestsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNearestLegalSlashRequestsCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZonghuProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
}

internal interface ICoreNearestLegalSlashRequestsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
}

internal interface ICoreNearestLegalSlashRequestsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNearestLegalSlashRequestsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreNearestLegalSlashRequestsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNearestLegalSlashRequestsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreNearestLegalSlashRequestsCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileSlashUseCards(global::CardGame.Core.CharacterState provider);
}

internal interface ICoreNearestLegalSlashRequestsCoreProgramSlashRequestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] GetNearestLivingCharacterSeats(int seat);
}

internal interface ICoreNearestLegalSlashRequestsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNearestLegalSlashRequestsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreNearestLegalSlashRequestsCoreCurrentTurnShownCardBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreNearestLegalSlashRequestsCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouLuXunWenheTarget(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int hpAmount);
    bool ValidNativeTrickTargetEffect(global::CardGame.Core.CardEffectBeforeApplyFrame window);
}

internal interface ICoreNearestLegalSlashRequestsCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default);
}

internal interface ICoreNearestLegalSlashRequestsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKenshangProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default);
    global::CardGame.Core.PendingDecision KenshangProgramNativeDecision(global::CardGame.Core.PendingDecision p);
}

internal interface ICoreNearestLegalSlashRequestsCoreWuJingQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnSlashChoiceUseForbidden(int actorSeat, global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreNearestLegalSlashRequestsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreNearestLegalSlashRequestsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreNearestLegalSlashRequestsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreNearestLegalSlashRequestsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNearestLegalSlashRequestsCoreAssistedSlashPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> Cards, global::CardGame.Core.CardKind Kind, global::CardGame.Core.CardConversionSource? Conversion) ReadAssistedSlashPayment(global::CardGame.Core.CharacterState actor, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNearestLegalSlashRequestsCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTousuiProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default);
    global::CardGame.Core.PendingDecision TousuiProgramNativeDecision(global::CardGame.Core.PendingDecision p);
}

internal interface ICoreNearestLegalSlashRequestsKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreNearestLegalSlashRequestsCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDrawFundedDistinctBasicProgramSelection(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
}

internal interface ICoreNearestLegalSlashRequestsCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.CardKind PhysicalSlashUseBaseKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IsOwnPublicPileSlashMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNearestLegalSlashRequestsCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnRedSlashPolicy(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreNearestLegalSlashRequestsCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGrantedPhaseEntityDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? ids);
}

internal interface ICoreNearestLegalSlashRequestsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICoreNearestLegalSlashRequestsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreNearestLegalSlashRequestsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNearestLegalSlashRequestsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNearestLegalSlashRequestsCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryLiuFengProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
}

internal interface ICoreNearestLegalSlashRequestsCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChainedStateBasicProgramSelection(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ChainedStateBasicFrame paid);
}

internal interface ICoreNearestLegalSlashRequestsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> GetSlashUseKinds(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind);
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNearestLegalSlashRequestsCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetOriginalTargetVirtualSlashReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget);
}

internal interface ICoreNearestLegalSlashRequestsCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds);
}

internal interface ICoreNearestLegalSlashRequestsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuanpeiProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
}

internal interface ICoreNearestLegalSlashRequestsCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDrawFundedDistinctBasicProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
}

internal interface ICoreNearestLegalSlashRequestsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNearestLegalSlashRequestsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false);
    bool IgnoresProgramSlashDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    bool IgnoresSpGuanYuWushengDistance(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IgnoresSlashUseDistance(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNearestLegalSlashRequestsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreNearestLegalSlashRequestsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreNearestLegalSlashRequestsCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNearestLegalSlashRequestsCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChainedStateBasicProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets);
}

internal interface ICoreNearestLegalSlashRequestsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreNearestLegalSlashRequestsCoreAssistedPhysicalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AssistedPhysicalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame);
    bool AssistedSlashPaymentHasRange(global::CardGame.Core.CharacterState actor, int targetSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreNearestLegalSlashRequestsCoreAssistedFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginAssistedProgramFactionSlashRequest(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int targetSeat, string resultBind);
}

internal interface ICoreNearestLegalSlashRequestsCoreWenheChaosOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactWenheChaos(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreNearestLegalSlashRequestsCoreParticipantAndReserveProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome ExecuteParticipantReserve(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCommandInputAdaptersCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AnswerCardDeclaration(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreCommandInputAdaptersCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySubmitRoundOnceCardlessPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result);
    bool TryResolveRoundOnceCardlessForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICoreCommandInputAdaptersCoreJudgmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitProgramJudgmentTriggerAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice);
}

internal interface ICoreCommandInputAdaptersCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZonghuAnswer(global::CardGame.Core.ChoiceId id);
    global::CardGame.Core.CommandResult SubmitZonghuAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id);
}

internal interface ICoreCommandInputAdaptersCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
}

internal interface ICoreCommandInputAdaptersKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreCommandInputAdaptersResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCommandInputAdaptersCoreFactionRequestPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFactionRequestCostChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
}

internal interface ICoreCommandInputAdaptersCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICoreCommandInputAdaptersCoreRoundOnceCardlessNativeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveRoundOnceCardlessResponse(global::CardGame.Core.PromptChoice choice, bool advance);
}

internal interface ICoreCommandInputAdaptersCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeginMouXqJueyanResponse(global::CardGame.Core.PromptChoice selected);
    bool BeginMouXqForcedSlash(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreCommandInputAdaptersCoreProgramRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitProgramRepeatJudgmentAnswer(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreCommandInputAdaptersCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitProgramJudgmentReplacementAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice);
}

internal interface ICoreCommandInputAdaptersCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRequestedDeckBasicAnswer(global::CardGame.Core.ChoiceId id);
    global::CardGame.Core.CommandResult SubmitRequestedDeckBasicAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice);
}

internal interface ICoreCommandInputAdaptersCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginProgramDodgeResponse(global::CardGame.Core.PromptChoice choice, bool advanceToHumanBoundary);
}

internal interface ICoreCommandInputAdaptersCoreBaguaResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SkipBaguaResponseCore();
}

internal interface ICoreCommandInputAdaptersCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySubmitDrawFundedDistinctBasicPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result);
}

internal interface ICoreCommandInputAdaptersCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWeijieAnswer(global::CardGame.Core.ChoiceId id);
    global::CardGame.Core.CommandResult SubmitWeijieAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id);
}

internal interface ICoreCommandInputAdaptersCoreFriendDuelResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFriendDuelDiscard(global::CardGame.Core.PromptChoice supplied);
}

internal interface ICoreCommandInputAdaptersCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceOneStepCore();
}

internal interface ICoreCommandInputAdaptersCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandError? ValidateNextActualUseZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreCommandInputAdaptersCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySubmitChainedStateBasicPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result);
    bool IsChainedStateBasicAnswer(global::CardGame.Core.ChoiceId id);
    global::CardGame.Core.CommandResult SubmitChainedStateBasicAnswer(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId);
}

internal interface ICoreCommandInputAdaptersCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLeiluanSource(global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreCommandInputAdaptersCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId);
}

internal interface ICoreCommandInputAdaptersCoreAssistedFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveAssistedFactionSlashChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
}

internal interface ICoreCommandInputAdaptersCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitYingboPromptAnswer(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreCommandInputAdaptersCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySubmitTieredRoundZeroPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result);
    bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICoreCommandInputAdaptersCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCommandInputAdaptersCoreTargetCardPreselectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long TargetHandMovementSequence(int seat);
}

internal interface ICoreCommandInputAdaptersCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryLiuFengQinlingAnswer(global::CardGame.Core.ChoiceId id);
    global::CardGame.Core.CommandResult SubmitBoundaryLiuFengQinlingAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id);
}

internal interface ICoreCommandInputAdaptersCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiSource(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreCommandInputAdaptersCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCommandInputAdaptersCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveTieredRoundZeroResponse(global::CardGame.Core.PromptChoice choice, bool advance);
}

internal interface ICoreCommandInputAdaptersCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitRecoveryReplacementAnswer(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreCommandInputAdaptersCoreExtraEffectForeignSelectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExtraForeignPublicAnswer(global::CardGame.Core.TargetCardSelectionFrame pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreCommandInputAdaptersCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreCommandInputAdaptersCoreHumanNativeCommandsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void HumanSelectGeneralCore(string generalId, bool advanceToHumanBoundary);
    void HumanPlayCore(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, bool advanceToHumanBoundary);
    void HumanUseEquipmentEffectCore(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind equipmentKind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, bool advanceToHumanBoundary);
    void HumanEndPlayCore(bool advanceToHumanBoundary);
    void HumanSelectGodFactionCore(string factionId, bool advanceToHumanBoundary);
    void HumanRequestFactionDefenseCore(bool advanceToHumanBoundary);
    void HumanRespondCore(bool useDodge, bool useBagua, int? requestedDodgeCardId, global::CardGame.Core.CardKind? requestedResponseCardKind, bool advanceToHumanBoundary);
    void HumanFactionDefenseResponseCore(bool useDodge, bool useBagua, int? requestedCardId, bool advanceToHumanBoundary);
    void HumanNullificationCore(bool useNullification, int? requestedNullificationCardId, bool advanceToHumanBoundary, global::CardGame.Core.CardConversionSource? selectedConversionSource = default);
    void HumanHarvestCardCore(int cardId, bool advanceToHumanBoundary);
    void HumanTargetCardSelectionCore(int slot, bool advanceToHumanBoundary);
    void HumanFireAttackCardCore(int cardId, bool advanceToHumanBoundary);
    void HumanFireAttackSkipCore(bool advanceToHumanBoundary);
    void HumanStoneAxeCore(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds, bool advanceToHumanBoundary);
    void HumanCixiongDoubleSwordsCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
    void HumanQinglongCrescentBladeCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
    void HumanZhuqueFanCore(bool convertToFireSlash, bool advanceToHumanBoundary);
    void HumanIceSwordCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
    void HumanQilinBowCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
    void HumanSkipDiscardPolicyCore(bool useSkill, bool advanceToHumanBoundary);
    void HumanBorrowedSwordFactionSlashCore(bool advanceToHumanBoundary);
    void HumanBorrowedSwordResponseCore(bool useSlash, int? requestedSlashCardId, global::CardGame.Core.CardKind? requestedEffectiveKind, bool advanceToHumanBoundary);
    void HumanZhangbaSlashResponseCore(global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds, bool advanceToHumanBoundary);
    void HumanProgramViewAsSlashResponseCore(global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds, global::CardGame.Core.CardConversionSource source, bool advanceToHumanBoundary);
    void HumanRequestFactionSlashCore(bool advanceToHumanBoundary);
    void HumanSlashResponseCore(bool useSlash, int? requestedSlashCardId, global::CardGame.Core.CardKind? requestedResponseCardKind, bool advanceToHumanBoundary);
    void HumanFactionSlashResponseCore(bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind, bool advanceToHumanBoundary);
    void HumanDyingResponseCore(bool usePeach, int? requestedPeachCardId, bool useAlcohol, int? requestedAlcoholCardId, bool advanceToHumanBoundary, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default);
    void HumanDyingProgramTriggerCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
}

internal interface ICoreCommandInputAdaptersCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuanpeiAnswer(global::CardGame.Core.ChoiceId id);
    global::CardGame.Core.CommandResult SubmitHuanpeiAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id);
}

internal interface ICoreCommandInputAdaptersCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveExtendedViewAsResponse(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreCommandInputAdaptersCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitProgramTriggerAnswer(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId);
}

internal interface ICoreCommandInputAdaptersCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginKenshangQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected);
    bool TryBeginKenshangBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreCommandInputAdaptersCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginTousuiQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected);
    bool TryBeginTousuiBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreCommandInputAdaptersCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDrawFundedDistinctBasicAnswer(global::CardGame.Core.ChoiceId choice);
    global::CardGame.Core.CommandResult SubmitDrawFundedDistinctBasicAnswer(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId);
}

internal interface ICoreCommandInputAdaptersKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreCommandInputAdaptersKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreCommandInputAdaptersCoreSkillPromptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitPindianPromptAnswer(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreCommandInputAdaptersCoreMatchSetupOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void StartCore();
}

internal interface ICoreCommandInputAdaptersCoreProgramParticipantPromptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat);
}

internal interface ICoreCommandInputAdaptersCoreProgramTopReorderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult SubmitProgramTopReorderAnswer(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreCommandInputAdaptersCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCommandInputAdaptersCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get; }

    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }

    global::CardGame.Core.NativeOwners.StoneAxeHandle? ActiveStoneAxe { get; set; }

    global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.IceSwordHandle? ActiveIceSword { get; set; }

    global::CardGame.Core.NativeOwners.QilinBowHandle? ActiveQilinBow { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreCommandInputAdaptersCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreCommandInputAdaptersCoreOpeningCustomizationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveOpeningHandChoice(global::CardGame.Core.PromptChoice selected);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeFactionDefenseCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? BeginJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, global::CardGame.Core.CardKind? sourceCard, global::CardGame.Core.JudgmentContinuationKind continuation, global::CardGame.Core.Card? delayedCard = default, int? sourceSeat = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility? programResultVisibility = default, global::CardGame.Core.OriginalUseLightningAttachment? originalUseLightning = default, int? originalUseLightningExcludedCardId = default);
}

internal interface ICoreNativeFactionDefenseCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeFactionDefenseCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    bool CanUseCardAsResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreNativeFactionDefenseResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeFactionDefenseCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtendedViewAsResponseChoices(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeFactionDefenseCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSkillInvocation(long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string? skillInstanceId = default, global::CardGame.Core.SkillInvocationReturnKind returnKind = (global::CardGame.Core.SkillInvocationReturnKind)0);
}

internal interface ICoreNativeFactionDefenseCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.CardConversionSource? PeekDeclarationConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool use, global::CardGame.Core.CardConversionSource? explicitSource = default);
    global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default);
}

internal interface ICoreNativeFactionDefenseCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreNativeFactionDefenseCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeFactionDefenseCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> ProgramDodgeResponseChoices(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeFactionDefenseKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, global::CardGame.Core.FactionResponsePolicySource source);
}

internal interface ICoreNativeFactionDefenseCoreProgramFactionPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind);
    string FactionPolicyName(global::CardGame.Core.FactionResponsePolicySource source);
    string FactionProviderDescription(global::CardGame.Core.FactionResponsePolicySource source);
}

internal interface ICoreNativeFactionDefenseCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreNativeFactionDefenseCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareNativePublicPileResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion);
}

internal interface ICoreNativeFactionDefenseCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeFactionDefenseCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeFactionDefenseCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool IsXieweiSource(global::CardGame.Core.CardConversionSource source);
    bool TryCompleteXieweiFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeFactionDefenseCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PaySingleCardResponse(global::CardGame.Core.Card card, global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardConversionSource? source);
    void FinishSingleCardResponse(global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreNativeFactionDefenseCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card);
}

internal interface ICoreNativeFactionDefenseCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeFactionDefenseCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? acceptedConversion);
}

internal interface ICoreNativeFactionDefenseCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false);
    global::CardGame.Core.CardConversionSource? GetSelectedResponseConversion(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreNativeFactionDefenseCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeFactionDefenseCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishOwnerDodgeDecision(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteFactionDefenseBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded);
    void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeFactionDefenseKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeFactionDefenseCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanRequestFactionDefense(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    global::System.Collections.Generic.IReadOnlyList<int> GetFactionProviderSeats(int ownerSeat, string factionId);
    bool HasBagua(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeFactionDefenseCoreNativeEquipmentInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBaguaEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, bool factionDefense = false);
}

internal interface ICoreNativeFactionDefenseCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, int? requesterSeat, int opponentSeat, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.Suit? completedResponseUseSuit = default);
}

internal interface ICoreNativeFactionDefenseCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeFactionDefenseCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

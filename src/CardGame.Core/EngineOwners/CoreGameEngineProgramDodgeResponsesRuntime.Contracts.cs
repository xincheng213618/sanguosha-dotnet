#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDodgeResponsesCoreNativeFactionDefenseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateFactionDefenseResponseChoices(global::CardGame.Core.CharacterState provider, global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, bool includeBagua);
    void CompleteFactionDefenseResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card? selectedDodge, bool usedBagua, global::CardGame.Core.CardConversionSource? virtualResponseSource = default);
}

internal interface ICoreProgramDodgeResponsesCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedPlayPhaseUseBan(int seat);
}

internal interface ICoreProgramDodgeResponsesCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreProgramDodgeResponsesCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreProgramDodgeResponsesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreProgramDodgeResponsesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramDodgeResponsesCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreProgramDodgeResponsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreProgramDodgeResponsesCharacterTaishiCiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenTaishiCiPoweiRange(int actor, int target);
}

internal interface ICoreProgramDodgeResponsesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, int? requesterSeat, int opponentSeat, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.Suit? completedResponseUseSuit = default);
}

internal interface ICoreProgramDodgeResponsesCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreProgramDodgeResponsesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeProgram(long frameId);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreProgramDodgeResponsesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreProgramDodgeResponsesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreProgramDodgeResponsesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramDodgeResponsesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramDodgeResponsesCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreProgramDodgeResponsesCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false);
}

internal interface ICoreProgramDodgeResponsesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramDodgeResponsesCoreProgramPersistentCombatPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGameFactionAttackRangeTarget(int actorSeat, int targetSeat);
}

internal interface ICoreProgramDodgeResponsesCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSayingRoundRange(int actor, int target);
}

internal interface ICoreProgramDodgeResponsesCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqJueyanResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dying = false);
}

internal interface ICoreProgramDodgeResponsesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default);
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreProgramDodgeResponsesKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreProgramDodgeResponsesCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreProgramDodgeResponsesCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicDodgeChoices(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreProgramDodgeResponsesCoreRoundOnceCardlessNativeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramDodgeResponsesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramDodgeResponsesCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dyingUse = false);
}

internal interface ICoreProgramDodgeResponsesCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgram> EnabledActivationPrograms(global::CardGame.Core.CharacterState player);
    global::CardGame.Core.SkillProgram GetEnabledSkillProgram(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreProgramDodgeResponsesCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBagua(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramDodgeResponsesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreProgramDodgeResponsesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

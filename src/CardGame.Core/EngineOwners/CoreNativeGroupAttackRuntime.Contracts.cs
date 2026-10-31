#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeGroupAttackCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
}

internal interface ICoreNativeGroupAttackCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreNativeGroupAttackCoreRequiredGroupDodgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FreezeRequiredGroupDodgeResponses(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeGroupAttackKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeGroupAttackCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreNativeGroupAttackCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default);
}

internal interface ICoreNativeGroupAttackCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginGroupTargetNullification(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeGroupAttackCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZeroEntityUse(long id);
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreNativeGroupAttackCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> ProgramDodgeResponseChoices(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeGroupAttackCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalTengjia { get; }
}

internal interface ICoreNativeGroupAttackCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreNativeGroupAttackCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeGroupAttackCoreNativeGroupResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse);
}

internal interface ICoreNativeGroupAttackCoreNativeGroupCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishGroupAttack(global::CardGame.Core.NativeOwners.GroupCardHandle group, bool allowYingboGift = true);
}

internal interface ICoreNativeGroupAttackCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTengjia(global::CardGame.Core.CharacterState player);
    bool HasBagua(global::CardGame.Core.CharacterState player);
    bool CanRequestFactionDefense(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool CanRequestFactionSlashResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeGroupAttackCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouLuXunGroupEffect(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeGroupAttackCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeGroupAttackCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedCardUnrespondable(long frameId);
}

internal interface ICoreNativeGroupAttackCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreNativeGroupAttackCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreNativeGroupAttackCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeGroupAttackCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeGroupAttackKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreNativeGroupAttackCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeGroupAttackCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeGroupAttackCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYingboUnrespondable(long resolutionId);
}

internal interface ICoreNativeGroupAttackCharacterLiuQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenjiResponseForbidden(long useId, int responderSeat);
}

internal interface ICoreNativeGroupAttackCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreNativeGroupAttackCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHellRemenImmunity(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeGroupAttackCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreNativeGroupAttackCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeGroupAttackCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int responderSeat, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreNativeGroupAttackCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeGroupCompletionCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreNativeGroupCompletionCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeGroupCompletionCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreNativeGroupCompletionCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreNativeGroupCompletionCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeGroupCompletionCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishAnniversaryXunXuUseMaterials(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeGroupCompletionCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreNativeGroupCompletionCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false);
}

internal interface ICoreNativeGroupCompletionCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeGroupCompletionCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipDiouFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreNativeGroupCompletionCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeGroupCompletionCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreNativeGroupCompletionCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipMouXqJueyanFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreNativeGroupCompletionCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICoreNativeGroupCompletionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeGroupCompletionCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeGroupCompletionCharacterTianChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTianChouShandaoUse(long id);
}

internal interface ICoreNativeGroupCompletionCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreNativeGroupCompletionCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeGroupCompletionCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card);
}

internal interface ICoreNativeGroupCompletionCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginYingboGift(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind cardKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.YingboGiftContinuation continuation, global::CardGame.Core.NativeOwners.CardAttackHandle? attack = default, global::CardGame.Core.NativeOwners.GroupCardHandle? group = default);
}

internal interface ICoreNativeGroupCompletionKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface ICoreNativeGroupCompletionCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeGroupCompletionCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreExtraEffectForeignSelectionCoreNativeTrickEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SkipUnavailableTargetCardEffect(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardEffectSkipReason reason);
    void ApplyTargetCardEffect(long resolutionId, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveCardKind, int sourceSeat, int targetSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card targetCard, global::CardGame.Core.CardLocation targetCardFromZone);
}

internal interface ICoreExtraEffectForeignSelectionCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreExtraEffectForeignSelectionCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreExtraEffectForeignSelectionCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreExtraEffectForeignSelectionCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreExtraEffectForeignSelectionCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreExtraEffectForeignSelectionCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreExtraEffectForeignSelectionCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreExtraEffectForeignSelectionCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreExtraEffectForeignSelectionCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreExtraEffectForeignSelectionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreExtraEffectForeignSelectionCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLeChengyanEffectUse(long id);
}

internal interface ICoreExtraEffectForeignSelectionCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreExtraEffectForeignSelectionCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICoreExtraEffectForeignSelectionCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangHasVisibleTargetHand(int source, global::CardGame.Core.CardKind kind);
}

internal interface ICoreExtraEffectForeignSelectionCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExtraCardEffectRound(long id);
}

internal interface ICoreExtraEffectForeignSelectionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreExtraEffectForeignSelectionCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMantianGuohaiUse(long id);
}

internal interface ICoreExtraEffectForeignSelectionCoreWushuangTargetCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangExtraForeignSelection(long useId);
    void FinishWushuangTargetCards(global::CardGame.Core.CardUseFrame use);
}

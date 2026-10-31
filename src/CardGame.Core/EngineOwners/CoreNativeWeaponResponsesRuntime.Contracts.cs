#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeWeaponResponsesCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalCixiongDoubleSwords { get; }
}

internal interface ICoreNativeWeaponResponsesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeWeaponResponsesCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeWeaponResponsesCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveLogicalDiscard(int initiatorSeat, int executorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, long? parentFrameId = default);
}

internal interface ICoreNativeWeaponResponsesKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeWeaponResponsesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreNativeWeaponResponsesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get; set; }
}

internal interface ICoreNativeWeaponResponsesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeWeaponResponsesCoreNativeEquipmentInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCixiongEquipmentInvocation(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending);
    void PublishCixiongAcceptedTargetChoice(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending);
}

internal interface ICoreNativeWeaponResponsesCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreNativeWeaponResponsesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeWeaponResponsesCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSlashDefense(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeWeaponResponsesCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeWeaponResponsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeWeaponResponsesCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteKemingCixiong(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending, bool activated, bool targetDiscarded, int? discardedCardId, int sourceDrawCount);
}

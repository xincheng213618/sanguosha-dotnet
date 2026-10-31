#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGuanduGeneralChoicesCoreGuanduGeneralsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, bool completed = true);
    global::CardGame.Core.Card[] GuanduDiscardable(int chooser, int owner);
    global::CardGame.Core.Card[] GuanduOwnedCards(int seat, bool judgment = false);
    void AssertGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.ProgramSkillFrame GuanduStage(global::CardGame.Core.ProgramSkillFrame f, string stage);
    bool RunGuanduGeneral(long id);
    global::CardGame.Core.ProgramSkillFrame GuanduUpdate(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GuanduGeneralReceipt r);
}

internal interface ICoreGuanduGeneralChoicesCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanduAllies(int first, int second);
}

internal interface ICoreGuanduGeneralChoicesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreGuanduGeneralChoicesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreGuanduGeneralChoicesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGuanduGeneralChoicesCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreGuanduGeneralChoicesCoreGuanduGeneralBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GuanduDiscardFactEvent[] GuanduClaimableDiscards(int target);
}

internal interface ICoreGuanduGeneralChoicesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreGuanduGeneralChoicesCoreGuanduGeneralQualificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueGuanduAllCardBan(global::CardGame.Core.ProgramSkillFrame f, int target);
}

internal interface ICoreGuanduGeneralChoicesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreGuanduGeneralChoicesRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICoreGuanduGeneralChoicesCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default);
    void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default);
}

internal interface ICoreGuanduGeneralChoicesCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreGuanduGeneralChoicesCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreGuanduGeneralChoicesCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default);
}

internal interface ICoreGuanduGeneralChoicesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreGuanduGeneralChoicesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

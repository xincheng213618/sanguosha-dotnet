#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramBoundCardRuntimePortsCoreSequentialDiscardReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame FreezeSequentialDiscardTopPayment(global::CardGame.Core.ProgramSkillFrame f, string bind, string? except, global::CardGame.Core.SkillProgramCardDestination destination);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreProgramBoundCardRuntimePortsCharacterWuGuoTaiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? BoundaryWuGuoTaiBuyiDiscardActor(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreHellMengpoProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHellShiyouCapability { get; }
}

internal interface ICoreProgramBoundCardRuntimePortsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramBoundCardRuntimePortsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramBoundCardRuntimePortsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreProgramLostHpChainedPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame FreezeLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, string? exceptBind, global::CardGame.Core.SkillProgramCardDestination destination);
    void CommitLostHpChainedPayment(long frameId);
}

internal interface ICoreProgramBoundCardRuntimePortsCorePindianHandReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? BeginPindianWholeHandAcquisition(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.SkillProgramCardDestination destination);
    void CommitPindianWholeHandAcquisition(long frameId, string bind, long before);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreCardDistributionPresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PresentBoundCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string sourceBind, global::CardGame.Core.SkillProgramTargetKind targetKind);
}

internal interface ICoreProgramBoundCardRuntimePortsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramBoundCardRuntimePortsCoreParticipantHandProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginBoundParticipantGift(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation target, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreLiejieSourceDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAtomicLiejieCostMove(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind);
    void FreezeLiejieDiscardColors(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, global::System.Collections.Generic.IReadOnlyList<int> selectedIds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> selectedLocations);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreDeckProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CleanupDeckSlashSequence(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramBoundCardRuntimePortsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreForcedPhaseAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PreparePhaseAlcoholPayment(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.CardMoveReason reason);
    void CommitPhaseAlcoholPayment(long id);
}

internal interface ICoreProgramBoundCardRuntimePortsCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreExactRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CleanupExactRepeatedJudgment(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreProgramBoundCardRuntimePortsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
    bool HasForeignDiscardCapability { get; }
}

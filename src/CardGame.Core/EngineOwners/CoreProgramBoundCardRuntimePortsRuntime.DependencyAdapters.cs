#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreSequentialDiscardReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreSequentialDiscardReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame FreezeSequentialDiscardTopPayment(global::CardGame.Core.ProgramSkillFrame f, string bind, string? except, global::CardGame.Core.SkillProgramCardDestination destination) => this._targetFactory().FreezeSequentialDiscardTopPayment(f, bind, except, destination);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default) => this._targetFactory().GetProgramTargetSeats(ownerSeat, targetKind, windowContext, marker, actorReference);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCharacterWuGuoTaiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCharacterWuGuoTaiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int? BoundaryWuGuoTaiBuyiDiscardActor(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations) => this._targetFactory().BoundaryWuGuoTaiBuyiDiscardActor(frame, bind, ids, locations);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreHellMengpoProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreHellMengpoProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _targetFactory = targetFactory;
    public bool HasHellShiyouCapability { get => this._targetFactory().HasHellShiyouCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreProgramLostHpChainedPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreProgramLostHpChainedPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame FreezeLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, string? exceptBind, global::CardGame.Core.SkillProgramCardDestination destination) => this._targetFactory().FreezeLostHpChainedPayment(frame, sourceBind, exceptBind, destination);
    public void CommitLostHpChainedPayment(long frameId) => this._targetFactory().CommitLostHpChainedPayment(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCorePindianHandReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCorePindianHandReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> _targetFactory = targetFactory;
    public long? BeginPindianWholeHandAcquisition(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.SkillProgramCardDestination destination) => this._targetFactory().BeginPindianWholeHandAcquisition(f, bind, destination);
    public void CommitPindianWholeHandAcquisition(long frameId, string bind, long before) => this._targetFactory().CommitPindianWholeHandAcquisition(frameId, bind, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card) => this._targetFactory().FieldEquipmentSlot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreCardDistributionPresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreCardDistributionPresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PresentBoundCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string sourceBind, global::CardGame.Core.SkillProgramTargetKind targetKind) => this._targetFactory().PresentBoundCardDistribution(frame, choices, sourceBind, targetKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreParticipantHandProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreParticipantHandProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginBoundParticipantGift(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation target, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().BeginBoundParticipantGift(frame, bind, ids, target, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreLiejieSourceDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreLiejieSourceDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> _targetFactory = targetFactory;
    public bool IsAtomicLiejieCostMove(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind) => this._targetFactory().IsAtomicLiejieCostMove(frame, sourceBind);
    public void FreezeLiejieDiscardColors(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, global::System.Collections.Generic.IReadOnlyList<int> selectedIds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> selectedLocations) => this._targetFactory().FreezeLiejieDiscardColors(frame, sourceBind, selectedIds, selectedLocations);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreDeckProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreDeckProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> _targetFactory = targetFactory;
    public void CleanupDeckSlashSequence(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CleanupDeckSlashSequence(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawOneToProcessing(reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreForcedPhaseAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreForcedPhaseAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> _targetFactory = targetFactory;
    public bool PreparePhaseAlcoholPayment(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PreparePhaseAlcoholPayment(f, bind, reason);
    public void CommitPhaseAlcoholPayment(long id) => this._targetFactory().CommitPhaseAlcoholPayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCharacterHuangQuanMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCharacterHuangQuanMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> _targetFactory = targetFactory;
    public global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default) => this._targetFactory().NativeGiveCompletion(producer, giver, recipient, ids, before, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreExactRepeatedJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreExactRepeatedJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> _targetFactory = targetFactory;
    public void CleanupExactRepeatedJudgment(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CleanupExactRepeatedJudgment(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreProgramBoundCardRuntimePortsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
    public bool HasForeignDiscardCapability { get => this._targetFactory().HasForeignDiscardCapability; }
}

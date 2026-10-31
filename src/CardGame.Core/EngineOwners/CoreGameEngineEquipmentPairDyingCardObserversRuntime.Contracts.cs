#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEquipmentPairDyingCardObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreEquipmentPairDyingCardObserversCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreEquipmentPairDyingCardObserversCoreEquipmentPairPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EquipmentPairCostIsPaid(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.CardLocation OwnedPaymentDiscardDestination(int cardId);
}

internal interface ICoreEquipmentPairDyingCardObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreEquipmentPairDyingCardObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreEquipmentPairDyingCardObserversCoreDyingOwnedCardSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDyingOwnedCardReceipt(global::CardGame.Core.ProgramSkillFrame root);
    bool ExactDyingOwnedCardEntry(global::CardGame.Core.ProgramSkillFrame root, out global::CardGame.Core.DyingFrame dying, out global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry);
}

internal interface ICoreEquipmentPairDyingCardObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreEquipmentPairDyingCardObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreEquipmentPairDyingCardObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellDyingProgramRidesOn(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying);
    bool PolicyCounterspellDyingFaceEdge(int index);
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreEquipmentPairDyingCardObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

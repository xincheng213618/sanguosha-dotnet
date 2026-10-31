#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSequentialDiscardObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSequentialDiscardObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreSequentialDiscardObserversCoreSequentialDiscardProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string SequentialDiscardReason(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSequentialDiscardObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSequentialDiscardObserversCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSequentialDiscardObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreSequentialDiscardObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreSequentialDiscardObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellDyingProgramRidesOn(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying);
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool PolicyCounterspellDyingFaceEdge(int index);
}

internal interface ICoreSequentialDiscardObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSequentialDiscardObserversCoreSequentialDiscardReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidSequentialDiscard(global::CardGame.Core.ProgramSkillFrame f);
    bool HasSequentialDiscardTopPayment(global::CardGame.Core.ProgramSkillFrame f, string bind);
}

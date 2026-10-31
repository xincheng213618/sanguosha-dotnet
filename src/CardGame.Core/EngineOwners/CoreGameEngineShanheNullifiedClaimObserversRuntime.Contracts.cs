#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheNullifiedClaimObserversCoreShanheDelayedNullifiedTrickClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateShanheDelayedNullifiedTrickClaim(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.ShanheDelayedNullifiedTrickClaim receipt);
}

internal interface ICoreShanheNullifiedClaimObserversCoreShanheNullifiedTrickClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheNullifiedClaimAwaitingMovement(global::CardGame.Core.NullificationWindowFrame window);
    bool IsShanheNullifiedClaimMovement(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardMovementBatchContext batch);
    void AssertShanheNullifiedTrickClaims();
}

internal interface ICoreShanheNullifiedClaimObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShanheNullifiedClaimObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreShanheNullifiedClaimObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool PolicyCounterspellDyingProgramRidesOn(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying);
    bool PolicyCounterspellDyingFaceEdge(int index);
}

internal interface ICoreShanheNullifiedClaimObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreShanheNullifiedClaimObserversCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreShanheNullifiedClaimObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreShanheNullifiedClaimObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheNullifiedClaimObserversCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreShanheNullifiedClaimObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreShanheNullifiedClaimObserversCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreShanheNullifiedClaimObserversCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheNullifiedClaimObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreShanheNullifiedClaimObserversCorePublicPileEquipmentSequencesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreShanheNullifiedClaimObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool PaidObserverDamageDyingProgramMatches(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying);
    bool PaidObserverDamageDyingFaceEdge(int index);
}

internal interface ICoreShanheNullifiedClaimObserversCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreShanheNullifiedClaimObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

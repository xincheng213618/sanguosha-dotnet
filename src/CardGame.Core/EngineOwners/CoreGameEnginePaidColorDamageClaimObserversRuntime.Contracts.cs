#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePaidColorDamageClaimObserversCorePaidColorConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesPaidColorDrawOwner(global::CardGame.Core.ProgramSkillFrame root);
    bool IsActualPaidColorMovement(int owner, global::CardGame.Core.ProgramPaidColorConversionOrigin p);
    void AssertPaidColorTurnConversions();
}

internal interface ICorePaidColorDamageClaimObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICorePaidColorDamageClaimObserversCoreActualTurnDamageClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesActualTurnDamageEnding(global::CardGame.Core.ProgramSkillFrame root);
}

internal interface ICorePaidColorDamageClaimObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePaidColorDamageClaimObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidColorDamageClaimObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICorePaidColorDamageClaimObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICorePaidColorDamageClaimObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
}

internal interface ICorePaidColorDamageClaimObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICorePaidColorDamageClaimObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidColorDamageClaimObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidColorDamageClaimObserversCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICorePaidColorDamageClaimObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool PaidObserverDamageDyingFaceEdge(int index);
    bool PaidObserverAttackHpLossDyingMatches(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.DyingFrame dying);
    bool PaidObserverDamageDyingProgramMatches(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidColorDamageClaimObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

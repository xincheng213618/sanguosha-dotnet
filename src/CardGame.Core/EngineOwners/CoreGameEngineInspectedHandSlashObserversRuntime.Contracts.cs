#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreInspectedHandSlashObserversCoreInspectedHandSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidInspectedHandSlash(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreInspectedHandSlashObserversCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreInspectedHandSlashObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreInspectedHandSlashObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreInspectedHandSlashObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreInspectedHandSlashObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreInspectedHandSlashObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreInspectedHandSlashObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreInspectedHandSlashObserversCorePaidColorDamageClaimObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidColorDamageClaimObserverEdge(int index);
}

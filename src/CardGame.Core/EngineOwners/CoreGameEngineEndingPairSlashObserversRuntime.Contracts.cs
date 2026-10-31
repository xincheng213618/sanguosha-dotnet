#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEndingPairSlashObserversCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreEndingPairSlashObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreEndingPairSlashObserversCoreEndingPairDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidEndingPairDraw(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreEndingPairSlashObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreEndingPairSlashObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreEndingPairSlashObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreEndingPairSlashObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageDyingFaceEdge(int index);
    bool PaidObserverAttackHpLossDyingMatches(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.DyingFrame dying);
    bool PaidObserverDamageDyingProgramMatches(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying);
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreEndingPairSlashObserversCorePlaySlashRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPlaySlashRecast(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreEndingPairSlashObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreEndingPairSlashObserversCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreEndingPairSlashObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreEndingPairSlashObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

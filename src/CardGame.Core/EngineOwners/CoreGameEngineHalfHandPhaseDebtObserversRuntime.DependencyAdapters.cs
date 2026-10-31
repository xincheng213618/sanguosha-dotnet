#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreHalfHandTargetSupportAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreHalfHandTargetSupportOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> _targetFactory = targetFactory;
    public bool HalfHandSupportPaymentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requirePaid) => this._targetFactory().HalfHandSupportPaymentMatches(f, parent, requirePaid);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCorePaidOwnTargetObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCorePaidOwnTargetObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? ActualUseTargetObserverRoot(long useId) => this._targetFactory().ActualUseTargetObserverRoot(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> _targetFactory = targetFactory;
    public long HalfHandDebtSequence { get => this._targetFactory().HalfHandDebtSequence; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreRecoveryReplacementDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreRecoveryReplacementDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _targetFactory = targetFactory;
    public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RecoveryReplacementFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreParticipantHandProgramRescueRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreParticipantHandProgramRescueRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentProgramAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
    public bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> _targetFactory = targetFactory;
    public bool ValidHalfHandDraw(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidHalfHandDraw(f);
    public bool ValidHalfHandGift(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidHalfHandGift(f);
    public bool ValidPhaseHandExchange(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidPhaseHandExchange(f);
    public bool ValidPhaseHandDebtPayment(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidPhaseHandDebtPayment(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CoreHalfHandPhaseDebtObserversCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtObserversCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
}

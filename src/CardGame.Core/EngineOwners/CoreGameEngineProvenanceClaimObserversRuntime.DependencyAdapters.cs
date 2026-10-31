#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreProvenanceClaimObserversCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProvenanceClaimObserversCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed class CoreProvenanceClaimObserversCoreParticipantHandProgramRescueRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProvenanceClaimObserversCoreParticipantHandProgramRescueRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentProgramAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed class CoreProvenanceClaimObserversCoreDiscardedProvenanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProvenanceClaimObserversCoreDiscardedProvenanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _targetFactory = targetFactory;
    public void AssertProvenanceClaim(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertProvenanceClaim(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreProvenanceClaimObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProvenanceClaimObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CoreProvenanceClaimObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProvenanceClaimObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

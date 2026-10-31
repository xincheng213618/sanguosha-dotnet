#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreForeignContestAidObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreForeignContestAidObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> _targetFactory = targetFactory;
    public bool HasSameTypeAidTargetObserver(long useId) => this._targetFactory().HasSameTypeAidTargetObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreRecipientContestObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreRecipientContestObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime> _targetFactory = targetFactory;
    public bool HasUniqueHpPeerActualTargetObserver(long useId) => this._targetFactory().HasUniqueHpPeerActualTargetObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreParticipantHandProgramRescueRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreParticipantHandProgramRescueRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentProgramAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreEarnedActualEndingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreEarnedActualEndingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> _targetFactory = targetFactory;
    public void IssueEarnedActualEnding(global::CardGame.Core.ProgramSkillFrame frame, string skillId, string bindingId) => this._targetFactory().IssueEarnedActualEnding(frame, skillId, bindingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreOwnTrickDrawAlcoholObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreOwnTrickDrawAlcoholObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> _targetFactory = targetFactory;
    public bool OwnTrickDrawLegacyAlcoholRide(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent, int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().OwnTrickDrawLegacyAlcoholRide(root, parent, dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreLostHpOwnedGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreLostHpOwnedGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome RunLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().RunLostHpOwnedGift(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreOwnTrickDrawTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreOwnTrickDrawTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> _targetFactory = targetFactory;
    public bool OwnTrickDrawMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent) => this._targetFactory().OwnTrickDrawMatches(root, parent);
    public bool OwnTrickDrawFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OwnTrickDrawFirstChild(f, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreActualUseTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreActualUseTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> _targetFactory = targetFactory;
    public bool MatchesActualUseTarget(global::CardGame.Core.ActualUseTargetIdentity identity) => this._targetFactory().MatchesActualUseTarget(identity);
    public bool CanRunActualUseTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunActualUseTarget(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
    public bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreProgramCardTargetEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreProgramCardTargetEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _targetFactory = targetFactory;
    public void MarkCardEffectIneffective(long resolutionId, int targetSeat) => this._targetFactory().MarkCardEffectIneffective(resolutionId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreHalfHandPhaseDebtObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreHalfHandPhaseDebtObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandSupportObserverRootMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent) => this._targetFactory().HalfHandSupportObserverRootMatches(f, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CorePaidOwnTargetObserversCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePaidOwnTargetObserversCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
}

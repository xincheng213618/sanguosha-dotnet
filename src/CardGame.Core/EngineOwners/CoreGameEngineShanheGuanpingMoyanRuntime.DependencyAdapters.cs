#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreObserverSubtreesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default) => this._targetFactory().NormalizeEphemeralCardDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
    public bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed class CoreShanheGuanpingMoyanCoreParticipantHandProgramRescueRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCoreParticipantHandProgramRescueRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentProgramAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreShanheGuanpingMoyanCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheGuanpingMoyanCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseSchedulingRuntime))]
internal sealed class CoreProgramLifecycleProgramPhaseSchedulingAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramPhaseSchedulingOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseSchedulingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreProgramLifecycleCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramBindingRuntime))]
internal sealed class CoreProgramLifecycleProgramBindingAdapter(global::System.Func<global::CardGame.Core.ProgramBindingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramBindingOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramBindingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramBindingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTargetSelectionRuntime))]
internal sealed class CoreProgramLifecycleProgramTargetSelectionAdapter(global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTargetSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetSelectionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramOwnedZoneDiscardRuntime))]
internal sealed class CoreProgramLifecycleProgramOwnedZoneDiscardAdapter(global::System.Func<global::CardGame.Core.ProgramOwnedZoneDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramOwnedZoneDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramOwnedZoneDiscardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramOwnedZoneDiscardRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed class CoreProgramLifecycleCorePublicDamagePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCorePublicDamagePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateAiSkillContext(global::CardGame.Core.CharacterState player, int viewerSeat, global::CardGame.Core.GameSnapshot? snapshot = default) => this._targetFactory().CreateAiSkillContext(player, viewerSeat, snapshot);
    public global::System.Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat) => this._targetFactory().CreatePublicSourceOccurrenceDamageBonus(viewerSeat);
    public global::CardGame.Core.PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, global::CardGame.Core.LegalAction action, int targetSeat) => this._targetFactory().PredictPublicActionDamage(viewerSeat, action, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerRuntime))]
internal sealed class CoreProgramLifecycleProgramTriggerAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTriggerOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramBoundaryInvariantRuntime))]
internal sealed class CoreProgramLifecycleProgramBoundaryInvariantAdapter(global::System.Func<global::CardGame.Core.ProgramBoundaryInvariantRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramBoundaryInvariantOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramBoundaryInvariantRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramBoundaryInvariantRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramRandomCardAcquisitionRuntime))]
internal sealed class CoreProgramLifecycleProgramRandomCardAcquisitionAdapter(global::System.Func<global::CardGame.Core.ProgramRandomCardAcquisitionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramRandomCardAcquisitionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramRandomCardAcquisitionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRandomCardAcquisitionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDamageCardClaimRuntime))]
internal sealed class CoreProgramLifecycleProgramDamageCardClaimAdapter(global::System.Func<global::CardGame.Core.ProgramDamageCardClaimRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramDamageCardClaimOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDamageCardClaimRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageCardClaimRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreProgramLifecycleProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPindianRuntime))]
internal sealed class CoreProgramLifecycleProgramPindianAdapter(global::System.Func<global::CardGame.Core.ProgramPindianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramPindianOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPindianRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPindianRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTurnWindowEntryRuntime))]
internal sealed class CoreProgramLifecycleProgramTurnWindowEntryAdapter(global::System.Func<global::CardGame.Core.ProgramTurnWindowEntryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTurnWindowEntryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTurnWindowEntryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTurnWindowEntryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerFactProjector))]
internal sealed class CoreProgramLifecycleProgramTriggerFactProjectorAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramTriggerFactProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerFactProjector Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramJudgmentRuntime))]
internal sealed class CoreProgramLifecycleProgramJudgmentAdapter(global::System.Func<global::CardGame.Core.ProgramJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramJudgmentRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDrawRuntime))]
internal sealed class CoreProgramLifecycleProgramDrawAdapter(global::System.Func<global::CardGame.Core.ProgramDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDrawRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseBoundaryRuntime))]
internal sealed class CoreProgramLifecycleProgramPhaseBoundaryAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramPhaseBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime))]
internal sealed class CoreProgramLifecycleProgramDrawDiscardWindowEntryAdapter(global::System.Func<global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramDrawDiscardWindowEntryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramHealthStateRuntime))]
internal sealed class CoreProgramLifecycleProgramHealthStateAdapter(global::System.Func<global::CardGame.Core.ProgramHealthStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramHealthStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramHealthStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramHealthStateRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramBoundCardRuntime))]
internal sealed class CoreProgramLifecycleProgramBoundCardAdapter(global::System.Func<global::CardGame.Core.ProgramBoundCardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramBoundCardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramBoundCardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramBoundCardRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramActivationWindowRuntime))]
internal sealed class CoreProgramLifecycleProgramActivationWindowAdapter(global::System.Func<global::CardGame.Core.ProgramActivationWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramActivationWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramActivationWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActivationWindowRuntime Get() => this._targetFactory();
}

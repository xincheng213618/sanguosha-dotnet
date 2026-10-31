#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime))]
internal sealed class CoreHellActivitiesCoreHellEncounterHostAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHellActivitiesCoreHellEncounterHostOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellEncounterHostRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IHellEncounterFeature? HellFeature { get => this._targetFactory().HellFeature; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreHellActivitiesCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHellActivitiesCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasArmor(global::CardGame.Core.CharacterState player) => this._targetFactory().HasArmor(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed class CoreHellActivitiesCoreHellMengpoProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHellActivitiesCoreHellMengpoProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _targetFactory = targetFactory;
    public bool HasHellShiyouCapability { get => this._targetFactory().HasHellShiyouCapability; }

    public void CompleteHellShiyouDiscardTransfers(global::CardGame.Core.HellShiyouTransferPlan plan, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => this._targetFactory().CompleteHellShiyouDiscardTransfers(plan, movements);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreHellActivitiesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHellActivitiesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreHellActivitiesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHellActivitiesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreHellActivitiesCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHellActivitiesCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => this._targetFactory().CurrentActualDiscardRecoveryPhase();
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHellActivitiesCoreHellEncounterHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IHellEncounterFeature? HellFeature { get; }
}

internal interface ICoreHellActivitiesCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmor(global::CardGame.Core.CharacterState player);
}

internal interface ICoreHellActivitiesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHellActivitiesCoreHellMengpoProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHellShiyouCapability { get; }

    void CompleteHellShiyouDiscardTransfers(global::CardGame.Core.HellShiyouTransferPlan plan, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements);
}

internal interface ICoreHellActivitiesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreHellActivitiesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreHellActivitiesCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
}

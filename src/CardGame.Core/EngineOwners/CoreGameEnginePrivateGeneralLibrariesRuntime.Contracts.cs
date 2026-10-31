#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePrivateGeneralLibrariesCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long CaptureLordProjectionDependencyStamp();
}

internal interface ICorePrivateGeneralLibrariesCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNationalWarMode { get; }

    bool IsDualGeneralMode { get; }
}

internal interface ICorePrivateGeneralLibrariesCoreConversionStatesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICorePrivateGeneralLibrariesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePrivateGeneralLibrariesCoreNativeFactionChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? CaptureNativeAvatarFactionBeforeDeclaration(global::CardGame.Core.CharacterState player);
    void CompleteNativeAvatarFactionDeclaration(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CharacterState player, string? previous);
}

internal interface ICorePrivateGeneralLibrariesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICorePrivateGeneralLibrariesCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICorePrivateGeneralLibrariesRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICorePrivateGeneralLibrariesCoreModeAllegianceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}

internal interface ICorePrivateGeneralLibrariesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICorePrivateGeneralLibrariesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICorePrivateGeneralLibrariesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICorePrivateGeneralLibrariesCorePrintedLordSkillQualificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICorePrivateGeneralLibrariesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICorePrivateGeneralLibrariesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICorePrivateGeneralLibrariesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePrivateGeneralLibrariesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

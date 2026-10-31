#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public long CaptureLordProjectionDependencyStamp() => this._targetFactory().CaptureLordProjectionDependencyStamp();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
    public bool IsDualGeneralMode { get => this._targetFactory().IsDualGeneralMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreConversionStatesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreConversionStatesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> _targetFactory = targetFactory;
    public void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().RegisterTaggedConversionSkill(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreNativeFactionChangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreNativeFactionChangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> _targetFactory = targetFactory;
    public string? CaptureNativeAvatarFactionBeforeDeclaration(global::CardGame.Core.CharacterState player) => this._targetFactory().CaptureNativeAvatarFactionBeforeDeclaration(player);
    public void CompleteNativeAvatarFactionDeclaration(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CharacterState player, string? previous) => this._targetFactory().CompleteNativeAvatarFactionDeclaration(frame, player, previous);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreCurrentTurnSkillSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreCurrentTurnSkillSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsCurrentTurnSkillGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreModeAllegianceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreModeAllegianceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime))]
internal sealed class CorePrivateGeneralLibrariesCorePrintedLordSkillQualificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCorePrintedLordSkillQualificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> _targetFactory = targetFactory;
    public bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsPrintedLordGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CorePrivateGeneralLibrariesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CorePrivateGeneralLibrariesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrivateGeneralLibrariesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

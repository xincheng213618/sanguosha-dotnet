#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.BoundaryYuanShuRuntime))]
internal sealed class CoreLordSkillProjectionsBoundaryYuanShuAdapter(global::System.Func<global::CardGame.Core.BoundaryYuanShuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsBoundaryYuanShuOperations
{
    private readonly global::System.Func<global::CardGame.Core.BoundaryYuanShuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.BoundaryYuanShuRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime))]
internal sealed class CoreLordSkillProjectionsCorePrintedLordSkillQualificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCorePrintedLordSkillQualificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> _targetFactory = targetFactory;
    public bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsPrintedLordGrantQualified(owner, grant);
    public string? PreferredQualifiedPrintedLordInstance(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().PreferredQualifiedPrintedLordInstance(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreLordSkillProjectionsCoreModeAllegianceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCoreModeAllegianceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed class CoreLordSkillProjectionsCoreCurrentTurnSkillSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCoreCurrentTurnSkillSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsCurrentTurnSkillGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreLordSkillProjectionsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreLordSkillProjectionsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
    public bool IsDualGeneralMode { get => this._targetFactory().IsDualGeneralMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreLordSkillProjectionsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime))]
internal sealed class CoreLordSkillProjectionsCorePrivateGeneralLibrariesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCorePrivateGeneralLibrariesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> _targetFactory = targetFactory;
    public bool IsGeneralLibraryGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsGeneralLibraryGrantQualified(owner, grant);
    public bool HasPrivateGeneralLibraryCapability { get => this._targetFactory().HasPrivateGeneralLibraryCapability; }

    public global::System.Collections.Generic.IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(global::CardGame.Core.CharacterState owner) => this._targetFactory().PrivateGeneralLibrarySuppressionInputs(owner);
}

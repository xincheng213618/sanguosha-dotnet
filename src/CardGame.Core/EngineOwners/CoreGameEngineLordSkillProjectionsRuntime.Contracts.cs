#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreLordSkillProjectionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreLordSkillProjectionsBoundaryYuanShuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.BoundaryYuanShuRuntime Get();
}

internal interface ICoreLordSkillProjectionsCorePrintedLordSkillQualificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    string? PreferredQualifiedPrintedLordInstance(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreLordSkillProjectionsCoreModeAllegianceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}

internal interface ICoreLordSkillProjectionsCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreLordSkillProjectionsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreLordSkillProjectionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreLordSkillProjectionsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNationalWarMode { get; }

    bool IsDualGeneralMode { get; }
}

internal interface ICoreLordSkillProjectionsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreLordSkillProjectionsCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGeneralLibraryGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    bool HasPrivateGeneralLibraryCapability { get; }

    global::System.Collections.Generic.IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(global::CardGame.Core.CharacterState owner);
}

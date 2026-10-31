#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHpConditionalSkillGrantsCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProjectedGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreHpConditionalSkillGrantsCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool LibraryBaseGrantLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    bool IsGeneralLibraryGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    bool HasPrivateGeneralLibraryCapability { get; }

    global::System.Collections.Generic.IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreHpConditionalSkillGrantsCharacterTangZiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesTangZiWoundedGrant(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramHpSkillGrant rule);
}

internal interface ICoreHpConditionalSkillGrantsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreHpConditionalSkillGrantsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreHpConditionalSkillGrantsCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreHpConditionalSkillGrantsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

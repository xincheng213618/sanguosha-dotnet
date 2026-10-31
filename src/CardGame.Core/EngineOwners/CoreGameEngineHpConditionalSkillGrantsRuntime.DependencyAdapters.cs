#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CoreHpConditionalSkillGrantsCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpConditionalSkillGrantsCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public bool IsProjectedGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsProjectedGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime))]
internal sealed class CoreHpConditionalSkillGrantsCorePrivateGeneralLibrariesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpConditionalSkillGrantsCorePrivateGeneralLibrariesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> _targetFactory = targetFactory;
    public bool LibraryBaseGrantLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().LibraryBaseGrantLive(owner, grant);
    public bool IsGeneralLibraryGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsGeneralLibraryGrantQualified(owner, grant);
    public bool HasPrivateGeneralLibraryCapability { get => this._targetFactory().HasPrivateGeneralLibraryCapability; }

    public global::System.Collections.Generic.IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(global::CardGame.Core.CharacterState owner) => this._targetFactory().PrivateGeneralLibrarySuppressionInputs(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTangZiOlRuntime))]
internal sealed class CoreHpConditionalSkillGrantsCharacterTangZiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangZiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpConditionalSkillGrantsCharacterTangZiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangZiOlRuntime> _targetFactory = targetFactory;
    public bool MatchesTangZiWoundedGrant(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramHpSkillGrant rule) => this._targetFactory().MatchesTangZiWoundedGrant(owner, rule);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreHpConditionalSkillGrantsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpConditionalSkillGrantsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed class CoreHpConditionalSkillGrantsCoreCurrentTurnSkillSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpConditionalSkillGrantsCoreCurrentTurnSkillSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsCurrentTurnSkillGrantQualified(owner, grant);
}

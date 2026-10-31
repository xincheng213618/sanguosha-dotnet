#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CorePrintedLordSkillQualificationsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordSkillQualificationsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed class CorePrintedLordSkillQualificationsCoreGuanduActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordSkillQualificationsCoreGuanduActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsGuanduCommanderPrintedLordGrant(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsGuanduCommanderPrintedLordGrant(player, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime))]
internal sealed class CorePrintedLordSkillQualificationsCoreScenarioPrintedLordQualificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordSkillQualificationsCoreScenarioPrintedLordQualificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> _targetFactory = targetFactory;
    public bool IsShanhePrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsShanhePrintedLordGrantQualified(owner, grant);
}

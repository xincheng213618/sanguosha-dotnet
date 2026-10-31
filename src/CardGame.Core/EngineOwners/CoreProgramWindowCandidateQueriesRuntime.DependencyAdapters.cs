#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerRuntime))]
internal sealed class CoreProgramWindowCandidateQueriesProgramTriggerAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesProgramTriggerOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramWindowCandidateQueriesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime))]
internal sealed class CoreProgramWindowCandidateQueriesCharacterYuanJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesCharacterYuanJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeYuanJiIssuedReturns(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => this._targetFactory().IncludeYuanJiIssuedReturns(owner, window, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerFactProjector))]
internal sealed class CoreProgramWindowCandidateQueriesProgramTriggerFactProjectorAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerFactProjector Get() => this._targetFactory();
}

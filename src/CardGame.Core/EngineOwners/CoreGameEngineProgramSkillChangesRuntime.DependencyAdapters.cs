#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramSkillChangeRuntime))]
internal sealed class CoreProgramSkillChangesProgramSkillChangeAdapter(global::System.Func<global::CardGame.Core.ProgramSkillChangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesProgramSkillChangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramSkillChangeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillChangeRuntime Get() => this._targetFactory();
}

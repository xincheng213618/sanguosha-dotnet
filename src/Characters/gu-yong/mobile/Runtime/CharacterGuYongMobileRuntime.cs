#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGuYongMobileRuntime : global::CardGame.Core.IMobileGuYongHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries _coreProgramCategoryChallengesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    internal CharacterGuYongMobileRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries coreProgramCategoryChallengesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations)
    {
        this._coreProgramCategoryChallengesOperations = coreProgramCategoryChallengesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGuYongMobileRuntime : global::CardGame.Core.IMobileGuYongHost
{
    private readonly ICharacterGuYongMobileCoreProgramCategoryChallengesOperations _coreProgramCategoryChallengesOperations;
    private readonly ICharacterGuYongMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterGuYongMobileCoreSkillProgramsOperations _coreSkillProgramsOperations;
    internal CharacterGuYongMobileRuntime(ICharacterGuYongMobileCoreProgramCategoryChallengesOperations coreProgramCategoryChallengesOperations, ICharacterGuYongMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterGuYongMobileCoreSkillProgramsOperations coreSkillProgramsOperations)
    {
        this._coreProgramCategoryChallengesOperations = coreProgramCategoryChallengesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
    }
}

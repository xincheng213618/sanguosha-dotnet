#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterLuJiMobileRuntime
{
    private readonly ICharacterLuJiMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    internal CharacterLuJiMobileRuntime(ICharacterLuJiMobileCoreNationalSkillsOperations coreNationalSkillsOperations)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
    }
}

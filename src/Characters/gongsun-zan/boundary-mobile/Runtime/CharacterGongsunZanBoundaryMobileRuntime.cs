#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterGongsunZanBoundaryMobileRuntime
{
    private readonly ICharacterGongsunZanBoundaryMobileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterGongsunZanBoundaryMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    internal CharacterGongsunZanBoundaryMobileRuntime(ICharacterGongsunZanBoundaryMobileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterGongsunZanBoundaryMobileCoreNationalSkillsOperations coreNationalSkillsOperations)
    {
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
    }
}

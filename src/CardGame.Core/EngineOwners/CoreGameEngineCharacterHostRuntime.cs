#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCharacterHostRuntime
{
    private readonly ICoreCharacterHostContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreCharacterHostCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly ICoreCharacterHostCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    internal CoreGameEngineCharacterHostRuntime(ICoreCharacterHostContentRegistryOperations contentRegistryOperations, ICoreCharacterHostCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, ICoreCharacterHostCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
    }
}

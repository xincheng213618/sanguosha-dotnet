#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCharacterHostRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries _coreProgramSkillChangesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    internal CoreGameEngineCharacterHostRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries coreProgramSkillChangesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
    }
}

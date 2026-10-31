#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeDrawOrdinalRuntimePortsRuntime : global::CardGame.Core.INativeDrawOrdinalSkillInstances
{
    private readonly ICoreNativeDrawOrdinalRuntimePortsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    internal CoreNativeDrawOrdinalRuntimePortsRuntime(ICoreNativeDrawOrdinalRuntimePortsCoreNationalSkillsOperations coreNationalSkillsOperations)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
    }
}

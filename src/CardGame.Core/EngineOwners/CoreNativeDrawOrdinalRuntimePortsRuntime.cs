#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeDrawOrdinalRuntimePortsRuntime : global::CardGame.Core.INativeDrawOrdinalSkillInstances
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    internal CoreNativeDrawOrdinalRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
    }
}

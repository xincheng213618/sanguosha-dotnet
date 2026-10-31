#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramCapabilitiesRuntime : global::CardGame.Core.IActionCategoryGiftProgramHost, global::CardGame.Core.IConfiguredConversionProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionCategoryGiftsEntries _coreActionCategoryGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineConfiguredConversionsEntries _coreConfiguredConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries _coreProgramSkillChangesOperations;
    internal CoreProgramCapabilitiesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineActionCategoryGiftsEntries coreActionCategoryGiftsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineConfiguredConversionsEntries coreConfiguredConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries coreProgramSkillChangesOperations)
    {
        this._coreActionCategoryGiftsOperations = coreActionCategoryGiftsOperations;
        this._coreConfiguredConversionsOperations = coreConfiguredConversionsOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramCapabilitiesRuntime : global::CardGame.Core.IActionCategoryGiftProgramHost, global::CardGame.Core.IConfiguredConversionProgramHost
{
    private readonly ICoreProgramCapabilitiesCoreActionCategoryGiftsOperations _coreActionCategoryGiftsOperations;
    private readonly ICoreProgramCapabilitiesCoreConfiguredConversionsOperations _coreConfiguredConversionsOperations;
    private readonly ICoreProgramCapabilitiesCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    internal CoreProgramCapabilitiesRuntime(ICoreProgramCapabilitiesCoreActionCategoryGiftsOperations coreActionCategoryGiftsOperations, ICoreProgramCapabilitiesCoreConfiguredConversionsOperations coreConfiguredConversionsOperations, ICoreProgramCapabilitiesCoreProgramSkillChangesOperations coreProgramSkillChangesOperations)
    {
        this._coreActionCategoryGiftsOperations = coreActionCategoryGiftsOperations;
        this._coreConfiguredConversionsOperations = coreConfiguredConversionsOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
    }
}

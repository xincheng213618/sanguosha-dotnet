#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramCapabilitiesCoreConfiguredConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginConfiguredCardDeclaration(global::CardGame.Core.ProgramSkillFrame frame, string bind, string state);
}

internal interface ICoreProgramCapabilitiesCoreActionCategoryGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginDifferentActionCategoryGift(global::CardGame.Core.ProgramSkillFrame frame, int provider, string resultBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones);
}

internal interface ICoreProgramCapabilitiesCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpgradeConfiguredConversionTier(global::CardGame.Core.ProgramSkillFrame frame, string state);
}

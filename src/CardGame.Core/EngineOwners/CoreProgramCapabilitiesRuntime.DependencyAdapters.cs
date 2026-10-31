#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime))]
internal sealed class CoreProgramCapabilitiesCoreConfiguredConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCapabilitiesCoreConfiguredConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginConfiguredCardDeclaration(global::CardGame.Core.ProgramSkillFrame frame, string bind, string state) => this._targetFactory().BeginConfiguredCardDeclaration(frame, bind, state);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime))]
internal sealed class CoreProgramCapabilitiesCoreActionCategoryGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCapabilitiesCoreActionCategoryGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginDifferentActionCategoryGift(global::CardGame.Core.ProgramSkillFrame frame, int provider, string resultBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones) => this._targetFactory().BeginDifferentActionCategoryGift(frame, provider, resultBind, zones);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime))]
internal sealed class CoreProgramCapabilitiesCoreProgramSkillChangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCapabilitiesCoreProgramSkillChangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _targetFactory = targetFactory;
    public void UpgradeConfiguredConversionTier(global::CardGame.Core.ProgramSkillFrame frame, string state) => this._targetFactory().UpgradeConfiguredConversionTier(frame, state);
}

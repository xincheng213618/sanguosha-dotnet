#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramViewContributorsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramViewContributorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramViewContributorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramViewContributorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramViewContributorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramViewContributors ViewContributors { get => this._targetFactory().ViewContributors; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

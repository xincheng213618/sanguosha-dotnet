#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeCoreInvariantsCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreNativeCoreInvariantsCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreNativeCoreInvariantsCoreProgramViewContributorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramViewContributors ViewContributors { get; }
}

internal interface ICoreNativeCoreInvariantsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

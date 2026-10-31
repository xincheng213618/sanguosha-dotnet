#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterQuHuangOlCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterQuHuangOlQuHuangPhaseEndLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.QuHuangPhaseEndLedger Get();
}

internal interface ICharacterQuHuangOlQuHuangOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.QuHuangRuntime Get();
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuanShuJunOlCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterHuanShuJunOlCoreCommandSessionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long Revision { get; }
}

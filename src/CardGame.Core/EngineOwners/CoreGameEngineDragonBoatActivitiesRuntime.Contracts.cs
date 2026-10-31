#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDragonBoatActivitiesCoreActivityBattleRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IDragonBoatBattleRulesFeature? DragonBoatRules { get; }
}

internal interface ICoreDragonBoatActivitiesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreScenarioGodFactionsCoreModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeFactionPolicy ModeFactionPolicy { get; }
}

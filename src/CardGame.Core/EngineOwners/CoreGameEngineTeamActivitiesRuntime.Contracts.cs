#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTeamActivitiesCoreModeAllegianceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}

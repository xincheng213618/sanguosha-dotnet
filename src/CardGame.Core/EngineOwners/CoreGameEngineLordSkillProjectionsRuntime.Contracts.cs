#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreLordSkillProjectionsBoundaryYuanShuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.BoundaryYuanShuRuntime Get();
}

internal interface ICoreLordSkillProjectionsCoreModeAllegianceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhouYuMouMouZhouYuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouZhouYuRuntime Get();
}

internal interface ICharacterZhouYuMouCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

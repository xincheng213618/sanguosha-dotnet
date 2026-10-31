#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterJiangJiMouMouJiangJiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouJiangJiRuntime Get();
}

internal interface ICharacterJiangJiMouCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

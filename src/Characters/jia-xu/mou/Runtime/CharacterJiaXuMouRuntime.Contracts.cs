#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterJiaXuMouCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterJiaXuMouMouJiaXuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouJiaXuRuntime Get();
}

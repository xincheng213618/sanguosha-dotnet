#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuCheErMouMouHuCheErOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouHuCheErRuntime Get();
}

internal interface ICharacterHuCheErMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuCheErMouCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

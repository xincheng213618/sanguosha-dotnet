#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDongZhuoMouOlMouDongZhuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouDongZhuoOlRuntime Get();
}

internal interface ICharacterDongZhuoMouOlCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterDongZhuoMouOlCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

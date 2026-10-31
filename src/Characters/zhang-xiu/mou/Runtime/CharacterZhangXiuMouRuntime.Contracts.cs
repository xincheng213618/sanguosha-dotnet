#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhangXiuMouCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterZhangXiuMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterZhangXiuMouMouZhangXiuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouZhangXiuAnniversaryRuntime Get();
}

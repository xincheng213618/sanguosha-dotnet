#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterXiaoQiaoMouOlCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterXiaoQiaoMouOlCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterXiaoQiaoMouOlMouXiaoQiaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouXiaoQiaoOlRuntime Get();
}

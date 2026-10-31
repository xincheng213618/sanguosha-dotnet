#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramOtherOwnedCardDiscardsCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddThought(global::CardGame.Core.AiThoughtRecord thought);
}

internal interface ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPaidCardRuntime Get();
}

internal interface ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreProgramOtherOwnedCardDiscardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

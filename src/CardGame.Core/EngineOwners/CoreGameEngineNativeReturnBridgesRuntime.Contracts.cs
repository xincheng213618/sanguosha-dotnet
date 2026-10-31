#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeReturnBridgesCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDyingCardResponse(global::CardGame.Core.DyingResponseEvent response);
}

internal interface ICoreNativeReturnBridgesCoreQinglongInsertionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RestoreContinuationAfterQinglongFollowup(long completedOwnerId);
}

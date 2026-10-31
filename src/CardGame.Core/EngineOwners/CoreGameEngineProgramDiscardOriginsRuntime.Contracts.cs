#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDiscardOriginsCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheStartingResourceDestroyedMaterial(global::CardGame.Core.CardMovementRecord move);
}

internal interface ICoreProgramDiscardOriginsCharacterZhangFenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDagongCheDestroyedMaterial(global::CardGame.Core.CardMovementRecord m);
}

internal interface ICoreProgramDiscardOriginsCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? WushuangJinzhiDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}

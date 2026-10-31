#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterSimaYiOlGod2016ContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterSimaYiOlGod2016CoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}

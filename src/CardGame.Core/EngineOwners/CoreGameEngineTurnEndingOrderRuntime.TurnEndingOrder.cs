#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnEndingOrderRuntime
{
    internal bool TryResolveTurnEndingOrder(PromptChoice choice) => _programPhaseBoundaryOperations.Get().TryResolveTurnEndingOrder(choice);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHandCardGuidanceProjectionRuntime
{
    internal IReadOnlyList<HandCardGuidance> GetHumanHandGuidance() => _coreHandGuidanceOperations.BuildHumanHandGuidance(_coreNativeLegalActionsOperations.GetHumanLegalActions());
}

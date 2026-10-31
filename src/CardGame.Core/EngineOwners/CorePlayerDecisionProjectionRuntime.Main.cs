#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePlayerDecisionProjectionRuntime
{
    internal PendingDecision? PendingDecision => _coreProgramParticipantPromptsOperations.GetPendingDecisionForActor(_coreMatchDefinitionOptionsAccess._options.HumanSeat)is { } decision ? global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime.CloneDecision(decision) : null;
}

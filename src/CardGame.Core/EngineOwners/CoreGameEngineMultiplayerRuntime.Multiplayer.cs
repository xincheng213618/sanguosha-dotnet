#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMultiplayerRuntime
{
    internal bool IsHumanControlledSeat(int seat) => _coreMatchDefinitionOptionsAccess._options.HumanSeats is { } seats ? seats.Contains(seat) : seat == _coreMatchDefinitionOptionsAccess._options.HumanSeat;
    internal int HumanDecisionSeat => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } pending && IsHumanControlledSeat(pending.PlayerSeat) ? pending.PlayerSeat : throw new InvalidOperationException("There is no human decision awaiting input.");
}

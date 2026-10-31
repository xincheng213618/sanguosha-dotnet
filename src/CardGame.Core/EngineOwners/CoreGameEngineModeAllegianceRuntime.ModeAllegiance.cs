#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeAllegianceRuntime
{
    internal IMatchModeAllegiancePolicy ModeAllegiancePolicy => _allegiancePolicyOperations._allegiancePolicy ??= _modeRuntimeOperations._modeRuntime.Find<IMatchModeAllegiancePolicy>() ?? throw new InvalidOperationException("The mode requires its installed allegiance policy.");

    internal static ModeAllegianceSeatFacts CaptureAllegianceSeat(CharacterState player) => new(player.Seat, player.IsAlive, player.TeamId);
}

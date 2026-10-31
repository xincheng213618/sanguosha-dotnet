#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    internal bool GuanSparedThisTurn(int owner, int target) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuanLethalSparedEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == target && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
}

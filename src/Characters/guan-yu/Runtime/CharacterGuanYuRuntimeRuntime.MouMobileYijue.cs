#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    internal bool GuanSparedThisTurn(int owner, int target) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuanLethalSparedEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == target && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
}

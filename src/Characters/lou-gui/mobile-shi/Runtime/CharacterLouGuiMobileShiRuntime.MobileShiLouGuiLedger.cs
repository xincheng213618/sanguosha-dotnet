#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLouGuiMobileShiRuntime
{
    internal int ShiLgGuanshaHandLimitBonus(int seat) => !HasShiLgCapability ? 0 : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiLgGuanshaAppliedEvent>().Where(entry => entry.OwnerSeat == seat && entry.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber).Sum(entry => entry.DistinctNames);
}

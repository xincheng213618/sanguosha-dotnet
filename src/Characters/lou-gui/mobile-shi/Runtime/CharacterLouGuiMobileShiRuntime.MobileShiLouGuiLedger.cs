#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLouGuiMobileShiRuntime
{
    internal int ShiLgGuanshaHandLimitBonus(int seat) => !HasShiLgCapability ? 0 : _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLgGuanshaAppliedEvent>().Where(entry => entry.OwnerSeat == seat && entry.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber).Sum(entry => entry.DistinctNames);
}

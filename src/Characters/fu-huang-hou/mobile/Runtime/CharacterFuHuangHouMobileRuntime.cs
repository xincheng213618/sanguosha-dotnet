#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterFuHuangHouMobileRuntime
{
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess;
    internal CharacterFuHuangHouMobileRuntime(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess)
    {
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess = coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess;
    }
}

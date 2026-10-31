#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterGuanPingBoundaryRuntime
{
    private readonly ICharacterGuanPingBoundaryCardZonesOperations _cardZonesOperations;
    private readonly ICharacterGuanPingBoundaryCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICharacterGuanPingBoundaryCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CharacterGuanPingBoundaryRuntime(ICharacterGuanPingBoundaryCardZonesOperations cardZonesOperations, ICharacterGuanPingBoundaryCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICharacterGuanPingBoundaryCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}

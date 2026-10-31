#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineLastZoneConversionsRuntime
{
    private readonly ICoreLastZoneConversionsCardZonesOperations _cardZonesOperations;
    private readonly ICoreLastZoneConversionsCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreLastZoneConversionsCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreLastZoneConversionsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineLastZoneConversionsRuntime(ICoreLastZoneConversionsCardZonesOperations cardZonesOperations, ICoreLastZoneConversionsCoreActionQueriesOperations coreActionQueriesOperations, ICoreLastZoneConversionsCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreLastZoneConversionsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}

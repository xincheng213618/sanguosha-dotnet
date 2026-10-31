#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDragonBoatBasicConversionsRuntime
{
    private readonly ICoreDragonBoatBasicConversionsModeRulesOperations _modeRulesOperations;
    private readonly ICoreDragonBoatBasicConversionsCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreDragonBoatBasicConversionsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreDragonBoatBasicConversionsCoreDragonBoatCardsOperations _coreDragonBoatCardsOperations;
    private readonly ICoreDragonBoatBasicConversionsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineDragonBoatBasicConversionsRuntime(ICoreDragonBoatBasicConversionsModeRulesOperations modeRulesOperations, ICoreDragonBoatBasicConversionsCoreActionQueriesOperations coreActionQueriesOperations, ICoreDragonBoatBasicConversionsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreDragonBoatBasicConversionsCoreDragonBoatCardsOperations coreDragonBoatCardsOperations, ICoreDragonBoatBasicConversionsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDragonBoatCardsOperations = coreDragonBoatCardsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}

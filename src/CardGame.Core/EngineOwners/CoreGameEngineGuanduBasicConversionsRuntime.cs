#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduBasicConversionsRuntime
{
    private readonly ICoreGuanduBasicConversionsModeRulesOperations _modeRulesOperations;
    private readonly ICoreGuanduBasicConversionsCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreGuanduBasicConversionsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreGuanduBasicConversionsCoreGuanduCardsOperations _coreGuanduCardsOperations;
    private readonly ICoreGuanduBasicConversionsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineGuanduBasicConversionsRuntime(ICoreGuanduBasicConversionsModeRulesOperations modeRulesOperations, ICoreGuanduBasicConversionsCoreActionQueriesOperations coreActionQueriesOperations, ICoreGuanduBasicConversionsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreGuanduBasicConversionsCoreGuanduCardsOperations coreGuanduCardsOperations, ICoreGuanduBasicConversionsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreGuanduCardsOperations = coreGuanduCardsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}

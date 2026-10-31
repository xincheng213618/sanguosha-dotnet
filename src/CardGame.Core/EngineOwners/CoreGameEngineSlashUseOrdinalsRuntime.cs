#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSlashUseOrdinalsRuntime
{
    private readonly ICoreSlashUseOrdinalsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreSlashUseOrdinalsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSlashUseOrdinalsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreSlashUseOrdinalsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreSlashUseOrdinalsCoreCurrentSlashFireOffersOperations _coreCurrentSlashFireOffersOperations;
    private readonly ICoreSlashUseOrdinalsCoreWholeActualTurnUsesOperations _coreWholeActualTurnUsesOperations;
    private readonly ICoreSlashUseOrdinalsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    internal CoreGameEngineSlashUseOrdinalsRuntime(ICoreSlashUseOrdinalsContentRegistryOperations contentRegistryOperations, ICoreSlashUseOrdinalsResolutionStackOperations resolutionStackOperations, ICoreSlashUseOrdinalsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreSlashUseOrdinalsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreSlashUseOrdinalsCoreCurrentSlashFireOffersOperations coreCurrentSlashFireOffersOperations, ICoreSlashUseOrdinalsCoreWholeActualTurnUsesOperations coreWholeActualTurnUsesOperations, ICoreSlashUseOrdinalsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
    }
}

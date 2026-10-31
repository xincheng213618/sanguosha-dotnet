#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRoundFirstSlashTargetsRuntime
{
    private readonly ICoreRoundFirstSlashTargetsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRoundFirstSlashTargetsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRoundFirstSlashTargetsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreRoundFirstSlashTargetsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreRoundFirstSlashTargetsCoreSlashUseOrdinalsOperations _coreSlashUseOrdinalsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CoreGameEngineRoundFirstSlashTargetsRuntime(ICoreRoundFirstSlashTargetsContentRegistryOperations contentRegistryOperations, ICoreRoundFirstSlashTargetsResolutionStackOperations resolutionStackOperations, ICoreRoundFirstSlashTargetsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreRoundFirstSlashTargetsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreRoundFirstSlashTargetsCoreSlashUseOrdinalsOperations coreSlashUseOrdinalsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreSlashUseOrdinalsOperations = coreSlashUseOrdinalsOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineJudgmentOutcomeClaimsRuntime
{
    private readonly ICoreJudgmentOutcomeClaimsCardZonesOperations _cardZonesOperations;
    private readonly ICoreJudgmentOutcomeClaimsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreJudgmentOutcomeClaimsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreJudgmentOutcomeClaimsCoreJudgmentOutcomeDamageEdgesOperations _coreJudgmentOutcomeDamageEdgesOperations;
    private readonly ICoreJudgmentOutcomeClaimsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreJudgmentOutcomeClaimsCoreProgramJudgmentOutcomesOperations _coreProgramJudgmentOutcomesOperations;
    private readonly ICoreJudgmentOutcomeClaimsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreJudgmentOutcomeClaimsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreJudgmentOutcomeClaimsCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreJudgmentOutcomeClaimsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineJudgmentOutcomeClaimsRuntime(ICoreJudgmentOutcomeClaimsCardZonesOperations cardZonesOperations, ICoreJudgmentOutcomeClaimsContentRegistryOperations contentRegistryOperations, ICoreJudgmentOutcomeClaimsResolutionStackOperations resolutionStackOperations, ICoreJudgmentOutcomeClaimsCoreJudgmentOutcomeDamageEdgesOperations coreJudgmentOutcomeDamageEdgesOperations, ICoreJudgmentOutcomeClaimsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreJudgmentOutcomeClaimsCoreProgramJudgmentOutcomesOperations coreProgramJudgmentOutcomesOperations, ICoreJudgmentOutcomeClaimsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreJudgmentOutcomeClaimsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreJudgmentOutcomeClaimsCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreJudgmentOutcomeClaimsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreJudgmentOutcomeDamageEdgesOperations = coreJudgmentOutcomeDamageEdgesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramJudgmentOutcomesOperations = coreProgramJudgmentOutcomesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}

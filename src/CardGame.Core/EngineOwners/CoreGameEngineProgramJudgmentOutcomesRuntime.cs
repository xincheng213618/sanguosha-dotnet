#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramJudgmentOutcomesRuntime
{
    private readonly ICoreProgramJudgmentOutcomesCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramJudgmentOutcomesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramJudgmentOutcomesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramJudgmentOutcomesCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreProgramJudgmentOutcomesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramJudgmentOutcomesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramJudgmentOutcomesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramJudgmentOutcomesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramJudgmentOutcomesRuntime(ICoreProgramJudgmentOutcomesCardZonesOperations cardZonesOperations, ICoreProgramJudgmentOutcomesContentRegistryOperations contentRegistryOperations, ICoreProgramJudgmentOutcomesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramJudgmentOutcomesCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreProgramJudgmentOutcomesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramJudgmentOutcomesCoreRuntimeOperations coreRuntimeOperations, ICoreProgramJudgmentOutcomesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramJudgmentOutcomesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

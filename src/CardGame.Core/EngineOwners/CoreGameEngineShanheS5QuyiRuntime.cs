#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheS5QuyiRuntime
{
    private readonly ICoreShanheS5QuyiContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5QuyiCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheS5QuyiCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreShanheS5QuyiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5QuyiCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreShanheS5QuyiCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICoreShanheS5QuyiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheS5QuyiCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5QuyiRuntime(ICoreShanheS5QuyiContentRegistryOperations contentRegistryOperations, ICoreShanheS5QuyiCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheS5QuyiCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreShanheS5QuyiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5QuyiCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreShanheS5QuyiCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICoreShanheS5QuyiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheS5QuyiCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

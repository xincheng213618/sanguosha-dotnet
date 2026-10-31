#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSchoolTeamRuntime : global::CardGame.Core.IShanheSchoolTeamHost
{
    private readonly ICoreShanheSchoolTeamResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSchoolTeamShanheMarketOperations _shanheMarketOperations;
    private readonly ICoreShanheSchoolTeamCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreShanheSchoolTeamCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreShanheSchoolTeamCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheSchoolTeamCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheSchoolTeamCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSchoolTeamCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICoreShanheSchoolTeamCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly ICoreShanheSchoolTeamCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheSchoolTeamCoreShanheMarketApplicationOperations _coreShanheMarketApplicationOperations;
    private readonly ICoreShanheSchoolTeamCoreShanheSchoolsOperations _coreShanheSchoolsOperations;
    private readonly ICoreShanheSchoolTeamCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly ICoreShanheSchoolTeamKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSchoolTeamRuntime(ICoreShanheSchoolTeamResolutionStackOperations resolutionStackOperations, ICoreShanheSchoolTeamShanheMarketOperations shanheMarketOperations, ICoreShanheSchoolTeamCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreShanheSchoolTeamCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreShanheSchoolTeamCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheSchoolTeamCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheSchoolTeamCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSchoolTeamCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICoreShanheSchoolTeamCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, ICoreShanheSchoolTeamCoreRuntimeOperations coreRuntimeOperations, ICoreShanheSchoolTeamCoreShanheMarketApplicationOperations coreShanheMarketApplicationOperations, ICoreShanheSchoolTeamCoreShanheSchoolsOperations coreShanheSchoolsOperations, ICoreShanheSchoolTeamCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, ICoreShanheSchoolTeamKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._shanheMarketOperations = shanheMarketOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheMarketApplicationOperations = coreShanheMarketApplicationOperations;
        this._coreShanheSchoolsOperations = coreShanheSchoolsOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

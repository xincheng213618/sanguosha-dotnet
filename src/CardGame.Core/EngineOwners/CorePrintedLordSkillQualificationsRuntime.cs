#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePrintedLordSkillQualificationsRuntime
{
    private readonly ICorePrintedLordSkillQualificationsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePrintedLordSkillQualificationsCoreGuanduActivitiesOperations _coreGuanduActivitiesOperations;
    private readonly ICorePrintedLordSkillQualificationsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePrintedLordSkillQualificationsCoreScenarioPrintedLordQualificationsOperations _coreScenarioPrintedLordQualificationsOperations;
    private readonly ICorePrintedLordSkillQualificationsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CorePrintedLordSkillQualificationsRuntime(ICorePrintedLordSkillQualificationsContentRegistryOperations contentRegistryOperations, ICorePrintedLordSkillQualificationsCoreGuanduActivitiesOperations coreGuanduActivitiesOperations, ICorePrintedLordSkillQualificationsCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePrintedLordSkillQualificationsCoreScenarioPrintedLordQualificationsOperations coreScenarioPrintedLordQualificationsOperations, ICorePrintedLordSkillQualificationsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreScenarioPrintedLordQualificationsOperations = coreScenarioPrintedLordQualificationsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

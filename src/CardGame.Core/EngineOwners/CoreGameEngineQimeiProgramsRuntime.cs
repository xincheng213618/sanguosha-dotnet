#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineQimeiProgramsRuntime
{
    private readonly ICoreQimeiProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreQimeiProgramsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreQimeiProgramsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreQimeiProgramsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreQimeiProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreQimeiProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreQimeiProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreQimeiProgramsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreQimeiProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreQimeiProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineQimeiProgramsRuntime(ICoreQimeiProgramsResolutionStackOperations resolutionStackOperations, ICoreQimeiProgramsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreQimeiProgramsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreQimeiProgramsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreQimeiProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreQimeiProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreQimeiProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreQimeiProgramsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreQimeiProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreQimeiProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

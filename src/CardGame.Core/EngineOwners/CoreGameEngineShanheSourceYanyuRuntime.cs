#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSourceYanyuRuntime : global::CardGame.Core.IShanheSourceYanyuHost
{
    private readonly ICoreShanheSourceYanyuCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheSourceYanyuContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheSourceYanyuResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceYanyuCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreShanheSourceYanyuCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreShanheSourceYanyuCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreShanheSourceYanyuCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheSourceYanyuCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheSourceYanyuCoreProgramJudgmentOutcomesOperations _coreProgramJudgmentOutcomesOperations;
    private readonly ICoreShanheSourceYanyuCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSourceYanyuCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheSourceYanyuCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreShanheSourceYanyuCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreShanheSourceYanyuCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreShanheSourceYanyuKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceYanyuRuntime(ICoreShanheSourceYanyuCardZonesOperations cardZonesOperations, ICoreShanheSourceYanyuContentRegistryOperations contentRegistryOperations, ICoreShanheSourceYanyuResolutionStackOperations resolutionStackOperations, ICoreShanheSourceYanyuCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreShanheSourceYanyuCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreShanheSourceYanyuCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreShanheSourceYanyuCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheSourceYanyuCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheSourceYanyuCoreProgramJudgmentOutcomesOperations coreProgramJudgmentOutcomesOperations, ICoreShanheSourceYanyuCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSourceYanyuCoreRuntimeOperations coreRuntimeOperations, ICoreShanheSourceYanyuCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreShanheSourceYanyuCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreShanheSourceYanyuCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreShanheSourceYanyuKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramJudgmentOutcomesOperations = coreProgramJudgmentOutcomesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

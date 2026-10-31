#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEndingPairDrawsRuntime
{
    private readonly ICoreEndingPairDrawsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreEndingPairDrawsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreEndingPairDrawsSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreEndingPairDrawsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreEndingPairDrawsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreEndingPairDrawsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreEndingPairDrawsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreEndingPairDrawsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreEndingPairDrawsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreEndingPairDrawsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreEndingPairDrawsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreEndingPairDrawsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreEndingPairDrawsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreEndingPairDrawsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreEndingPairDrawsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineEndingPairDrawsRuntime(ICoreEndingPairDrawsContentRegistryOperations contentRegistryOperations, ICoreEndingPairDrawsResolutionStackOperations resolutionStackOperations, ICoreEndingPairDrawsSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreEndingPairDrawsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreEndingPairDrawsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreEndingPairDrawsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreEndingPairDrawsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreEndingPairDrawsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreEndingPairDrawsCoreRuntimeOperations coreRuntimeOperations, ICoreEndingPairDrawsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreEndingPairDrawsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreEndingPairDrawsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreEndingPairDrawsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreEndingPairDrawsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreEndingPairDrawsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

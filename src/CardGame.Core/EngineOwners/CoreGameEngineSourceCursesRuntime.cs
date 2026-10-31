#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineSourceCursesRuntime : global::CardGame.Core.ISourceCurseProgramHost
{
    private readonly ICoreSourceCursesCardZonesOperations _cardZonesOperations;
    private readonly ICoreSourceCursesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreSourceCursesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSourceCursesCoreAfterTurnEndedProgramsOperations _coreAfterTurnEndedProgramsOperations;
    private readonly ICoreSourceCursesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreSourceCursesCoreFireTargetBenefitsOperations _coreFireTargetBenefitsOperations;
    private readonly ICoreSourceCursesCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreSourceCursesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreSourceCursesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreSourceCursesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreSourceCursesCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreSourceCursesCoreSourceCurseObserversOperations _coreSourceCurseObserversOperations;
    private readonly ICoreSourceCursesCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreSourceCursesCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreSourceCursesCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreSourceCursesCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreSourceCursesCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreSourceCursesCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICoreSourceCursesKernelCoreGameEngineAdvancedLifecycleHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly ICoreSourceCursesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess;
    internal CoreGameEngineSourceCursesRuntime(ICoreSourceCursesCardZonesOperations cardZonesOperations, ICoreSourceCursesContentRegistryOperations contentRegistryOperations, ICoreSourceCursesResolutionStackOperations resolutionStackOperations, ICoreSourceCursesCoreAfterTurnEndedProgramsOperations coreAfterTurnEndedProgramsOperations, ICoreSourceCursesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreSourceCursesCoreFireTargetBenefitsOperations coreFireTargetBenefitsOperations, ICoreSourceCursesCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreSourceCursesCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreSourceCursesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreSourceCursesCoreRuntimeOperations coreRuntimeOperations, ICoreSourceCursesCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreSourceCursesCoreSourceCurseObserversOperations coreSourceCurseObserversOperations, ICoreSourceCursesCoreTableOrderOperations coreTableOrderOperations, ICoreSourceCursesCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreSourceCursesCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreSourceCursesCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreSourceCursesCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreSourceCursesCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICoreSourceCursesKernelCoreGameEngineAdvancedLifecycleHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, ICoreSourceCursesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreFireTargetBenefitsOperations = coreFireTargetBenefitsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreSourceCurseObserversOperations = coreSourceCurseObserversOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess = coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess;
    }
}

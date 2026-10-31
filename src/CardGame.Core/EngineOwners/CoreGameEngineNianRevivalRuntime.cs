#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineNianRevivalRuntime : global::CardGame.Core.INianRevivalHost, global::CardGame.Core.INianRevivalSelectionQuery
{
    private readonly ICoreNianRevivalCardZonesOperations _cardZonesOperations;
    private readonly ICoreNianRevivalResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNianRevivalCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreNianRevivalCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreNianRevivalCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreNianRevivalCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreNianRevivalCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreNianRevivalCoreNianChallengeHostOperations _coreNianChallengeHostOperations;
    private readonly ICoreNianRevivalCoreProgramLifeRestorationOperations _coreProgramLifeRestorationOperations;
    private readonly ICoreNianRevivalCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreNianRevivalCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICoreNianRevivalCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNianRevivalCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreNianRevivalCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreNianRevivalCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreNianRevivalCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreNianRevivalKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNianRevivalRuntime(ICoreNianRevivalCardZonesOperations cardZonesOperations, ICoreNianRevivalResolutionStackOperations resolutionStackOperations, ICoreNianRevivalCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreNianRevivalCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreNianRevivalCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreNianRevivalCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreNianRevivalCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreNianRevivalCoreNianChallengeHostOperations coreNianChallengeHostOperations, ICoreNianRevivalCoreProgramLifeRestorationOperations coreProgramLifeRestorationOperations, ICoreNianRevivalCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreNianRevivalCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICoreNianRevivalCoreRuntimeOperations coreRuntimeOperations, ICoreNianRevivalCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreNianRevivalCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreNianRevivalCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreNianRevivalCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreNianRevivalKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNianChallengeHostOperations = coreNianChallengeHostOperations;
        this._coreProgramLifeRestorationOperations = coreProgramLifeRestorationOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

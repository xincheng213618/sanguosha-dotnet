#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecoveryAmountBonusesRuntime
{
    private readonly ICoreRecoveryAmountBonusesCharacterCaoSongOlOperations _characterCaoSongOlOperations;
    private readonly ICoreRecoveryAmountBonusesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRecoveryAmountBonusesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRecoveryAmountBonusesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreRecoveryAmountBonusesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreRecoveryAmountBonusesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreRecoveryAmountBonusesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRecoveryAmountBonusesRuntime(ICoreRecoveryAmountBonusesCharacterCaoSongOlOperations characterCaoSongOlOperations, ICoreRecoveryAmountBonusesContentRegistryOperations contentRegistryOperations, ICoreRecoveryAmountBonusesResolutionStackOperations resolutionStackOperations, ICoreRecoveryAmountBonusesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreRecoveryAmountBonusesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreRecoveryAmountBonusesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreRecoveryAmountBonusesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoSongOlOperations = characterCaoSongOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsRuntime : global::CardGame.Core.ITurnHandLimitKindExemptionProgramHost
{
    private readonly ICoreTurnHandLimitKindExemptionsTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreTurnHandLimitKindExemptionsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreTurnHandLimitKindExemptionsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTurnHandLimitKindExemptionsRuntime(ICoreTurnHandLimitKindExemptionsTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreTurnHandLimitKindExemptionsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreTurnHandLimitKindExemptionsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

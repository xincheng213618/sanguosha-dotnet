#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineActorHandLimitPenaltiesRuntime : global::CardGame.Core.IActorHandLimitPenaltyHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries _coreDamageTargetObtainProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignPublicPileSlashesEntries _coreForeignPublicPileSlashesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashReturnsEntries _coreForeignSelectedCardSlashReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineKenshangPaymentsEntries _coreKenshangPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries _coreVirtualOrdinaryTrickUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess _coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess;
    internal CoreGameEngineActorHandLimitPenaltiesRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries coreDamageTargetObtainProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignPublicPileSlashesEntries coreForeignPublicPileSlashesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashReturnsEntries coreForeignSelectedCardSlashReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineKenshangPaymentsEntries coreKenshangPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries coreVirtualOrdinaryTrickUsesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess)
    {
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreChainedStateBasicReturnsOperations = coreChainedStateBasicReturnsOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreDamageTargetObtainProgramsOperations = coreDamageTargetObtainProgramsOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreForeignPublicPileSlashesOperations = coreForeignPublicPileSlashesOperations;
        this._coreForeignSelectedCardSlashReturnsOperations = coreForeignSelectedCardSlashReturnsOperations;
        this._coreKenshangPaymentsOperations = coreKenshangPaymentsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreTieredRoundZeroUsesOperations = coreTieredRoundZeroUsesOperations;
        this._coreVirtualOrdinaryTrickUsesOperations = coreVirtualOrdinaryTrickUsesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess = coreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess;
    }
}

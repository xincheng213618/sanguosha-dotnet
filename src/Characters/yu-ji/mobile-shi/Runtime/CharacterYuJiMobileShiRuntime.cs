#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterYuJiMobileShiRuntime : global::CardGame.Core.IMobileShiYuJiProgramHost
{
    private readonly ICharacterYuJiMobileShiCardZonesOperations _cardZonesOperations;
    private readonly ICharacterYuJiMobileShiContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterYuJiMobileShiResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterYuJiMobileShiCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterYuJiMobileShiCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterYuJiMobileShiCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterYuJiMobileShiCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterYuJiMobileShiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterYuJiMobileShiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterYuJiMobileShiCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICharacterYuJiMobileShiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterYuJiMobileShiCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterYuJiMobileShiCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterYuJiMobileShiCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterYuJiMobileShiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterYuJiMobileShiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterYuJiMobileShiRuntime(ICharacterYuJiMobileShiCardZonesOperations cardZonesOperations, ICharacterYuJiMobileShiContentRegistryOperations contentRegistryOperations, ICharacterYuJiMobileShiResolutionStackOperations resolutionStackOperations, ICharacterYuJiMobileShiCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterYuJiMobileShiCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterYuJiMobileShiCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterYuJiMobileShiCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterYuJiMobileShiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterYuJiMobileShiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterYuJiMobileShiCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICharacterYuJiMobileShiCoreRuntimeOperations coreRuntimeOperations, ICharacterYuJiMobileShiCoreMatchLogOperations coreMatchLogOperations, ICharacterYuJiMobileShiCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterYuJiMobileShiCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterYuJiMobileShiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterYuJiMobileShiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

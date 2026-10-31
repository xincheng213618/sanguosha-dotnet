#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhouYuMobileShiRuntime : global::CardGame.Core.IShiZhouYuFacts, global::CardGame.Core.IShiZhouYuOperations
{
    private readonly ICharacterZhouYuMobileShiCardZonesOperations _cardZonesOperations;
    private readonly ICharacterZhouYuMobileShiContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhouYuMobileShiCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterZhouYuMobileShiCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterZhouYuMobileShiCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterZhouYuMobileShiCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterZhouYuMobileShiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterZhouYuMobileShiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhouYuMobileShiCoreProgramRevealsOperations _coreProgramRevealsOperations;
    private readonly ICharacterZhouYuMobileShiCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterZhouYuMobileShiCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterZhouYuMobileShiCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICharacterZhouYuMobileShiCoreNativeCardClassificationsOperations _coreNativeCardClassificationsOperations;
    private readonly ICharacterZhouYuMobileShiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhouYuMobileShiCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterZhouYuMobileShiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICharacterZhouYuMobileShiShiZhouYuOperations _shiZhouYuOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhouYuMobileShiRuntime(ICharacterZhouYuMobileShiCardZonesOperations cardZonesOperations, ICharacterZhouYuMobileShiContentRegistryOperations contentRegistryOperations, ICharacterZhouYuMobileShiCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterZhouYuMobileShiCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterZhouYuMobileShiCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterZhouYuMobileShiCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterZhouYuMobileShiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterZhouYuMobileShiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhouYuMobileShiCoreProgramRevealsOperations coreProgramRevealsOperations, ICharacterZhouYuMobileShiCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterZhouYuMobileShiCoreMatchLogOperations coreMatchLogOperations, ICharacterZhouYuMobileShiCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICharacterZhouYuMobileShiCoreNativeCardClassificationsOperations coreNativeCardClassificationsOperations, ICharacterZhouYuMobileShiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhouYuMobileShiCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterZhouYuMobileShiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICharacterZhouYuMobileShiShiZhouYuOperations shiZhouYuOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramRevealsOperations = coreProgramRevealsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._shiZhouYuOperations = shiZhouYuOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

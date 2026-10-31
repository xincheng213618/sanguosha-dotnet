#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterYuanShaoBoundaryMobileRuntime : global::CardGame.Core.IMobileYuanShaoProgramHost, global::CardGame.Core.IMobileYuanShaoSelectionQueries
{
    private readonly ICharacterYuanShaoBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterYuanShaoBoundaryMobilePendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICharacterYuanShaoBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterYuanShaoBoundaryMobileSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICharacterYuanShaoBoundaryMobileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterYuanShaoBoundaryMobileCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICharacterYuanShaoBoundaryMobileCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterYuanShaoBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterYuanShaoBoundaryMobileCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICharacterYuanShaoBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterYuanShaoBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterYuanShaoBoundaryMobileRuntime(ICharacterYuanShaoBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterYuanShaoBoundaryMobilePendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICharacterYuanShaoBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterYuanShaoBoundaryMobileSkillRuntimeStateOperations skillRuntimeStateOperations, ICharacterYuanShaoBoundaryMobileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterYuanShaoBoundaryMobileCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICharacterYuanShaoBoundaryMobileCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterYuanShaoBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterYuanShaoBoundaryMobileCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICharacterYuanShaoBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterYuanShaoBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

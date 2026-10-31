#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterXueZongMobileRuntime
{
    private readonly ICharacterXueZongMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterXueZongMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterXueZongMobileCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICharacterXueZongMobileCoreDelayedNullificationOperations _coreDelayedNullificationOperations;
    private readonly ICharacterXueZongMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterXueZongMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterXueZongMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterXueZongMobileCoreResponseEntityExchangeOperations _coreResponseEntityExchangeOperations;
    private readonly ICharacterXueZongMobileCoreResponseUseCompletionOperations _coreResponseUseCompletionOperations;
    private readonly ICharacterXueZongMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterXueZongMobileCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICharacterXueZongMobileCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICharacterXueZongMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterXueZongMobileRuntime(ICharacterXueZongMobileContentRegistryOperations contentRegistryOperations, ICharacterXueZongMobileResolutionStackOperations resolutionStackOperations, ICharacterXueZongMobileCoreCardActionsOperations coreCardActionsOperations, ICharacterXueZongMobileCoreDelayedNullificationOperations coreDelayedNullificationOperations, ICharacterXueZongMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterXueZongMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterXueZongMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterXueZongMobileCoreResponseEntityExchangeOperations coreResponseEntityExchangeOperations, ICharacterXueZongMobileCoreResponseUseCompletionOperations coreResponseUseCompletionOperations, ICharacterXueZongMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterXueZongMobileCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICharacterXueZongMobileCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICharacterXueZongMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreDelayedNullificationOperations = coreDelayedNullificationOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

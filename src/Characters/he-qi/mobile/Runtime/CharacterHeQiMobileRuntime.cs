#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterHeQiMobileRuntime : global::CardGame.Core.IMobileHeQiHost
{
    private readonly ICharacterHeQiMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterHeQiMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterHeQiMobileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterHeQiMobileCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterHeQiMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterHeQiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterHeQiMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterHeQiMobileCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterHeQiMobileCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterHeQiMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterHeQiMobileCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICharacterHeQiMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterHeQiMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterHeQiMobileRuntime(ICharacterHeQiMobileContentRegistryOperations contentRegistryOperations, ICharacterHeQiMobileResolutionStackOperations resolutionStackOperations, ICharacterHeQiMobileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterHeQiMobileCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterHeQiMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterHeQiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterHeQiMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterHeQiMobileCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterHeQiMobileCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterHeQiMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterHeQiMobileCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICharacterHeQiMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterHeQiMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

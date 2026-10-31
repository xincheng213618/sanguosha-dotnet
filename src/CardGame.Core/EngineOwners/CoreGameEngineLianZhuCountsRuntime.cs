#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineLianZhuCountsRuntime : global::CardGame.Core.ILianZhuGiftSelectionQueries
{
    private readonly ICoreLianZhuCountsCharacterDongBaiOlOperations _characterDongBaiOlOperations;
    private readonly ICoreLianZhuCountsCardZonesOperations _cardZonesOperations;
    private readonly ICoreLianZhuCountsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreLianZhuCountsPendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICoreLianZhuCountsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreLianZhuCountsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreLianZhuCountsCoreEphemeralCardMaterialsOperations _coreEphemeralCardMaterialsOperations;
    private readonly ICoreLianZhuCountsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreLianZhuCountsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreLianZhuCountsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreLianZhuCountsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreLianZhuCountsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreGameEngineLianZhuCountsRuntime(ICoreLianZhuCountsCharacterDongBaiOlOperations characterDongBaiOlOperations, ICoreLianZhuCountsCardZonesOperations cardZonesOperations, ICoreLianZhuCountsContentRegistryOperations contentRegistryOperations, ICoreLianZhuCountsPendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICoreLianZhuCountsResolutionStackOperations resolutionStackOperations, ICoreLianZhuCountsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreLianZhuCountsCoreEphemeralCardMaterialsOperations coreEphemeralCardMaterialsOperations, ICoreLianZhuCountsCoreRuntimeOperations coreRuntimeOperations, ICoreLianZhuCountsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreLianZhuCountsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreLianZhuCountsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreLianZhuCountsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._characterDongBaiOlOperations = characterDongBaiOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLuoXianOlRuntime : global::CardGame.Core.ILuoXianProgramHost
{
    private readonly ICharacterLuoXianOlCharacterXiaoQiaoMouOlOperations _characterXiaoQiaoMouOlOperations;
    private readonly ICharacterLuoXianOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterLuoXianOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLuoXianOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLuoXianOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterLuoXianOlCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterLuoXianOlCoreAfterTurnEndedProgramsOperations _coreAfterTurnEndedProgramsOperations;
    private readonly ICharacterLuoXianOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterLuoXianOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICharacterLuoXianOlCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterLuoXianOlCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterLuoXianOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLuoXianOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLuoXianOlCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterLuoXianOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterLuoXianOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterLuoXianOlCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICharacterLuoXianOlCharacterLuoXianOlOperations _characterLuoXianOlOperations;
    private readonly ICharacterLuoXianOlCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations2;
    private readonly ICharacterLuoXianOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLuoXianOlRuntime(ICharacterLuoXianOlCharacterXiaoQiaoMouOlOperations characterXiaoQiaoMouOlOperations, ICharacterLuoXianOlCardZonesOperations cardZonesOperations, ICharacterLuoXianOlContentRegistryOperations contentRegistryOperations, ICharacterLuoXianOlResolutionStackOperations resolutionStackOperations, ICharacterLuoXianOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterLuoXianOlCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterLuoXianOlCoreAfterTurnEndedProgramsOperations coreAfterTurnEndedProgramsOperations, ICharacterLuoXianOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterLuoXianOlCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICharacterLuoXianOlCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterLuoXianOlCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterLuoXianOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLuoXianOlCoreRuntimeOperations coreRuntimeOperations, ICharacterLuoXianOlCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterLuoXianOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterLuoXianOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterLuoXianOlCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICharacterLuoXianOlCharacterLuoXianOlOperations characterLuoXianOlOperations, ICharacterLuoXianOlCoreObserverSubtreesOperations2 coreObserverSubtreesOperations2, ICharacterLuoXianOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._characterLuoXianOlOperations = characterLuoXianOlOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

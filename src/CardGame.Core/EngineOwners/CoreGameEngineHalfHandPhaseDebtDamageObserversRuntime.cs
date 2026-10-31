#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashReturnsEntries _coreForeignSelectedCardSlashReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJoinedTrickDamageObserversEntries _coreJoinedTrickDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastObserversEntries _coreMatchingRecastObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardObserversEntries _coreOffTurnUsedCardObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries _coreOrderedPrintedSkillLossObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries _coreOverflowTargetCancellationObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponseObserversEntries _corePairedColorResponseObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileEquipmentSequencesEntries _corePublicPileEquipmentSequencesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries _coreRecipientCategoryMarkObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedEquipmentObserversEntries _coreRoundGainedEquipmentObserversOperations;
    internal CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashReturnsEntries coreForeignSelectedCardSlashReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineJoinedTrickDamageObserversEntries coreJoinedTrickDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastObserversEntries coreMatchingRecastObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardObserversEntries coreOffTurnUsedCardObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries coreOrderedPrintedSkillLossObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries coreOverflowTargetCancellationObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponseObserversEntries corePairedColorResponseObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileEquipmentSequencesEntries corePublicPileEquipmentSequencesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries coreRecipientCategoryMarkObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedEquipmentObserversEntries coreRoundGainedEquipmentObserversOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreChainedStateBasicReturnsOperations = coreChainedStateBasicReturnsOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreForeignSelectedCardSlashReturnsOperations = coreForeignSelectedCardSlashReturnsOperations;
        this._coreJoinedTrickDamageObserversOperations = coreJoinedTrickDamageObserversOperations;
        this._coreMatchingRecastObserversOperations = coreMatchingRecastObserversOperations;
        this._coreOffTurnUsedCardObserversOperations = coreOffTurnUsedCardObserversOperations;
        this._coreOrderedPrintedSkillLossObserversOperations = coreOrderedPrintedSkillLossObserversOperations;
        this._coreOverflowTargetCancellationObserversOperations = coreOverflowTargetCancellationObserversOperations;
        this._corePairedColorResponseObserversOperations = corePairedColorResponseObserversOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._corePublicPileEquipmentSequencesOperations = corePublicPileEquipmentSequencesOperations;
        this._coreRecipientCategoryMarkObserversOperations = coreRecipientCategoryMarkObserversOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRoundGainedEquipmentObserversOperations = coreRoundGainedEquipmentObserversOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOwnedHandRankSumGiftRuntime
{
    private readonly ICoreOwnedHandRankSumGiftCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICoreOwnedHandRankSumGiftCardZonesOperations _cardZonesOperations;
    private readonly ICoreOwnedHandRankSumGiftContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreOwnedHandRankSumGiftCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreOwnedHandRankSumGiftCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreOwnedHandRankSumGiftCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreOwnedHandRankSumGiftCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreOwnedHandRankSumGiftCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreOwnedHandRankSumGiftKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineOwnedHandRankSumGiftRuntime(ICoreOwnedHandRankSumGiftCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICoreOwnedHandRankSumGiftCardZonesOperations cardZonesOperations, ICoreOwnedHandRankSumGiftContentRegistryOperations contentRegistryOperations, ICoreOwnedHandRankSumGiftCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreOwnedHandRankSumGiftCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreOwnedHandRankSumGiftCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreOwnedHandRankSumGiftCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreOwnedHandRankSumGiftCoreRuntimeOperations coreRuntimeOperations, ICoreOwnedHandRankSumGiftKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

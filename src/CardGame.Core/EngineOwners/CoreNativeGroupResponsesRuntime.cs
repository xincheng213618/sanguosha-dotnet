#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeGroupResponsesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries _characterLiuQiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionZhangbaEntries _coreResponseCompletionZhangbaOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries _coreResponseUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries _coreNativeDodgeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreNativeGroupResponsesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries characterLiuQiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionZhangbaEntries coreResponseCompletionZhangbaOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries coreResponseUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries coreNativeDodgeOperations, global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._characterCaoZhiLeOperations = characterCaoZhiLeOperations;
        this._characterLiuQiOlOperations = characterLiuQiOlOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreResponseCompletionZhangbaOperations = coreResponseCompletionZhangbaOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}

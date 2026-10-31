#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAssistedSlashPaymentRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterKongShuOlEntries _characterKongShuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries _coreOwnPublicPileConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess;
    internal CoreGameEngineAssistedSlashPaymentRuntime(global::CardGame.Core.RuntimeEntries.CharacterKongShuOlEntries characterKongShuOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries coreOwnPublicPileConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess)
    {
        this._characterKongShuOlOperations = characterKongShuOlOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreOwnPublicPileConversionsOperations = coreOwnPublicPileConversionsOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess = coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess;
    }
}

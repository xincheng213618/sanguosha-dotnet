#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEphemeralCardMaterialsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries _characterDengAiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries _characterZhangFenAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries _characterZuoCiHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries _coreCardUsePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineEphemeralCardMaterialsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries characterDengAiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries characterZhangFenAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries characterZuoCiHappyGodOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries coreCardUsePaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterDengAiAnniversaryGodOperations = characterDengAiAnniversaryGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterZhangFenAnniversaryOperations = characterZhangFenAnniversaryOperations;
        this._characterZuoCiHappyGodOperations = characterZuoCiHappyGodOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}

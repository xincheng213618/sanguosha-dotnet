#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramDiscardOriginsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries _characterZhangFenAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineProgramDiscardOriginsRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries characterZhangFenAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterZhangFenAnniversaryOperations = characterZhangFenAnniversaryOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}

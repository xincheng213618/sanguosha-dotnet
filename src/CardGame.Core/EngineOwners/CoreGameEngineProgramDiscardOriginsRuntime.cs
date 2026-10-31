#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramDiscardOriginsRuntime
{
    private readonly ICoreProgramDiscardOriginsCharacterZhangFenAnniversaryOperations _characterZhangFenAnniversaryOperations;
    private readonly ICoreProgramDiscardOriginsCoreShanheStartingResourcesOperations _coreShanheStartingResourcesOperations;
    private readonly ICoreProgramDiscardOriginsCoreWushuangJinzhiOperations _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineProgramDiscardOriginsRuntime(ICoreProgramDiscardOriginsCharacterZhangFenAnniversaryOperations characterZhangFenAnniversaryOperations, ICoreProgramDiscardOriginsCoreShanheStartingResourcesOperations coreShanheStartingResourcesOperations, ICoreProgramDiscardOriginsCoreWushuangJinzhiOperations coreWushuangJinzhiOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterZhangFenAnniversaryOperations = characterZhangFenAnniversaryOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}

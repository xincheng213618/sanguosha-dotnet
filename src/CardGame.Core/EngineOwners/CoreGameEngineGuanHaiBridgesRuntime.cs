#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanHaiBridgesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries _coreTurnRedSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    internal CoreGameEngineGuanHaiBridgesRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries coreTurnRedSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations)
    {
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
    }
}

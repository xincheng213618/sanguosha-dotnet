#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanHaiBridgesRuntime
{
    private readonly ICoreGuanHaiBridgesCharacterGuanHaiOlOperations _characterGuanHaiOlOperations;
    private readonly ICoreGuanHaiBridgesCoreAlternatingSuitAndCompletedTopOperations _coreAlternatingSuitAndCompletedTopOperations;
    private readonly ICoreGuanHaiBridgesCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreGuanHaiBridgesCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreGuanHaiBridgesCoreTurnRedSlashBenefitsOperations _coreTurnRedSlashBenefitsOperations;
    private readonly ICoreGuanHaiBridgesCoreWushuangJinzhiOperations _coreWushuangJinzhiOperations;
    internal CoreGameEngineGuanHaiBridgesRuntime(ICoreGuanHaiBridgesCharacterGuanHaiOlOperations characterGuanHaiOlOperations, ICoreGuanHaiBridgesCoreAlternatingSuitAndCompletedTopOperations coreAlternatingSuitAndCompletedTopOperations, ICoreGuanHaiBridgesCoreCardActionsOperations coreCardActionsOperations, ICoreGuanHaiBridgesCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreGuanHaiBridgesCoreTurnRedSlashBenefitsOperations coreTurnRedSlashBenefitsOperations, ICoreGuanHaiBridgesCoreWushuangJinzhiOperations coreWushuangJinzhiOperations)
    {
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreActiveNativeAttackQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries _characterGuoSiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardAttackHandleLookup _kernelCoreMainOperations;
    internal CoreActiveNativeAttackQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries characterGuoSiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.EngineOwners.IKernelCardAttackHandleLookup kernelCoreMainOperations)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterGuoSiOlOperations = characterGuoSiOlOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardContinuationCursorsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardContinuationHandleFactory _kernelCoreMainOperations;
    internal CoreNativeCardContinuationCursorsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations, global::CardGame.Core.EngineOwners.IKernelCardContinuationHandleFactory kernelCoreMainOperations)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
    }
}

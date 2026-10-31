#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardContinuationCursorsRuntime
{
    private readonly ICoreNativeCardContinuationCursorsCharacterDiaoChanLeOperations _characterDiaoChanLeOperations;
    private readonly ICoreNativeCardContinuationCursorsCharacterLiuChengMouMobileOperations _characterLiuChengMouMobileOperations;
    private readonly ICoreNativeCardContinuationCursorsCharacterXiaoQiaoMouOlOperations _characterXiaoQiaoMouOlOperations;
    private readonly ICoreNativeCardContinuationCursorsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeCardContinuationCursorsCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreNativeCardContinuationCursorsCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICoreNativeCardContinuationCursorsKernelCoreMainOperations _kernelCoreMainOperations;
    internal CoreNativeCardContinuationCursorsRuntime(ICoreNativeCardContinuationCursorsCharacterDiaoChanLeOperations characterDiaoChanLeOperations, ICoreNativeCardContinuationCursorsCharacterLiuChengMouMobileOperations characterLiuChengMouMobileOperations, ICoreNativeCardContinuationCursorsCharacterXiaoQiaoMouOlOperations characterXiaoQiaoMouOlOperations, ICoreNativeCardContinuationCursorsResolutionStackOperations resolutionStackOperations, ICoreNativeCardContinuationCursorsCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreNativeCardContinuationCursorsCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICoreNativeCardContinuationCursorsKernelCoreMainOperations kernelCoreMainOperations)
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

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreActiveNativeAttackQueriesRuntime
{
    private readonly ICoreActiveNativeAttackQueriesCharacterDiaoChanLeOperations _characterDiaoChanLeOperations;
    private readonly ICoreActiveNativeAttackQueriesCharacterGuoSiOlOperations _characterGuoSiOlOperations;
    private readonly ICoreActiveNativeAttackQueriesCharacterLiuChengMouMobileOperations _characterLiuChengMouMobileOperations;
    private readonly ICoreActiveNativeAttackQueriesCharacterXiaoQiaoMouOlOperations _characterXiaoQiaoMouOlOperations;
    private readonly ICoreActiveNativeAttackQueriesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreActiveNativeAttackQueriesCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreActiveNativeAttackQueriesKernelCoreMainOperations _kernelCoreMainOperations;
    internal CoreActiveNativeAttackQueriesRuntime(ICoreActiveNativeAttackQueriesCharacterDiaoChanLeOperations characterDiaoChanLeOperations, ICoreActiveNativeAttackQueriesCharacterGuoSiOlOperations characterGuoSiOlOperations, ICoreActiveNativeAttackQueriesCharacterLiuChengMouMobileOperations characterLiuChengMouMobileOperations, ICoreActiveNativeAttackQueriesCharacterXiaoQiaoMouOlOperations characterXiaoQiaoMouOlOperations, ICoreActiveNativeAttackQueriesResolutionStackOperations resolutionStackOperations, ICoreActiveNativeAttackQueriesCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreActiveNativeAttackQueriesKernelCoreMainOperations kernelCoreMainOperations)
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

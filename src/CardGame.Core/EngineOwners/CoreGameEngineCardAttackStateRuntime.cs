#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardAttackStateRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries _characterGuoSiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries _coreHpLossSlashMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries _coreNextActualUseTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    internal CoreGameEngineCardAttackStateRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries characterGuoSiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries coreHpLossSlashMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries coreNextActualUseTargetAdjustmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterGuoSiOlOperations = characterGuoSiOlOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreHpLossSlashMaterialsOperations = coreHpLossSlashMaterialsOperations;
        this._coreNextActualUseTargetAdjustmentsOperations = coreNextActualUseTargetAdjustmentsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
    }
}

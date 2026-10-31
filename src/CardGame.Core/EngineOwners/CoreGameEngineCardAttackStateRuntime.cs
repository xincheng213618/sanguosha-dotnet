#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardAttackStateRuntime
{
    private readonly ICoreCardAttackStateCharacterDiaoChanLeOperations _characterDiaoChanLeOperations;
    private readonly ICoreCardAttackStateCharacterGuoSiOlOperations _characterGuoSiOlOperations;
    private readonly ICoreCardAttackStateCharacterLiuChengMouMobileOperations _characterLiuChengMouMobileOperations;
    private readonly ICoreCardAttackStateCharacterXiaoQiaoMouOlOperations _characterXiaoQiaoMouOlOperations;
    private readonly ICoreCardAttackStateCardZonesOperations _cardZonesOperations;
    private readonly ICoreCardAttackStateResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCardAttackStateCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreCardAttackStateCoreHpLossSlashMaterialsOperations _coreHpLossSlashMaterialsOperations;
    private readonly ICoreCardAttackStateCoreNextActualUseTargetAdjustmentsOperations _coreNextActualUseTargetAdjustmentsOperations;
    private readonly ICoreCardAttackStateCoreRuntimeOperations _coreRuntimeOperations;
    internal CoreGameEngineCardAttackStateRuntime(ICoreCardAttackStateCharacterDiaoChanLeOperations characterDiaoChanLeOperations, ICoreCardAttackStateCharacterGuoSiOlOperations characterGuoSiOlOperations, ICoreCardAttackStateCharacterLiuChengMouMobileOperations characterLiuChengMouMobileOperations, ICoreCardAttackStateCharacterXiaoQiaoMouOlOperations characterXiaoQiaoMouOlOperations, ICoreCardAttackStateCardZonesOperations cardZonesOperations, ICoreCardAttackStateResolutionStackOperations resolutionStackOperations, ICoreCardAttackStateCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreCardAttackStateCoreHpLossSlashMaterialsOperations coreHpLossSlashMaterialsOperations, ICoreCardAttackStateCoreNextActualUseTargetAdjustmentsOperations coreNextActualUseTargetAdjustmentsOperations, ICoreCardAttackStateCoreRuntimeOperations coreRuntimeOperations)
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

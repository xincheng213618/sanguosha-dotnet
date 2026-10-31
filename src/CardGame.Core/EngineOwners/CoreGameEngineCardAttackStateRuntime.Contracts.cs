#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardAttackStateCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardAttackStateCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreCardAttackStateCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get; }
}

internal interface ICoreCardAttackStateCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreCardAttackStateCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? NextActualUseAdjustedSlashParent(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardAttackStateCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSuspendedGuoSiCompletedParent(global::CardGame.Core.CardUseFrame original);
}

internal interface ICoreCardAttackStateCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? HpLossMaterialSlashParent(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardAttackStateCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
}

internal interface ICoreCardAttackStateResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardAttackStateCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
    bool IsDiouSuspendedRescueAttack(global::CardGame.Core.ResolutionFrame owner);
}

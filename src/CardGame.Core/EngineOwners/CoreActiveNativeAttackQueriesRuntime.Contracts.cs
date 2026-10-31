#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActiveNativeAttackQueriesCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get; }
}

internal interface ICoreActiveNativeAttackQueriesCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSuspendedGuoSiCompletedParent(global::CardGame.Core.CardUseFrame original);
}

internal interface ICoreActiveNativeAttackQueriesCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
}

internal interface ICoreActiveNativeAttackQueriesCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreActiveNativeAttackQueriesKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICoreActiveNativeAttackQueriesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreActiveNativeAttackQueriesCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouSuspendedRescueAttack(global::CardGame.Core.ResolutionFrame owner);
    bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCardContinuationCursorsCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLiuChengSuspendedCounterspell(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreNativeCardContinuationCursorsCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default);
}

internal interface ICoreNativeCardContinuationCursorsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouSuspendedCounterspell(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreNativeCardContinuationCursorsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeCardContinuationCursorsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId);
    global::CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId);
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    global::CardGame.Core.NativeOwners.StoneAxeHandle CreateStoneAxeHandle(long ownerId);
    global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(long ownerId);
    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(long ownerId);
    global::CardGame.Core.NativeOwners.QinglongFollowupHandle CreateQinglongFollowupHandle(long ownerId);
    global::CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(long ownerId);
    global::CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(long ownerId);
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    global::CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(long ownerId);
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
}

internal interface ICoreNativeCardContinuationCursorsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get; }
}

internal interface ICoreNativeCardContinuationCursorsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? FindCardContinuationOwner(global::System.Func<global::CardGame.Core.CardResolutionContinuations, bool> matches);
    void UpdateCardContinuations(long id, global::System.Func<global::CardGame.Core.CardResolutionContinuations, global::CardGame.Core.CardResolutionContinuations> update);
    global::CardGame.Core.CardResolutionContinuations GetCardContinuations(long id);
}

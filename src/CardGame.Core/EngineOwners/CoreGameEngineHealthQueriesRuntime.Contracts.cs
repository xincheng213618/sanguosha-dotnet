#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHealthQueriesCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int NativeExtraEffectRoundOrdinal(long id);
}

internal interface ICoreHealthQueriesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreHealthQueriesCoreCardDamageMaximumHpReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreHealthQueriesCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyQiaoHealth(int seat);
    bool TryBeginHappyQiaoHealthChoice(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int amount, global::CardGame.Core.HappyQiaoHealthReturnKind returnKind, string occurrenceKey, int? sourceSeat = default, long? attackResolutionId = default);
    void RequireHappyQiaoHealthChoice(global::CardGame.Core.HappyQiaoHealthChoiceFrame f);
    bool TryBeginHappyQiaoQuery(global::CardGame.Core.ResolutionFrame supplied, int subjectSeat, string queryKey);
}

internal interface ICoreHealthQueriesKernelCoreGameEngineActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(global::CardGame.Core.NativeOwners.ActionQuery? previous = default);
}

internal interface ICoreHealthQueriesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHealthQueriesCoreLastDamageSourceReciprocityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? LastDamageSourceOriginalAttack(long id);
}

internal interface ICoreHealthQueriesKernelCoreGameEngineProgramExpressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(global::CardGame.Core.ProgramSkillFrame frame);
}

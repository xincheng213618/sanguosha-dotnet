#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualTurnLegacyCompletionCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreActualTurnLegacyCompletionCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCuiYanCapability { get; }
}

internal interface ICoreActualTurnLegacyCompletionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreActualTurnLegacyCompletionCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuYanCapability { get; }
}

internal interface ICoreActualTurnLegacyCompletionCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKongShuCapability { get; }
}

internal interface ICoreActualTurnLegacyCompletionCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeCaiYongFeibaiCapability { get; }
}

internal interface ICoreActualTurnLegacyCompletionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreActualTurnLegacyCompletionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreActualTurnLegacyCompletionCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreActualTurnLegacyCompletionCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksActualTurnUseOrdinal { get; }

    bool IsActualTurnLegacyVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreActualTurnLegacyCompletionCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICoreActualTurnLegacyCompletionCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouHuangYueyingBingcaiCapability { get; }
}

internal interface ICoreActualTurnLegacyCompletionCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreActualTurnLegacyCompletionCoreActualHandGainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNormalizedLegacyCompletedCategoryUse(global::CardGame.Core.CardUseFrame use, int actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> completedTargets);
}

internal interface ICoreActualTurnLegacyCompletionCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreActualTurnLegacyCompletionCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeCaoZhiWenlanCapability { get; }
}

internal interface ICoreActualTurnLegacyCompletionContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

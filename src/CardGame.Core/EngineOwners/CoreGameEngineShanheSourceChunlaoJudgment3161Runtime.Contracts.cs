#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSourceChunlaoJudgment3161CoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreShanheSourceChunlaoJudgment3161KernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheSourceChunlaoJudgment3161CoreActualPhaseEndsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualPhaseEnd(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, global::CardGame.Core.ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default);
}

internal interface ICoreShanheSourceChunlaoJudgment3161CoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheSourceChunlaoJudgment3161CoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreShanheSourceChunlaoJudgment3161CoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteTurnStart(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool skippedActualJudgment = false);
}

internal interface ICoreShanheSourceChunlaoJudgment3161ResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheSourceChunlaoJudgment3161CoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor);
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
}

internal interface ICoreShanheSourceChunlaoJudgment3161CorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShanheSourceChunlaoJudgment3161CoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeFrame(long frameId);
}

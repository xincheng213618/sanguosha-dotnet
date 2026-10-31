#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDeferredTurnEndPreludesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDeferredTurnEndPreludesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreDeferredTurnEndPreludesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDeferredTurnEndPreludesCorePrivateTurnHoldsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActualTurnEndMovementPrelude { get; }
}

internal interface ICoreDeferredTurnEndPreludesCardsMovedWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardsMovedWindowRuntime Get();
}

internal interface ICoreDeferredTurnEndPreludesCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDeferredTurnEnd();
}

internal interface ICoreDeferredTurnEndPreludesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
}

internal interface ICoreDeferredTurnEndPreludesCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateCardsMovedProgramContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreDeferredTurnEndPreludesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDeferredTurnEndPreludesPendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

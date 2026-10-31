#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCorePrivateTurnHoldsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCorePrivateTurnHoldsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> _targetFactory = targetFactory;
    public bool HasActualTurnEndMovementPrelude { get => this._targetFactory().HasActualTurnEndMovementPrelude; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardsMovedWindowRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCardsMovedWindowAdapter(global::System.Func<global::CardGame.Core.CardsMovedWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCardsMovedWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardsMovedWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardsMovedWindowRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public void ContinueDeferredTurnEnd() => this._targetFactory().ContinueDeferredTurnEnd();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredTurnEndPreludesCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardsMovedProgramContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateCardsMovedProgramContext(frame, candidate);
}

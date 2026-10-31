#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreJudgmentOutcomeClaimsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreJudgmentOutcomeClaimsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime))]
internal sealed class CoreJudgmentOutcomeClaimsCoreJudgmentOutcomeDamageEdgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCoreJudgmentOutcomeDamageEdgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> _targetFactory = targetFactory;
    public bool JudgmentOutcomeDamageEdge(int index, global::CardGame.Core.ProgramSkillFrame root) => this._targetFactory().JudgmentOutcomeDamageEdge(index, root);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreJudgmentOutcomeClaimsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame) => this._targetFactory().IsValidProgramJudgmentContinuation(pending, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreJudgmentOutcomeClaimsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId) => this._targetFactory().GetJudgmentFrame(frameId);
    public void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().ReplaceJudgmentFrame(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime))]
internal sealed class CoreJudgmentOutcomeClaimsCoreProgramJudgmentOutcomesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentOutcomeClaimsCoreProgramJudgmentOutcomesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? JudgmentOutcomeInstruction(global::CardGame.Core.ProgramSkillFrame frame, int instructionIndex) => this._targetFactory().JudgmentOutcomeInstruction(frame, instructionIndex);
}

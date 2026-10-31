#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiTaiShiCiOperations(EngineStates.CharacterTaiShiCiMobileShiState characterTaiShiCiMobileShiState, Func<RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries> getCoreGameEngineProgramCardTargetEffectsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramTurnPhaseEntries> getCoreGameEngineProgramTurnPhaseRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IShiTaiShiCiOperations
{
    public void SetLieScanSequence(int sequence) => characterTaiShiCiMobileShiState.ReferenceToShiTzLieScanSequence = sequence;
    public void MarkEffectIneffective(long frameId, int target) => getCoreGameEngineProgramCardTargetEffectsRuntime().MarkCardEffectIneffective(frameId, target);
    public void PublishState() => advanceRulesAndPublishState();
    public void LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(frame, actor, ids, reason);
    public SkillProgramStepOutcome BeginVirtualCardUse(ProgramSkillFrame frame, int target, CardKind kind, bool ignoreDistance) => getCoreGameEngineProgramTurnPhaseRuntime().BeginProgramVirtualCardUse(frame, target, kind, ignoreDistance);
    public void DrawCards(long frameId, int recipient, int amount, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(frameId, recipient, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
    public void Recover(long frameId, int owner, int target, int amount) => createProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    public bool DrainChildWindows(long frameId) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
    public void Finish(ProgramSkillFrame frame, bool completed) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlShenZhenJiOperations(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime) : IOlShenZhenJiOperations
{
    public void PublishState() => advanceRulesAndPublishState();
    public void LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(frame, actor, ids, reason);
    public void BeginDamage(ProgramSkillFrame frame, int target, int amount, DamageNature nature) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(frame, target, amount, nature: nature);
    public bool DrainChildWindows(long frameId) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
}

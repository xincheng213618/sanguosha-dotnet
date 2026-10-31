#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhouYuOperations(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramRevealsEntries> getCoreGameEngineProgramRevealsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IShiZhouYuOperations
{
    public void PublishState() => advanceRulesAndPublishState();
    public bool DrainChildWindows(long frameId) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
    public void Finish(ProgramSkillFrame frame, bool completed) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed);
    public void MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, destination, reason, disclosure: disclosure);
    public void LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(frame, actor, ids, reason);
    public void RevealHand(long frameId, int owner, string name, IReadOnlyList<int> ids) => getCoreGameEngineProgramRevealsRuntime().CommitProgramHandReveal(frameId, owner, name, ids);
    public void Damage(ProgramSkillFrame frame, int target, int amount, DamageNature nature) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(frame, target, amount, nature: nature);
    public void DrawCards(long frameId, int recipient, int amount, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(frameId, recipient, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
    public void SetChained(ProgramSkillFrame frame, bool chained, int target) => getCoreGameEngineProgramLifecycleRuntime().SetProgramChainedState(frame, chained, target);
    public void Log(string category, string message, int actor, int target) => getCoreMatchLogRuntime().AddLog(category, message, actor, target);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestContinuations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IForeignTurnContestContinuations
{
    public bool BeginRecoveryReplacement(long frameId) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement);
    public bool BeginHpChanged(long frameId) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    public void GrantSelfOnly(ProgramSkillFrame frame, int subjectSeat) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardTargetRestriction(frame, SkillProgramCardTargetRestriction.SelfOnly, subjectSeat);
    public void Finish(ProgramSkillFrame frame, bool completed) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed);
}

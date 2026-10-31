#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestContinuations(Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IForeignTurnContestContinuations
{
    public bool BeginRecoveryReplacement(long frameId) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement);
    public bool BeginHpChanged(long frameId) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    public void GrantSelfOnly(ProgramSkillFrame frame, int subjectSeat) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardTargetRestriction(frame, SkillProgramCardTargetRestriction.SelfOnly, subjectSeat);
    public void Finish(ProgramSkillFrame frame, bool completed) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardActionContestProgramCapability(Func<RuntimeEntries.CoreGameEngineCardActionContestsEntries> getCoreGameEngineCardActionContestsRuntime) : ICardActionContestProgramHost
{
    public void ReplaceAllSlashTargets(ProgramSkillFrame f, string bind) => getCoreGameEngineCardActionContestsRuntime().ReplaceAllProgramSlashTargets(f, bind);
    public SkillProgramStepOutcome StartCardActionPindian(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineCardActionContestsRuntime().StartProgramCardActionPindian(f, e);
}

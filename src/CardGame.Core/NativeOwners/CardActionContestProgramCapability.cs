#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardActionContestProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCardActionContestsRuntime>> getCoreGameEngineCardActionContestsRuntime) : ICardActionContestProgramHost
{
    public void ReplaceAllSlashTargets(ProgramSkillFrame f, string bind) => getCoreGameEngineCardActionContestsRuntime().Value.ReplaceAllProgramSlashTargets(f, bind);
    public SkillProgramStepOutcome StartCardActionPindian(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineCardActionContestsRuntime().Value.StartProgramCardActionPindian(f, e);
}

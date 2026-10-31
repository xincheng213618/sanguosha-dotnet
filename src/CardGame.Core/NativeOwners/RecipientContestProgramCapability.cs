#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class RecipientContestProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime>> getCoreGameEngineUniqueHpPeerProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineRecipientContestsRuntime>> getCoreGameEngineRecipientContestsRuntime) : IRecipientContestProgramHost
{
    public SkillProgramStepOutcome DiscardDrawPeer(ProgramSkillFrame f) => getCoreGameEngineUniqueHpPeerProgramsRuntime().Value.BeginUniqueHpPeer(f);
    public SkillProgramStepOutcome GiveAllHandContest(ProgramSkillFrame f, int seat, string bind) => getCoreGameEngineRecipientContestsRuntime().Value.BeginRecipientContest(f, seat, bind);
    public SkillProgramStepOutcome UseContestWinnerSlash(ProgramSkillFrame f, string bind) => getCoreGameEngineRecipientContestsRuntime().Value.UseRecipientContestWinner(f, bind);
}

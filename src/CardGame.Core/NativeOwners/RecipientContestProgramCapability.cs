#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class RecipientContestProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineUniqueHpPeerProgramsEntries> getCoreGameEngineUniqueHpPeerProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientContestsEntries> getCoreGameEngineRecipientContestsRuntime) : IRecipientContestProgramHost
{
    public SkillProgramStepOutcome DiscardDrawPeer(ProgramSkillFrame f) => getCoreGameEngineUniqueHpPeerProgramsRuntime().BeginUniqueHpPeer(f);
    public SkillProgramStepOutcome GiveAllHandContest(ProgramSkillFrame f, int seat, string bind) => getCoreGameEngineRecipientContestsRuntime().BeginRecipientContest(f, seat, bind);
    public SkillProgramStepOutcome UseContestWinnerSlash(ProgramSkillFrame f, string bind) => getCoreGameEngineRecipientContestsRuntime().UseRecipientContestWinner(f, bind);
}

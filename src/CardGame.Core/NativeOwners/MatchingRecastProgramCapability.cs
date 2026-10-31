#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MatchingRecastProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries> getCoreGameEngineMatchingRecastsRuntime) : IMatchingRecastProgramHost
{
    public SkillProgramStepOutcome RecastMatchingCardsThenPeerChoice(ProgramSkillFrame f, int peer, string ledger) => getCoreGameEngineMatchingRecastsRuntime().BeginMatchingRecast(f, peer, ledger);
}

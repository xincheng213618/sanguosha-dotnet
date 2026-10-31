#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MatchingRecastProgramCapability(Func<RuntimeEntries.CoreGameEngineMatchingRecastsEntries> getCoreGameEngineMatchingRecastsRuntime) : IMatchingRecastProgramHost
{
    public SkillProgramStepOutcome RecastMatchingCardsThenPeerChoice(ProgramSkillFrame f, int peer, string ledger) => getCoreGameEngineMatchingRecastsRuntime().BeginMatchingRecast(f, peer, ledger);
}

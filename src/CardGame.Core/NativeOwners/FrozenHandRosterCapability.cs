#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FrozenHandRosterCapability(Func<RuntimeEntries.CoreGameEngineFrozenHandRosterEntries> getCoreGameEngineFrozenHandRosterRuntime) : IFrozenHandRosterHost
{
    public SkillProgramStepOutcome ResolveFrozenHandRosterEndingExchange(ProgramSkillFrame f) => getCoreGameEngineFrozenHandRosterRuntime().ResolveFrozenHandRosterEndingExchange(f);
}

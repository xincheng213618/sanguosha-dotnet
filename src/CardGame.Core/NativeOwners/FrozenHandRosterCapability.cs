#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class FrozenHandRosterCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFrozenHandRosterEntries> getCoreGameEngineFrozenHandRosterRuntime) : IFrozenHandRosterHost
{
    public SkillProgramStepOutcome ResolveFrozenHandRosterEndingExchange(ProgramSkillFrame f) => getCoreGameEngineFrozenHandRosterRuntime().ResolveFrozenHandRosterEndingExchange(f);
}

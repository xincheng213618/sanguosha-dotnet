#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FrozenHandRosterCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFrozenHandRosterRuntime>> getCoreGameEngineFrozenHandRosterRuntime) : IFrozenHandRosterHost
{
    public SkillProgramStepOutcome ResolveFrozenHandRosterEndingExchange(ProgramSkillFrame f) => getCoreGameEngineFrozenHandRosterRuntime().Value.ResolveFrozenHandRosterEndingExchange(f);
}

#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PrivateWhirlCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePrivateWhirlRuntime>> getCoreGameEnginePrivateWhirlRuntime) : IPrivateWhirlHost
{
    public SkillProgramStepOutcome BeginPrivateWhirl(ProgramSkillFrame f, SkillProgramEffect effect) => getCoreGameEnginePrivateWhirlRuntime().Value.BeginPrivateWhirl(f, effect);
}

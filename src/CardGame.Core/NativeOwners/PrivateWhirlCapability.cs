#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PrivateWhirlCapability(Func<RuntimeEntries.CoreGameEnginePrivateWhirlEntries> getCoreGameEnginePrivateWhirlRuntime) : IPrivateWhirlHost
{
    public SkillProgramStepOutcome BeginPrivateWhirl(ProgramSkillFrame f, SkillProgramEffect effect) => getCoreGameEnginePrivateWhirlRuntime().BeginPrivateWhirl(f, effect);
}

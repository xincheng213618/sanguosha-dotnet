#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PrivateWhirlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateWhirlEntries> getCoreGameEnginePrivateWhirlRuntime) : IPrivateWhirlHost
{
    public SkillProgramStepOutcome BeginPrivateWhirl(ProgramSkillFrame f, SkillProgramEffect effect) => getCoreGameEnginePrivateWhirlRuntime().BeginPrivateWhirl(f, effect);
}

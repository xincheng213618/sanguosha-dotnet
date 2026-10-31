#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiUseCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiUseHost
{
    public SkillProgramStepOutcome Sample(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().BeginWuZhangFeiUse(frame);
}

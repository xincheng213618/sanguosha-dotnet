#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiUseCapability(Func<RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiUseHost
{
    public SkillProgramStepOutcome Sample(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().BeginWuZhangFeiUse(frame);
}

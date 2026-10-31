#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiHaoxianCapability(Func<RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiHaoxianHost
{
    public SkillProgramStepOutcome ResolveHaoxian(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().BeginWuHaoxian(frame);
}

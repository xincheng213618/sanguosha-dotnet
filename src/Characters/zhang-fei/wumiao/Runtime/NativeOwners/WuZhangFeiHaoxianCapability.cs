#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiHaoxianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiHaoxianHost
{
    public SkillProgramStepOutcome ResolveHaoxian(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().BeginWuHaoxian(frame);
}

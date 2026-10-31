#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuKangDixianCapability(Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : IWuLuKangDixianHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterLuKangWumiaoRuntime().BeginWuLuKangDixian(f);
}

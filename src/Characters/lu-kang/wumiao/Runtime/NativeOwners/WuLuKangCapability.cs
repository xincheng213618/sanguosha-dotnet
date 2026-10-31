#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuKangCapability(Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : IWuLuKangHost
{
    public SkillProgramStepOutcome Shenduan(ProgramSkillFrame f) => getCharacterLuKangWumiaoRuntime().BeginWuLuKangShenduan(f);
    public SkillProgramStepOutcome Kegou(ProgramSkillFrame f) => getCharacterLuKangWumiaoRuntime().BeginWuLuKangKegou(f);
}

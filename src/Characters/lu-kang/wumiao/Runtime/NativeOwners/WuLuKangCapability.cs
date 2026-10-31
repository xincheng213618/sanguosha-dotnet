#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuKangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : IWuLuKangHost
{
    public SkillProgramStepOutcome Shenduan(ProgramSkillFrame f) => getCharacterLuKangWumiaoRuntime().BeginWuLuKangShenduan(f);
    public SkillProgramStepOutcome Kegou(ProgramSkillFrame f) => getCharacterLuKangWumiaoRuntime().BeginWuLuKangKegou(f);
}

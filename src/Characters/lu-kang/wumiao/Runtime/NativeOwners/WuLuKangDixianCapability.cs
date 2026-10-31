#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuKangDixianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : IWuLuKangDixianHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterLuKangWumiaoRuntime().BeginWuLuKangDixian(f);
}

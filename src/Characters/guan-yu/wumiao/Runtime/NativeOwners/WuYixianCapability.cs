#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuYixianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime) : IWuYixianHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterGuanYuWumiaoRuntime().BeginWuYixian(frame);
}

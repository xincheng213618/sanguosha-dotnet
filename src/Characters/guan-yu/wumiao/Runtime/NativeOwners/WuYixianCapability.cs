#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuYixianCapability(Func<RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime) : IWuYixianHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterGuanYuWumiaoRuntime().BeginWuYixian(frame);
}

#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuGuanYuWuyouCapability(Func<RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime) : IWuGuanYuWuyouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterGuanYuWumiaoRuntime().BeginWuyou(frame);
}

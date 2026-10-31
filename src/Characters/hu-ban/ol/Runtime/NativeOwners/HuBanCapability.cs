#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HuBanCapability(Func<RuntimeEntries.CharacterHuBanOlEntries> getCharacterHuBanOlRuntime) : IHuBanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterHuBanOlRuntime().BeginHuiyun(f);
}

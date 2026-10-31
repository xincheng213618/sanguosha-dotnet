#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileSpXuShuJianyanHost(Func<RuntimeEntries.CharacterXuShuMobileEntries> getCharacterXuShuMobileRuntime) : IMobileSpXuShuJianyanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterXuShuMobileRuntime().BeginMobileSpXuShuJianyan(f);
}

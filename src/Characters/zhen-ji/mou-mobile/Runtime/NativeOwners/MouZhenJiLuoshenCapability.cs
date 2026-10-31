#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhenJiLuoshenCapability(Func<RuntimeEntries.CharacterZhenJiMouMobileEntries> getCharacterZhenJiMouMobileRuntime) : IMouZhenJiLuoshenHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f) => getCharacterZhenJiMouMobileRuntime().StartMouZhenJiLuoshen(f);
}

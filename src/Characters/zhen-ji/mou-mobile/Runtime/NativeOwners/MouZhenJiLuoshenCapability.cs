#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhenJiLuoshenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhenJiMouMobileEntries> getCharacterZhenJiMouMobileRuntime) : IMouZhenJiLuoshenHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f) => getCharacterZhenJiMouMobileRuntime().StartMouZhenJiLuoshen(f);
}

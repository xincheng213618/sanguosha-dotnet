#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HuBanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuBanOlEntries> getCharacterHuBanOlRuntime) : IHuBanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterHuBanOlRuntime().BeginHuiyun(f);
}

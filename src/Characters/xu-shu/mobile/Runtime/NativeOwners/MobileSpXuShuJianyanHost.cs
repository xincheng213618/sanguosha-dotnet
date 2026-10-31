#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileSpXuShuJianyanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuShuMobileEntries> getCharacterXuShuMobileRuntime) : IMobileSpXuShuJianyanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterXuShuMobileRuntime().BeginMobileSpXuShuJianyan(f);
}

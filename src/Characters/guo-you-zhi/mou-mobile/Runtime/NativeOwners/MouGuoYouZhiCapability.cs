#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoYouZhiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoYouZhiMouMobileEntries> getCharacterGuoYouZhiMouMobileRuntime) : IMouGuoYouZhiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoYouZhiMouMobileRuntime().BeginMouGyz(f, op);
}

#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileJwHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMobileGodEntries> getCharacterJiangWeiMobileGodRuntime) : IMobileJiangWeiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiMobileGodRuntime().BeginMobileJw(f, op);
}

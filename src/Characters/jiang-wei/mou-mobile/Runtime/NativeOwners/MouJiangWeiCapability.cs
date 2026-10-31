#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouJiangWeiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMouMobileEntries> getCharacterJiangWeiMouMobileRuntime) : IMouJiangWeiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiMouMobileRuntime().BeginMouJw(f, op);
}

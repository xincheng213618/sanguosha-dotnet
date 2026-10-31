#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileJiKangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiKangMobileEntries> getCharacterJiKangMobileRuntime) : IMobileJiKangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiKangMobileRuntime().BeginMobileJiKang(f, op);
}

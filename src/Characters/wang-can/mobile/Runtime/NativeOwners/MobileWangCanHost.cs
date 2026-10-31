#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileWangCanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangCanMobileEntries> getCharacterWangCanMobileRuntime) : IMobileWangCanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterWangCanMobileRuntime().BeginMobileWangCanXi(frame, op);
}

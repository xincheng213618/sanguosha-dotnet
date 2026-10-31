#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouXiahouDunMobileCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouDunMouMobileEntries> getCharacterXiahouDunMouMobileRuntime) : IMouXiahouDunMobileHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouDunMouMobileRuntime().StartMouXiahouDunMobile(f, op);
}

#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouXiahouDunMobileCapability(Func<RuntimeEntries.CharacterXiahouDunMouMobileEntries> getCharacterXiahouDunMouMobileRuntime) : IMouXiahouDunMobileHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouDunMouMobileRuntime().StartMouXiahouDunMobile(f, op);
}

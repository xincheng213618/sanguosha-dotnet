#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouDiaoChanMobileCapability(Func<RuntimeEntries.CharacterDiaoChanMouMobileEntries> getCharacterDiaoChanMouMobileRuntime) : IMouDiaoChanMobileHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDiaoChanMouMobileRuntime().StartMouDiaoChanMobile(f, op);
}

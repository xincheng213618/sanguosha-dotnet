#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouFaZhengCapability(Func<RuntimeEntries.CharacterFaZhengMouMobileEntries> getCharacterFaZhengMouMobileRuntime) : IMouFaZhengHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFaZhengMouMobileRuntime().StartMouFaZheng(f, op);
}

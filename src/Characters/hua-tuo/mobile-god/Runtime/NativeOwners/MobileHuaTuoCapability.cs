#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileHuaTuoCapability(Func<RuntimeEntries.CharacterHuaTuoMobileGodEntries> getCharacterHuaTuoMobileGodRuntime) : IMobileHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterHuaTuoMobileGodRuntime().BeginMobileHt(frame, op);
}

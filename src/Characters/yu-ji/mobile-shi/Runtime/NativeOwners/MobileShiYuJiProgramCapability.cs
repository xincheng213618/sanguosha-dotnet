#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiYuJiProgramCapability(Func<RuntimeEntries.CharacterYuJiMobileShiEntries> getCharacterYuJiMobileShiRuntime) : IMobileShiYuJiProgramHost
{
    public SkillProgramStepOutcome ResolveFujiGift(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().ResolveMobileShiFujiGiftProgram(frame);
    public SkillProgramStepOutcome ResolveFujiUseEffects(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().ResolveMobileShiFujiUseEffectsProgram(frame);
    public SkillProgramStepOutcome ResolveFujiDodgeDraw(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().ResolveMobileShiFujiDodgeDrawProgram(frame);
}

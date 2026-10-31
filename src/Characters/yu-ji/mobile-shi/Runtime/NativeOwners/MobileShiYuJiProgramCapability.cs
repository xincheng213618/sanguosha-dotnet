#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiYuJiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuJiMobileShiEntries> getCharacterYuJiMobileShiRuntime) : IMobileShiYuJiProgramHost
{
    public SkillProgramStepOutcome ResolveFujiGift(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().ResolveMobileShiFujiGiftProgram(frame);
    public SkillProgramStepOutcome ResolveFujiUseEffects(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().ResolveMobileShiFujiUseEffectsProgram(frame);
    public SkillProgramStepOutcome ResolveFujiDodgeDraw(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().ResolveMobileShiFujiDodgeDrawProgram(frame);
}

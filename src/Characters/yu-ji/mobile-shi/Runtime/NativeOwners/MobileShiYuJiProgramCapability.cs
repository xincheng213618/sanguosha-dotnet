#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiYuJiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuJiMobileShiRuntime>> getCharacterYuJiMobileShiRuntime) : IMobileShiYuJiProgramHost
{
    public SkillProgramStepOutcome ResolveFujiGift(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().Value.ResolveMobileShiFujiGiftProgram(frame);
    public SkillProgramStepOutcome ResolveFujiUseEffects(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().Value.ResolveMobileShiFujiUseEffectsProgram(frame);
    public SkillProgramStepOutcome ResolveFujiDodgeDraw(ProgramSkillFrame frame) => getCharacterYuJiMobileShiRuntime().Value.ResolveMobileShiFujiDodgeDrawProgram(frame);
}

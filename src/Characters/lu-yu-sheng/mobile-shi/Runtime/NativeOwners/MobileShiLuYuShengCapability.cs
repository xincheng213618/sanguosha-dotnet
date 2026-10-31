#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiLuYuShengCapability(Func<RuntimeEntries.CharacterLuYuShengMobileShiEntries> getCharacterLuYuShengMobileShiRuntime) : IMobileShiLuYuShengProgramHost
{
    public SkillProgramStepOutcome ResolveRunweiGrant(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().BeginMobileShiRunweiGrant(frame);
    public SkillProgramStepOutcome ResolveRunweiLossDebt(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().ResolveMobileShiRunweiLossDebt(frame);
    public SkillProgramStepOutcome ResolveRunweiPhaseEnd(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().ResolveMobileShiRunweiPhaseEnd(frame);
    public SkillProgramStepOutcome ResolveShuanghuaiGuard(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().BeginMobileShiShuanghuaiGuard(frame);
}

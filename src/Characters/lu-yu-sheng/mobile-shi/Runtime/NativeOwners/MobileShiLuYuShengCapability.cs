#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiLuYuShengCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuYuShengMobileShiRuntime>> getCharacterLuYuShengMobileShiRuntime) : IMobileShiLuYuShengProgramHost
{
    public SkillProgramStepOutcome ResolveRunweiGrant(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().Value.BeginMobileShiRunweiGrant(frame);
    public SkillProgramStepOutcome ResolveRunweiLossDebt(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().Value.ResolveMobileShiRunweiLossDebt(frame);
    public SkillProgramStepOutcome ResolveRunweiPhaseEnd(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().Value.ResolveMobileShiRunweiPhaseEnd(frame);
    public SkillProgramStepOutcome ResolveShuanghuaiGuard(ProgramSkillFrame frame) => getCharacterLuYuShengMobileShiRuntime().Value.BeginMobileShiShuanghuaiGuard(frame);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FireTargetBenefitProgramCapability(Func<RuntimeEntries.CoreGameEngineFireTargetBenefitsEntries> getCoreGameEngineFireTargetBenefitsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime>> getCoreGameEngineNamedCardAcquisitionsRuntime) : IFireTargetBenefitProgramHost
{
    public SkillProgramStepOutcome DrawFireTargetAndGrantTurnUseQuota(ProgramSkillFrame f) => getCoreGameEngineFireTargetBenefitsRuntime().BeginFireTargetBenefit(f);
    public SkillProgramStepOutcome LoseSkillsAndObtainNamedCard(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineNamedCardAcquisitionsRuntime().Value.BeginNamedCardAcquisition(f, e);
}

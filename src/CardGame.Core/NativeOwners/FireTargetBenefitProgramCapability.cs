#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FireTargetBenefitProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFireTargetBenefitsEntries> getCoreGameEngineFireTargetBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedCardAcquisitionsEntries> getCoreGameEngineNamedCardAcquisitionsRuntime) : IFireTargetBenefitProgramHost
{
    public SkillProgramStepOutcome DrawFireTargetAndGrantTurnUseQuota(ProgramSkillFrame f) => getCoreGameEngineFireTargetBenefitsRuntime().BeginFireTargetBenefit(f);
    public SkillProgramStepOutcome LoseSkillsAndObtainNamedCard(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineNamedCardAcquisitionsRuntime().BeginNamedCardAcquisition(f, e);
}

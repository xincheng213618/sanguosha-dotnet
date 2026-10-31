#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SlashTargetBenefitProgramCapability(Func<RuntimeEntries.CoreGameEngineSlashTargetBenefitPaymentsEntries> getCoreGameEngineSlashTargetBenefitPaymentsRuntime) : ISlashTargetBenefitProgramHost
{
    public SkillProgramStepOutcome OfferSlashTargetBenefit(ProgramSkillFrame f, string binding) => getCoreGameEngineSlashTargetBenefitPaymentsRuntime().OfferSlashTargetBenefit(f, binding);
    public SkillProgramStepOutcome SettleDodgeCancelledSlashBenefit(ProgramSkillFrame f) => getCoreGameEngineSlashTargetBenefitPaymentsRuntime().SettleDodgeCancelledSlashBenefit(f);
}

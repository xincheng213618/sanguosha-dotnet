#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SlashTargetBenefitProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime>> getCoreGameEngineSlashTargetBenefitPaymentsRuntime) : ISlashTargetBenefitProgramHost
{
    public SkillProgramStepOutcome OfferSlashTargetBenefit(ProgramSkillFrame f, string binding) => getCoreGameEngineSlashTargetBenefitPaymentsRuntime().Value.OfferSlashTargetBenefit(f, binding);
    public SkillProgramStepOutcome SettleDodgeCancelledSlashBenefit(ProgramSkillFrame f) => getCoreGameEngineSlashTargetBenefitPaymentsRuntime().Value.SettleDodgeCancelledSlashBenefit(f);
}

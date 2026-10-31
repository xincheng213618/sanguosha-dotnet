#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FirstSlashDuelBenefitCapability(Func<RuntimeEntries.CoreGameEngineFirstSlashDuelBenefitsEntries> getCoreGameEngineFirstSlashDuelBenefitsRuntime) : IFirstSlashDuelBenefitHost
{
    public SkillProgramStepOutcome ChooseFirstSlashDuelDiamondBenefit(ProgramSkillFrame frame) => getCoreGameEngineFirstSlashDuelBenefitsRuntime().BeginFirstSlashDuelBenefit(frame);
}

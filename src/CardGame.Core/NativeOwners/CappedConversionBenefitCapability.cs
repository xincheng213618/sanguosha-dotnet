#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CappedConversionBenefitCapability(Func<RuntimeEntries.CoreGameEngineCappedConversionBenefitsEntries> getCoreGameEngineCappedConversionBenefitsRuntime) : ICappedConversionBenefitHost
{
    public SkillProgramStepOutcome DrawBeforeCappedConversionTierUpgrade(ProgramSkillFrame f, string state) => getCoreGameEngineCappedConversionBenefitsRuntime().BeginCappedConversionBenefit(f, state);
}

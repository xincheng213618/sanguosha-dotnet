#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CappedConversionBenefitCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCappedConversionBenefitsEntries> getCoreGameEngineCappedConversionBenefitsRuntime) : ICappedConversionBenefitHost
{
    public SkillProgramStepOutcome DrawBeforeCappedConversionTierUpgrade(ProgramSkillFrame f, string state) => getCoreGameEngineCappedConversionBenefitsRuntime().BeginCappedConversionBenefit(f, state);
}

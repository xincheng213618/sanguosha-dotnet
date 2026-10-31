#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CappedConversionBenefitCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime>> getCoreGameEngineCappedConversionBenefitsRuntime) : ICappedConversionBenefitHost
{
    public SkillProgramStepOutcome DrawBeforeCappedConversionTierUpgrade(ProgramSkillFrame f, string state) => getCoreGameEngineCappedConversionBenefitsRuntime().Value.BeginCappedConversionBenefit(f, state);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlternatingBenefitsProgramCapability(Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime) : IAlternatingBenefitsProgramHost
{
    public void ApplyAlternatingChoiceBenefit(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineAlternatingBenefitsRuntime().ApplyAlternatingChoiceBenefit(f, e);
    public SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame f, int seat, SkillProgramEffect e) => getCoreGameEngineAlternatingBenefitsRuntime().ObtainDeckCardWithConsecutiveTarget(f, seat, e);
}

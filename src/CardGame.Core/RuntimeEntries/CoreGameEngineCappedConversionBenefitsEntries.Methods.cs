#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedConversionBenefitsEntries
{
    internal SkillProgramStepOutcome BeginCappedConversionBenefit(ProgramSkillFrame supplied, string state) => _resolve().BeginCappedConversionBenefit(supplied, state);
    internal void ContinueCappedConversionBenefit(long id) => _resolve().ContinueCappedConversionBenefit(id);
    internal bool ExactCappedConversionParent(ProgramSkillFrame f, scoped out DamageTriggerWindowFrame window) => _resolve().ExactCappedConversionParent(f, out window);
    internal bool ResumeCappedConversionBenefit(long id) => _resolve().ResumeCappedConversionBenefit(id);
    internal bool ReturnCappedConversionBenefitMovement(ProgramSkillFrame frame) => _resolve().ReturnCappedConversionBenefitMovement(frame);
    internal bool ValidCappedConversionBenefit(ProgramSkillFrame f) => _resolve().ValidCappedConversionBenefit(f);
}

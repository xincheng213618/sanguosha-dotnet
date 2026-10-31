#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedConversionBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCappedConversionBenefit(global::CardGame.Core.ProgramSkillFrame supplied, string state) => _resolve().BeginCappedConversionBenefit(supplied, state);
    internal void ContinueCappedConversionBenefit(long id) => _resolve().ContinueCappedConversionBenefit(id);
    internal bool ExactCappedConversionParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.DamageTriggerWindowFrame window) => _resolve().ExactCappedConversionParent(f, out window);
    internal bool ResumeCappedConversionBenefit(long id) => _resolve().ResumeCappedConversionBenefit(id);
    internal bool ReturnCappedConversionBenefitMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnCappedConversionBenefitMovement(frame);
    internal bool ValidCappedConversionBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCappedConversionBenefit(f);
}

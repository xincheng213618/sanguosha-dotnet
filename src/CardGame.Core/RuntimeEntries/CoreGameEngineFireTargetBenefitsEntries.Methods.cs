#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFireTargetBenefitsEntries
{
    internal SkillProgramStepOutcome BeginFireTargetBenefit(ProgramSkillFrame supplied) => _resolve().BeginFireTargetBenefit(supplied);
    internal bool CanRunFireTargetBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunFireTargetBenefit(candidate, trigger, context);
    internal bool ResumeFireTargetBenefit(long id) => _resolve().ResumeFireTargetBenefit(id);
    internal bool ReturnFireTargetBenefitMovement(ProgramSkillFrame f) => _resolve().ReturnFireTargetBenefitMovement(f);
    internal bool TryDrainFireTargetMovement(ProgramSkillFrame f) => _resolve().TryDrainFireTargetMovement(f);
}

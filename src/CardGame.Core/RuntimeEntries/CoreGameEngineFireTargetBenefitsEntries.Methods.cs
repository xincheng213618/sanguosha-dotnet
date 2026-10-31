#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFireTargetBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFireTargetBenefit(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginFireTargetBenefit(supplied);
    internal bool CanRunFireTargetBenefit(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunFireTargetBenefit(candidate, trigger, context);
    internal bool IsFireTargetBenefitMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsFireTargetBenefitMovement(f, effect, pending);
    internal bool ResumeFireTargetBenefit(long id) => _resolve().ResumeFireTargetBenefit(id);
    internal bool ReturnFireTargetBenefitMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFireTargetBenefitMovement(f);
    internal bool TryDrainFireTargetMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TryDrainFireTargetMovement(f);
}

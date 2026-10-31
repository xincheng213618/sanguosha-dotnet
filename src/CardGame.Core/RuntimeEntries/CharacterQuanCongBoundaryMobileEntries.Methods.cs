#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQuanCongBoundaryMobileEntries
{
    internal bool AllowsBoundaryYaoming(ProgramActivationLegalityContext gate) => _resolve().AllowsBoundaryYaoming(gate);
    internal void AnswerBoundaryYaoming(PromptChoice answer) => _resolve().AnswerBoundaryYaoming(answer);
    internal SkillProgramStepOutcome BeginBoundaryYaoming(ProgramSkillFrame supplied) => _resolve().BeginBoundaryYaoming(supplied);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> BoundaryQuanCongPublicStates(CharacterState owner, string skillId) => _resolve().BoundaryQuanCongPublicStates(owner, skillId);
    internal PromptChoice BoundaryYaomingAi(PendingDecision pending, ProgramSkillFrame frame) => _resolve().BoundaryYaomingAi(pending, frame);
    internal PromptChoice BoundaryYaomingChoice(ProgramSkillFrame frame, string option, int target, string label) => _resolve().BoundaryYaomingChoice(frame, option, target, label);
    internal IReadOnlyList<PromptChoice> BoundaryYaomingChoices(ProgramSkillFrame frame) => _resolve().BoundaryYaomingChoices(frame);
    internal (int Remaining, string? Pending) BoundaryYaomingState(int owner, string instance) => _resolve().BoundaryYaomingState(owner, instance);
    internal void CompleteBoundaryYaoming(ProgramSkillFrame frame) => _resolve().CompleteBoundaryYaoming(frame);
    internal bool ContinueBoundaryYaoming(long id) => _resolve().ContinueBoundaryYaoming(id);
    internal bool KeepPaidBoundaryYaoming(ProgramSkillFrame frame) => _resolve().KeepPaidBoundaryYaoming(frame);
    internal bool OriginalBoundaryYaoming(ProgramSkillFrame frame) => _resolve().OriginalBoundaryYaoming(frame);
    internal void PublishBoundaryYaoming(ProgramSkillFrame frame) => _resolve().PublishBoundaryYaoming(frame);
    internal void ReturnBoundaryYaomingDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnBoundaryYaomingDraw(supplied, outcome);
    internal bool ReturnBoundaryYaomingMovement(ProgramSkillFrame frame) => _resolve().ReturnBoundaryYaomingMovement(frame);
    internal bool ValidBoundaryYaoming(ProgramSkillFrame frame) => _resolve().ValidBoundaryYaoming(frame);
}

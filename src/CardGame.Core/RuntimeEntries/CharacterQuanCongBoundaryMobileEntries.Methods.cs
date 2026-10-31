#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQuanCongBoundaryMobileEntries
{
    internal bool AllowsBoundaryYaoming(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsBoundaryYaoming(gate);
    internal void AnswerBoundaryYaoming(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerBoundaryYaoming(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryYaoming(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryYaoming(supplied);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryQuanCongPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().BoundaryQuanCongPublicStates(owner, skillId);
    internal global::CardGame.Core.PromptChoice BoundaryYaomingAi(global::CardGame.Core.PendingDecision pending, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryYaomingAi(pending, frame);
    internal global::CardGame.Core.PromptChoice BoundaryYaomingChoice(global::CardGame.Core.ProgramSkillFrame frame, string option, int target, string label) => _resolve().BoundaryYaomingChoice(frame, option, target, label);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryYaomingChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryYaomingChoices(frame);
    internal (int Remaining, string? Pending) BoundaryYaomingState(int owner, string instance) => _resolve().BoundaryYaomingState(owner, instance);
    internal void CompleteBoundaryYaoming(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteBoundaryYaoming(frame);
    internal bool ContinueBoundaryYaoming(long id) => _resolve().ContinueBoundaryYaoming(id);
    internal bool IsBoundaryYaomingPaidMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paidEffect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsBoundaryYaomingPaidMovement(frame, paidEffect, pending);
    internal bool KeepPaidBoundaryYaoming(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepPaidBoundaryYaoming(frame);
    internal bool OriginalBoundaryYaoming(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OriginalBoundaryYaoming(frame);
    internal void PublishBoundaryYaoming(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryYaoming(frame);
    internal void ReturnBoundaryYaomingDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundaryYaomingDraw(supplied, outcome);
    internal bool ReturnBoundaryYaomingMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryYaomingMovement(frame);
    internal bool ValidBoundaryYaoming(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryYaoming(frame);
}

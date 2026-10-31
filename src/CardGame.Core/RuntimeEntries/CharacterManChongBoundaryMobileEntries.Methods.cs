#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterManChongBoundaryMobileEntries
{
    internal void AnswerBoundaryManChongJunxing(PromptChoice choice) => _resolve().AnswerBoundaryManChongJunxing(choice);
    internal SkillProgramStepOutcome BeginBoundaryManChongJunxing(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginBoundaryManChongJunxing(supplied, effect);
    internal PromptChoice BoundaryManChongAi(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().BoundaryManChongAi(prompt, frame);
    internal Card[] BoundaryManChongDiscardable(int target) => _resolve().BoundaryManChongDiscardable(target);
    internal bool BoundaryManChongPaidMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().BoundaryManChongPaidMovement(frame, effect, pending);
    internal void PublishBoundaryManChongJunxing(ProgramSkillFrame frame) => _resolve().PublishBoundaryManChongJunxing(frame);
    internal bool ReturnBoundaryManChongJunxing(ProgramSkillFrame frame) => _resolve().ReturnBoundaryManChongJunxing(frame);
}

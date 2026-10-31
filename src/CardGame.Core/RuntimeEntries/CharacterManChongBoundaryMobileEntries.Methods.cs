#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterManChongBoundaryMobileEntries
{
    internal void AnswerBoundaryManChongJunxing(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryManChongJunxing(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryManChongJunxing(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginBoundaryManChongJunxing(supplied, effect);
    internal global::CardGame.Core.PromptChoice BoundaryManChongAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryManChongAi(prompt, frame);
    internal global::CardGame.Core.Card[] BoundaryManChongDiscardable(int target) => _resolve().BoundaryManChongDiscardable(target);
    internal bool BoundaryManChongPaidMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().BoundaryManChongPaidMovement(frame, effect, pending);
    internal void PublishBoundaryManChongJunxing(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryManChongJunxing(frame);
    internal bool ReturnBoundaryManChongJunxing(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryManChongJunxing(frame);
}

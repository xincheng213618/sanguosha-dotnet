#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunXiuBoundaryMobileEntries
{
    internal void AnswerBoundaryXingxue(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerBoundaryXingxue(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryXingxue(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryXingxue(supplied);
    internal void BeginBoundaryXingxueDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginBoundaryXingxueDraw(frame);
    internal global::CardGame.Core.PromptChoice BoundaryXingxueAi(global::CardGame.Core.PendingDecision pending, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryXingxueAi(pending, frame);
    internal global::CardGame.Core.PromptChoice BoundaryXingxueChoice(global::CardGame.Core.ProgramSkillFrame frame, string branch, string key, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().BoundaryXingxueChoice(frame, branch, key, text, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryXingxueChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryXingxueChoices(frame);
    internal int BoundaryXingxueChooser(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryXingxueChooser(frame);
    internal bool BoundaryXingxuePaidMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().BoundaryXingxuePaidMovement(frame, effect, pending);
    internal void CompleteBoundaryXingxue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteBoundaryXingxue(frame);
    internal bool ContinueBoundaryXingxue(long id) => _resolve().ContinueBoundaryXingxue(id);
    internal bool IsBoundarySunXiuFactionRangeTarget(int actorSeat, int targetSeat) => _resolve().IsBoundarySunXiuFactionRangeTarget(actorSeat, targetSeat);
    internal bool KeepIssuedBoundaryXingxue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepIssuedBoundaryXingxue(frame);
    internal void NextBoundaryXingxue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NextBoundaryXingxue(frame);
    internal void PublishBoundaryXingxue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryXingxue(frame);
    internal void ReturnBoundaryXingxueDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundaryXingxueDraw(supplied, outcome);
    internal bool ReturnBoundaryXingxueMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryXingxueMovement(frame);
    internal bool ValidBoundaryXingxue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryXingxue(frame);
}

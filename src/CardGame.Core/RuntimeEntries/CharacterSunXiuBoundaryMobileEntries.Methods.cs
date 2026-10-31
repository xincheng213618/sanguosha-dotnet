#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunXiuBoundaryMobileEntries
{
    internal void AnswerBoundaryXingxue(PromptChoice answer) => _resolve().AnswerBoundaryXingxue(answer);
    internal SkillProgramStepOutcome BeginBoundaryXingxue(ProgramSkillFrame supplied) => _resolve().BeginBoundaryXingxue(supplied);
    internal void BeginBoundaryXingxueDraw(ProgramSkillFrame frame) => _resolve().BeginBoundaryXingxueDraw(frame);
    internal PromptChoice BoundaryXingxueAi(PendingDecision pending, ProgramSkillFrame frame) => _resolve().BoundaryXingxueAi(pending, frame);
    internal PromptChoice BoundaryXingxueChoice(ProgramSkillFrame frame, string branch, string key, string text, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().BoundaryXingxueChoice(frame, branch, key, text, cards, targets);
    internal IReadOnlyList<PromptChoice> BoundaryXingxueChoices(ProgramSkillFrame frame) => _resolve().BoundaryXingxueChoices(frame);
    internal int BoundaryXingxueChooser(ProgramSkillFrame frame) => _resolve().BoundaryXingxueChooser(frame);
    internal void CompleteBoundaryXingxue(ProgramSkillFrame frame) => _resolve().CompleteBoundaryXingxue(frame);
    internal bool ContinueBoundaryXingxue(long id) => _resolve().ContinueBoundaryXingxue(id);
    internal bool IsBoundarySunXiuFactionRangeTarget(int actorSeat, int targetSeat) => _resolve().IsBoundarySunXiuFactionRangeTarget(actorSeat, targetSeat);
    internal bool KeepIssuedBoundaryXingxue(ProgramSkillFrame frame) => _resolve().KeepIssuedBoundaryXingxue(frame);
    internal void NextBoundaryXingxue(ProgramSkillFrame frame) => _resolve().NextBoundaryXingxue(frame);
    internal void PublishBoundaryXingxue(ProgramSkillFrame frame) => _resolve().PublishBoundaryXingxue(frame);
    internal void ReturnBoundaryXingxueDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnBoundaryXingxueDraw(supplied, outcome);
    internal bool ReturnBoundaryXingxueMovement(ProgramSkillFrame frame) => _resolve().ReturnBoundaryXingxueMovement(frame);
    internal bool ValidBoundaryXingxue(ProgramSkillFrame frame) => _resolve().ValidBoundaryXingxue(frame);
}

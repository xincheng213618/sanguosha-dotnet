#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiRuBoundaryMobileEntries
{
    internal PromptChoice AiBoundaryLiRuMieji(PendingDecision p, ProgramSkillFrame f) => _resolve().AiBoundaryLiRuMieji(p, f);
    internal void AnswerBoundaryLiRuMieji(PromptChoice choice) => _resolve().AnswerBoundaryLiRuMieji(choice);
    internal IReadOnlyList<int> BoundaryLiRuLostCardTargets(int owner) => _resolve().BoundaryLiRuLostCardTargets(owner);
    internal IEnumerable<(Card Card, CardLocation From)> BoundaryLiRuMiejiCards(int target, bool gift) => _resolve().BoundaryLiRuMiejiCards(target, gift);
    internal CardConversionSource BoundaryLiRuSource(ProgramSkillFrame f) => _resolve().BoundaryLiRuSource(f);
    internal bool KeepPaidBoundaryLiRuMieji(ProgramSkillFrame f) => _resolve().KeepPaidBoundaryLiRuMieji(f);
    internal void PayBoundaryLiRuMieji(ProgramSkillFrame f) => _resolve().PayBoundaryLiRuMieji(f);
    internal SkillProgramStepOutcome PublishBoundaryLiRuMieji(ProgramSkillFrame f) => _resolve().PublishBoundaryLiRuMieji(f);
    internal SkillProgramStepOutcome ResolveBoundaryLiRuMieji(ProgramSkillFrame supplied) => _resolve().ResolveBoundaryLiRuMieji(supplied);
    internal bool ReturnBoundaryLiRuMieji(ProgramSkillFrame f) => _resolve().ReturnBoundaryLiRuMieji(f);
    internal bool ValidBoundaryLiRuMieji(ProgramSkillFrame f) => _resolve().ValidBoundaryLiRuMieji(f);
}

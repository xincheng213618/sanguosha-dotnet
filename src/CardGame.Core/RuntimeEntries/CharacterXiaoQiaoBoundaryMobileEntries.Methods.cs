#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoBoundaryMobileEntries
{
    internal void AnswerBoundaryXiaoQiaoJudgment(PromptChoice choice) => _resolve().AnswerBoundaryXiaoQiaoJudgment(choice);
    internal SkillProgramStepOutcome BeginBoundaryXiaoQiaoJudgment(ProgramSkillFrame f) => _resolve().BeginBoundaryXiaoQiaoJudgment(f);
    internal Suit? BoundaryXiaoQiaoEffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation) => _resolve().BoundaryXiaoQiaoEffectiveSuit(owner, card, identityLocation);
    internal Suit EffectiveBoundaryXiaoQiaoJudgmentSuit(JudgmentFrame frame, Suit fallback) => _resolve().EffectiveBoundaryXiaoQiaoJudgmentSuit(frame, fallback);
    internal bool IsExactBoundaryXiaoQiaoBeforeResultProgramChild(ProgramJudgmentTriggerWindowFrame w, ProgramSkillFrame f) => _resolve().IsExactBoundaryXiaoQiaoBeforeResultProgramChild(w, f);
    internal bool QualifyBoundaryXiaoQiaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c) => _resolve().QualifyBoundaryXiaoQiaoMobile(b, c);
}

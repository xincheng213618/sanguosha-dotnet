#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoBoundaryMobileEntries
{
    internal void AnswerBoundaryXiaoQiaoJudgment(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryXiaoQiaoJudgment(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryXiaoQiaoJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundaryXiaoQiaoJudgment(f);
    internal global::CardGame.Core.Suit? BoundaryXiaoQiaoEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation) => _resolve().BoundaryXiaoQiaoEffectiveSuit(owner, card, identityLocation);
    internal global::CardGame.Core.Suit EffectiveBoundaryXiaoQiaoJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback) => _resolve().EffectiveBoundaryXiaoQiaoJudgmentSuit(frame, fallback);
    internal bool IsExactBoundaryXiaoQiaoBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame w, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsExactBoundaryXiaoQiaoBeforeResultProgramChild(w, f);
    internal bool QualifyBoundaryXiaoQiaoMobile(global::CardGame.Core.ProgramTriggerCandidate b, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().QualifyBoundaryXiaoQiaoMobile(b, c);
}

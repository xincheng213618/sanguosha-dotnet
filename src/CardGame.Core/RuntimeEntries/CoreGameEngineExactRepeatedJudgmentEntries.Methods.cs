#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExactRepeatedJudgmentEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramExactRepeatedJudgment(global::CardGame.Core.ProgramSkillFrame frame, string reason, string bind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> suits, global::CardGame.Core.SkillProgramClaimHandLimitExemption policy) => _resolve().BeginProgramExactRepeatedJudgment(frame, reason, bind, suits, policy);
    internal void CaptureProgramRepeatedJudgmentClaimOutcome(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit) => _resolve().CaptureProgramRepeatedJudgmentClaimOutcome(judgment, card, suit);
    internal void CleanupExactRepeatedJudgment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CleanupExactRepeatedJudgment(frame);
    internal bool ExactRepeatedJudgmentSourceLive(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramRepeatedJudgment state) => _resolve().ExactRepeatedJudgmentSourceLive(frame, state);
    internal void ResumeProgramExactRepeatedJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramRepeatedJudgment state) => _resolve().ResumeProgramExactRepeatedJudgment(judgment, frame, state);
}

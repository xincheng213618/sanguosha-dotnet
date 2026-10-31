#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExactRepeatedJudgmentEntries
{
    internal SkillProgramStepOutcome BeginProgramExactRepeatedJudgment(ProgramSkillFrame frame, string reason, string bind, IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy) => _resolve().BeginProgramExactRepeatedJudgment(frame, reason, bind, suits, policy);
    internal void CaptureProgramRepeatedJudgmentClaimOutcome(JudgmentFrame judgment, Card card, Suit suit) => _resolve().CaptureProgramRepeatedJudgmentClaimOutcome(judgment, card, suit);
    internal void CleanupExactRepeatedJudgment(ProgramSkillFrame frame) => _resolve().CleanupExactRepeatedJudgment(frame);
    internal bool ExactRepeatedJudgmentSourceLive(ProgramSkillFrame frame, ProgramRepeatedJudgment state) => _resolve().ExactRepeatedJudgmentSourceLive(frame, state);
    internal void ResumeProgramExactRepeatedJudgment(JudgmentFrame judgment, ProgramSkillFrame frame, ProgramRepeatedJudgment state) => _resolve().ResumeProgramExactRepeatedJudgment(judgment, frame, state);
}

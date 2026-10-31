#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhenyiJudgmentWindowsEntries
{
    internal void CompleteZhenyiBeforeResultWindow(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame pending) => _resolve().CompleteZhenyiBeforeResultWindow(frame, pending);
    internal SkillProgramTriggerWindow CurrentZhenyiJudgmentWindowKind(long judgmentId) => _resolve().CurrentZhenyiJudgmentWindowKind(judgmentId);
    internal JudgmentFrame GetJudgmentFrameOrCompletedZhenyi(JudgmentFrame fallback) => _resolve().GetJudgmentFrameOrCompletedZhenyi(fallback);
    internal bool IsExactZhenyiBeforeResultProgramChild(ProgramJudgmentTriggerWindowFrame window, ProgramSkillFrame child) => _resolve().IsExactZhenyiBeforeResultProgramChild(window, child);
    internal ProgramJudgmentTriggerWindowFrame RefreshZhenyiBeforeResultWindow(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame pending) => _resolve().RefreshZhenyiBeforeResultWindow(frame, pending);
    internal bool TryBeginZhenyiBeforeResultWindow(JudgmentFrame pending, Card card, Suit suit) => _resolve().TryBeginZhenyiBeforeResultWindow(pending, card, suit);
    internal bool ZhenyiJudgmentSucceeded(JudgmentFrame frame, Suit suit, int rank) => _resolve().ZhenyiJudgmentSucceeded(frame, suit, rank);
}

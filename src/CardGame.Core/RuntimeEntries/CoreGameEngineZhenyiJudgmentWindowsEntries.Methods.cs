#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhenyiJudgmentWindowsEntries
{
    internal void CompleteZhenyiBeforeResultWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending) => _resolve().CompleteZhenyiBeforeResultWindow(frame, pending);
    internal global::CardGame.Core.SkillProgramTriggerWindow CurrentZhenyiJudgmentWindowKind(long judgmentId) => _resolve().CurrentZhenyiJudgmentWindowKind(judgmentId);
    internal global::CardGame.Core.JudgmentFrame GetJudgmentFrameOrCompletedZhenyi(global::CardGame.Core.JudgmentFrame fallback) => _resolve().GetJudgmentFrameOrCompletedZhenyi(fallback);
    internal bool IsExactZhenyiBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child) => _resolve().IsExactZhenyiBeforeResultProgramChild(window, child);
    internal global::CardGame.Core.ProgramJudgmentTriggerWindowFrame RefreshZhenyiBeforeResultWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending) => _resolve().RefreshZhenyiBeforeResultWindow(frame, pending);
    internal bool TryBeginZhenyiBeforeResultWindow(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit) => _resolve().TryBeginZhenyiBeforeResultWindow(pending, card, suit);
    internal bool ZhenyiJudgmentSucceeded(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit suit, int rank) => _resolve().ZhenyiJudgmentSucceeded(frame, suit, rank);
}

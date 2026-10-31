#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedHandRefreshProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCappedHandRefresh(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int cap) => _resolve().BeginCappedHandRefresh(frame, targetSeat, cap);
    internal void BeginCappedHandRefreshMovement(long frameId) => _resolve().BeginCappedHandRefreshMovement(frameId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CappedHandRefreshChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CappedHandRefreshChoices(frame);
    internal bool CappedHandRefreshSourceCurrent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CappedHandRefreshSourceCurrent(frame);
    internal void FinishCappedHandRefresh(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().FinishCappedHandRefresh(frame, completed);
    internal void PublishCappedHandRefreshDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishCappedHandRefreshDiscard(frame);
    internal void ResolveCappedHandRefreshChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveCappedHandRefreshChoice(selected);
    internal bool ResumeCappedHandRefresh(long frameId) => _resolve().ResumeCappedHandRefresh(frameId);
    internal bool ReturnCappedHandRefreshMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnCappedHandRefreshMovement(frame);
}

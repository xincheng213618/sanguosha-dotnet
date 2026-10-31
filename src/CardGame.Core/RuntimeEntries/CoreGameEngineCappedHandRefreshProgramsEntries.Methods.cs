#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedHandRefreshProgramsEntries
{
    internal SkillProgramStepOutcome BeginCappedHandRefresh(ProgramSkillFrame frame, int targetSeat, int cap) => _resolve().BeginCappedHandRefresh(frame, targetSeat, cap);
    internal void BeginCappedHandRefreshMovement(long frameId) => _resolve().BeginCappedHandRefreshMovement(frameId);
    internal IReadOnlyList<PromptChoice> CappedHandRefreshChoices(ProgramSkillFrame frame) => _resolve().CappedHandRefreshChoices(frame);
    internal bool CappedHandRefreshSourceCurrent(ProgramSkillFrame frame) => _resolve().CappedHandRefreshSourceCurrent(frame);
    internal void FinishCappedHandRefresh(ProgramSkillFrame frame, bool completed) => _resolve().FinishCappedHandRefresh(frame, completed);
    internal void PublishCappedHandRefreshDiscard(ProgramSkillFrame frame) => _resolve().PublishCappedHandRefreshDiscard(frame);
    internal void ResolveCappedHandRefreshChoice(PromptChoice selected) => _resolve().ResolveCappedHandRefreshChoice(selected);
    internal bool ResumeCappedHandRefresh(long frameId) => _resolve().ResumeCappedHandRefresh(frameId);
    internal bool ReturnCappedHandRefreshMovement(ProgramSkillFrame frame) => _resolve().ReturnCappedHandRefreshMovement(frame);
}

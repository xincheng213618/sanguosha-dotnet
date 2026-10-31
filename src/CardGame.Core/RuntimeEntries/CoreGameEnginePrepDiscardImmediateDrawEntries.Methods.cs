#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardImmediateDrawEntries
{
    internal bool CanApplyPrepDiscardImmediateDraw(ProgramSkillFrame frame) => _resolve().CanApplyPrepDiscardImmediateDraw(frame);
    internal bool ContinuePaidImmediatePrepDiscard(ProgramSkillFrame frame) => _resolve().ContinuePaidImmediatePrepDiscard(frame);
    internal SkillProgramStepOutcome ContinuePrepDiscardImmediateDraw(ProgramSkillFrame supplied) => _resolve().ContinuePrepDiscardImmediateDraw(supplied);
    internal void IssuePrepDiscardImmediateDraw(ProgramSkillFrame supplied) => _resolve().IssuePrepDiscardImmediateDraw(supplied);
    internal void ReturnPrepDiscardImmediateDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnPrepDiscardImmediateDraw(supplied, outcome);
    internal bool ValidPrepDiscardImmediateDraw(ProgramSkillFrame frame) => _resolve().ValidPrepDiscardImmediateDraw(frame);
}

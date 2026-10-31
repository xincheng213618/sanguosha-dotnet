#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardImmediateDrawEntries
{
    internal bool CanApplyPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanApplyPrepDiscardImmediateDraw(frame);
    internal bool ContinuePaidImmediatePrepDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidImmediatePrepDiscard(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ContinuePrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ContinuePrepDiscardImmediateDraw(supplied);
    internal void IssuePrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssuePrepDiscardImmediateDraw(supplied);
    internal void ReturnPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnPrepDiscardImmediateDraw(supplied, outcome);
    internal bool ValidPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPrepDiscardImmediateDraw(frame);
}

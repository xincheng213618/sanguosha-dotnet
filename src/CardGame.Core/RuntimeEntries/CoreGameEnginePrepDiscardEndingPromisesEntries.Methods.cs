#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardEndingPromisesEntries
{
    internal void AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendPrepDiscardEndingItems(items);
    internal void ConsumePrepDiscardEnding(PrepDiscardEndingPromise p, bool applied) => _resolve().ConsumePrepDiscardEnding(p, applied);
    internal void ConsumePrepDiscardEndingAtBegin(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().ConsumePrepDiscardEndingAtBegin(c, context);
    internal void ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame) => _resolve().ConsumeSkippedPrepDiscardEnding(frame);
    internal SkillProgramStepOutcome DrawPrepDiscardEnding(ProgramSkillFrame supplied) => _resolve().DrawPrepDiscardEnding(supplied);
    internal void ExpirePrepDiscardEndingPromises() => _resolve().ExpirePrepDiscardEndingPromises();
    internal void IssuePrepDiscardEnding(ProgramSkillFrame supplied, string endingBinding) => _resolve().IssuePrepDiscardEnding(supplied, endingBinding);
    internal bool MatchesPrepDiscardEnding(ProgramSkillFrame f) => _resolve().MatchesPrepDiscardEnding(f);
    internal bool MatchesPrepDiscardEnding2(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool begun) => _resolve().MatchesPrepDiscardEnding(c, context, begun);
    internal bool PrepDiscardEndingConsumed(long id) => _resolve().PrepDiscardEndingConsumed(id);
    internal bool ValidPrepDiscardEndingDraw(ProgramSkillFrame f) => _resolve().ValidPrepDiscardEndingDraw(f);
    internal bool ValidPrepDiscardEndingPromise(PrepDiscardEndingPromise p) => _resolve().ValidPrepDiscardEndingPromise(p);
}

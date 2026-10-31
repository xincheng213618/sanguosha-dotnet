#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardEndingPromisesEntries
{
    internal void AppendPrepDiscardEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendPrepDiscardEndingItems(items);
    internal void ConsumePrepDiscardEnding(global::CardGame.Core.PrepDiscardEndingPromise p, bool applied) => _resolve().ConsumePrepDiscardEnding(p, applied);
    internal void ConsumePrepDiscardEndingAtBegin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ConsumePrepDiscardEndingAtBegin(c, context);
    internal void ConsumeSkippedPrepDiscardEnding(global::CardGame.Core.TurnEndingBoundaryFrame frame) => _resolve().ConsumeSkippedPrepDiscardEnding(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawPrepDiscardEnding(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().DrawPrepDiscardEnding(supplied);
    internal void ExpirePrepDiscardEndingPromises() => _resolve().ExpirePrepDiscardEndingPromises();
    internal void IssuePrepDiscardEnding(global::CardGame.Core.ProgramSkillFrame supplied, string endingBinding) => _resolve().IssuePrepDiscardEnding(supplied, endingBinding);
    internal bool MatchesPrepDiscardEnding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchesPrepDiscardEnding(f);
    internal bool MatchesPrepDiscardEnding2(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, bool begun) => _resolve().MatchesPrepDiscardEnding(c, context, begun);
    internal bool PrepDiscardEndingConsumed(long id) => _resolve().PrepDiscardEndingConsumed(id);
    internal bool ValidPrepDiscardEndingDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPrepDiscardEndingDraw(f);
    internal bool ValidPrepDiscardEndingPromise(global::CardGame.Core.PrepDiscardEndingPromise p) => _resolve().ValidPrepDiscardEndingPromise(p);
}

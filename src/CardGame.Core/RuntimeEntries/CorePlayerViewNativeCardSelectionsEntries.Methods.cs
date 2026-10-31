#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePlayerViewNativeCardSelectionsEntries
{
    internal global::CardGame.Core.PendingDecision NativeOwnedSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, string stage, string verbKey, global::System.Collections.Generic.IReadOnlyList<string> pickValues, global::System.Collections.Generic.IReadOnlyList<string> finishValues, global::System.Collections.Generic.IReadOnlyList<int> selected, int minimum, int maximum, global::System.Collections.Generic.IReadOnlyList<string>? resetValues = default, global::System.Collections.Generic.IReadOnlyList<int>? allowedCounts = default) => _resolve().NativeOwnedSelection(decision, frame, stage, verbKey, pickValues, finishValues, selected, minimum, maximum, resetValues, allowedCounts);
    internal global::CardGame.Core.PendingDecision ProjectNativeCardSelections(global::CardGame.Core.PendingDecision decision) => _resolve().ProjectNativeCardSelections(decision);
}

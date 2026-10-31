#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePlayerViewNativeCardSelectionsEntries
{
    internal PendingDecision NativeOwnedSelection(PendingDecision decision, ProgramSkillFrame frame, string stage, string verbKey, IReadOnlyList<string> pickValues, IReadOnlyList<string> finishValues, IReadOnlyList<int> selected, int minimum, int maximum, IReadOnlyList<string>? resetValues = default, IReadOnlyList<int>? allowedCounts = default) => _resolve().NativeOwnedSelection(decision, frame, stage, verbKey, pickValues, finishValues, selected, minimum, maximum, resetValues, allowedCounts);
    internal PendingDecision ProjectNativeCardSelections(PendingDecision decision) => _resolve().ProjectNativeCardSelections(decision);
}

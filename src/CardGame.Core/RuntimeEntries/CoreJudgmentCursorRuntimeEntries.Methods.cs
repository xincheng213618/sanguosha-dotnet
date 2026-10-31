#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreJudgmentCursorRuntimeEntries
{
    internal JudgmentFrame? ActiveJudgment { get => _resolve().ActiveJudgment; }
}

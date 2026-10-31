#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed partial class CoreJudgmentCursorRuntimeEntries
{
    private readonly Func<EngineOwners.CoreJudgmentCursorRuntimeRuntime> _resolve;
    private CoreJudgmentCursorRuntimeEntries(Func<EngineOwners.CoreJudgmentCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreJudgmentCursorRuntimeEntries Create(Func<EngineOwners.CoreJudgmentCursorRuntimeRuntime> resolve) => new(resolve);
}

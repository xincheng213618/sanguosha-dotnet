#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed partial class CoreJudgmentCursorRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _resolve;
    private CoreJudgmentCursorRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreJudgmentCursorRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> resolve) => new(resolve);
}

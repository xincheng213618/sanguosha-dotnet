#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime))]
internal sealed partial class CoreGameEngineResponseCompletionHealthEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> _resolve;
    private CoreGameEngineResponseCompletionHealthEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseCompletionHealthEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> resolve) => new(resolve);
}

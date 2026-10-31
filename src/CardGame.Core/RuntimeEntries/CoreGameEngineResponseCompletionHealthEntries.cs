#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineResponseCompletionHealthRuntime))]
internal sealed partial class CoreGameEngineResponseCompletionHealthEntries
{
    private readonly Func<EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> _resolve;
    private CoreGameEngineResponseCompletionHealthEntries(Func<EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseCompletionHealthEntries Create(Func<EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> resolve) => new(resolve);
}

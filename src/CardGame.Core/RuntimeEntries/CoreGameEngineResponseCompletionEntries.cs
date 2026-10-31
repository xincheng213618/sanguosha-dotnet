#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed partial class CoreGameEngineResponseCompletionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineResponseCompletionRuntime> _resolve;
    private CoreGameEngineResponseCompletionEntries(Func<EngineOwners.CoreGameEngineResponseCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseCompletionEntries Create(Func<EngineOwners.CoreGameEngineResponseCompletionRuntime> resolve) => new(resolve);
}

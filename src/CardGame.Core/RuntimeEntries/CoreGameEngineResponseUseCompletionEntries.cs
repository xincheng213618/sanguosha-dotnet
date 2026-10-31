#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed partial class CoreGameEngineResponseUseCompletionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _resolve;
    private CoreGameEngineResponseUseCompletionEntries(Func<EngineOwners.CoreGameEngineResponseUseCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseUseCompletionEntries Create(Func<EngineOwners.CoreGameEngineResponseUseCompletionRuntime> resolve) => new(resolve);
}

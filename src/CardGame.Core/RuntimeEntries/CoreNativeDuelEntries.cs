#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDuelRuntime))]
internal sealed partial class CoreNativeDuelEntries
{
    private readonly Func<EngineOwners.CoreNativeDuelRuntime> _resolve;
    private CoreNativeDuelEntries(Func<EngineOwners.CoreNativeDuelRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDuelEntries Create(Func<EngineOwners.CoreNativeDuelRuntime> resolve) => new(resolve);
}

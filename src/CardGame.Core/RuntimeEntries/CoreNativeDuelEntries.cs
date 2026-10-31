#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed partial class CoreNativeDuelEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _resolve;
    private CoreNativeDuelEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDuelEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> resolve) => new(resolve);
}

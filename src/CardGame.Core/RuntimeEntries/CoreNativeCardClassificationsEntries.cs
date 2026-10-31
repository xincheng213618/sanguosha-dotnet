#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed partial class CoreNativeCardClassificationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _resolve;
    private CoreNativeCardClassificationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardClassificationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> resolve) => new(resolve);
}

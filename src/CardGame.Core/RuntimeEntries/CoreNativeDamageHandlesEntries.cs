#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageHandlesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _resolve;
    private CoreNativeDamageHandlesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageHandlesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> resolve) => new(resolve);
}

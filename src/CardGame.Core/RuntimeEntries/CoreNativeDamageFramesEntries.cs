#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> _resolve;
    private CoreNativeDamageFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> _resolve;
    private CoreNativeDamageWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeResponseWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _resolve;
    private CoreNativeResponseWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeResponseWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeWeaponResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> _resolve;
    private CoreNativeWeaponResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeWeaponResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> resolve) => new(resolve);
}

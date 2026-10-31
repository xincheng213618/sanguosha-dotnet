#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanPingMouRuntime> _resolve;
    private CharacterGuanPingMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanPingMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanPingMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanPingMouRuntime> resolve) => new(resolve);
}

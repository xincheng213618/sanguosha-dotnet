#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangShiPingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> _resolve;
    private CharacterZhangShiPingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangShiPingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> resolve) => new(resolve);
}

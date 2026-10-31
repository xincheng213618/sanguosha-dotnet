#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuShiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> _resolve;
    private CharacterLuShiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuShiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> resolve) => new(resolve);
}

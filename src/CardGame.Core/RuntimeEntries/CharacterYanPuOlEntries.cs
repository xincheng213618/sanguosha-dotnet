#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanPuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> _resolve;
    private CharacterYanPuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanPuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> resolve) => new(resolve);
}

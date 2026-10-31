#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanJunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanJunOlRuntime> _resolve;
    private CharacterYanJunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanJunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanJunOlRuntime> resolve) => new(resolve);
}

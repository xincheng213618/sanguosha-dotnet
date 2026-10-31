#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanRouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanRouOlRuntime> _resolve;
    private CharacterYanRouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanRouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanRouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanRouOlRuntime> resolve) => new(resolve);
}

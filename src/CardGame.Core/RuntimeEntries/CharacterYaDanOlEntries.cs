#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYaDanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> _resolve;
    private CharacterYaDanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYaDanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> resolve) => new(resolve);
}

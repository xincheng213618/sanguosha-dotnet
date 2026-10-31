#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYiJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYiJiOlRuntime> _resolve;
    private CharacterYiJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYiJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYiJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYiJiOlRuntime> resolve) => new(resolve);
}

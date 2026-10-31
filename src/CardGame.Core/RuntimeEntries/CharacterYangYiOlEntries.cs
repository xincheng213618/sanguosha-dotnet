#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangYiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime> _resolve;
    private CharacterYangYiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangYiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime> resolve) => new(resolve);
}

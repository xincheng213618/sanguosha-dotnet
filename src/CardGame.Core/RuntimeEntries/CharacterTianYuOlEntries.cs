#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianYuOlRuntime> _resolve;
    private CharacterTianYuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianYuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianYuOlRuntime> resolve) => new(resolve);
}

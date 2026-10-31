#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaYuanYiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> _resolve;
    private CharacterMaYuanYiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaYuanYiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> resolve) => new(resolve);
}

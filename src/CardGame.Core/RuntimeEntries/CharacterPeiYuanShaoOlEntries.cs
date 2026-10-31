#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPeiYuanShaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> _resolve;
    private CharacterPeiYuanShaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPeiYuanShaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> resolve) => new(resolve);
}

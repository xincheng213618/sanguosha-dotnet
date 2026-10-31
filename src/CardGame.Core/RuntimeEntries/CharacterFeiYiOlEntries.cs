#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFeiYiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _resolve;
    private CharacterFeiYiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFeiYiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> resolve) => new(resolve);
}

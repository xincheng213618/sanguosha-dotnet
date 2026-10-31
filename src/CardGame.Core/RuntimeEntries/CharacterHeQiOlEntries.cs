#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> _resolve;
    private CharacterHeQiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeQiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> resolve) => new(resolve);
}

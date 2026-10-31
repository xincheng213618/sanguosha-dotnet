#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangCanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> _resolve;
    private CharacterWangCanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangCanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> resolve) => new(resolve);
}

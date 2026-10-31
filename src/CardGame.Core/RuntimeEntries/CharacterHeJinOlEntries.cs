#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeJinOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> _resolve;
    private CharacterHeJinOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeJinOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> resolve) => new(resolve);
}

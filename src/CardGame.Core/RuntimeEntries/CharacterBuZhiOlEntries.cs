#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuZhiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuZhiOlRuntime> _resolve;
    private CharacterBuZhiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuZhiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBuZhiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuZhiOlRuntime> resolve) => new(resolve);
}

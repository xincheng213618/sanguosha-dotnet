#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaoXinOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> _resolve;
    private CharacterBaoXinOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaoXinOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> resolve) => new(resolve);
}

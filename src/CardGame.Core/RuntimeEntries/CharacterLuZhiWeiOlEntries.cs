#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiWeiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> _resolve;
    private CharacterLuZhiWeiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiWeiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuoXianOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuoXianOlRuntime> _resolve;
    private CharacterLuoXianOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuoXianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuoXianOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuoXianOlRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvQianOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _resolve;
    private CharacterLvQianOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvQianOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> _resolve;
    private CharacterZhangJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> resolve) => new(resolve);
}

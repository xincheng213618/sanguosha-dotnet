#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangYiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYiOlRuntime> _resolve;
    private CharacterZhangYiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYiOlRuntime> resolve) => new(resolve);
}

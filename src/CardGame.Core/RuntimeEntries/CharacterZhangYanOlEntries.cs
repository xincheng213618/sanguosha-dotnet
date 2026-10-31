#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _resolve;
    private CharacterZhangYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> resolve) => new(resolve);
}

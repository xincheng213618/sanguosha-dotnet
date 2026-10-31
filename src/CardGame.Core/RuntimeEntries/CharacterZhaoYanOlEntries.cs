#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYanOlRuntime> _resolve;
    private CharacterZhaoYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYanOlRuntime> resolve) => new(resolve);
}

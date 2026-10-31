#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoXiangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> _resolve;
    private CharacterZhaoXiangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoXiangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> resolve) => new(resolve);
}

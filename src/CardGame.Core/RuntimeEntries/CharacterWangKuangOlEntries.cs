#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangKuangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> _resolve;
    private CharacterWangKuangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangKuangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> resolve) => new(resolve);
}

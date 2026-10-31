#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuanShuJunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _resolve;
    private CharacterHuanShuJunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuanShuJunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFanJiangZhangDaOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> _resolve;
    private CharacterFanJiangZhangDaOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFanJiangZhangDaOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> resolve) => new(resolve);
}

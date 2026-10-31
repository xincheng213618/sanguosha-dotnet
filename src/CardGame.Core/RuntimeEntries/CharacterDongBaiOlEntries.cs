#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongBaiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> _resolve;
    private CharacterDongBaiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongBaiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoXiaoQiaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime> _resolve;
    private CharacterDaQiaoXiaoQiaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoXiaoQiaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime> resolve) => new(resolve);
}

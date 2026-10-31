#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _resolve;
    private CharacterXiaoQiaoMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMouMobileRuntime> _resolve;
    private CharacterDaQiaoMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMouMobileRuntime> resolve) => new(resolve);
}

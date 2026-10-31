#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> _resolve;
    private CharacterDaQiaoMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> resolve) => new(resolve);
}

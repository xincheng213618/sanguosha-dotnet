#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenJiaoMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenJiaoMobileShiRuntime> _resolve;
    private CharacterChenJiaoMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenJiaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenJiaoMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenJiaoMobileShiRuntime> resolve) => new(resolve);
}

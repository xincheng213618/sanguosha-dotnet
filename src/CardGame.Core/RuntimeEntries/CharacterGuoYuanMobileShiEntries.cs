#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoYuanMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _resolve;
    private CharacterGuoYuanMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoYuanMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXinXianYingMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> _resolve;
    private CharacterXinXianYingMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXinXianYingMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiYanMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> _resolve;
    private CharacterWeiYanMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWeiYanMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> resolve) => new(resolve);
}

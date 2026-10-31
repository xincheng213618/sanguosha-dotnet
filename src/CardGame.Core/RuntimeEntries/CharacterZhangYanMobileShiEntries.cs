#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangYanMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> _resolve;
    private CharacterZhangYanMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYanMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> resolve) => new(resolve);
}

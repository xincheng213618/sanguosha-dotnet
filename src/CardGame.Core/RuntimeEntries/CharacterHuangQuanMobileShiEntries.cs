#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> _resolve;
    private CharacterHuangQuanMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangQuanMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> resolve) => new(resolve);
}

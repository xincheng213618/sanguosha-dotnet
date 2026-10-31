#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZangHongMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZangHongMobileShiRuntime> _resolve;
    private CharacterZangHongMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZangHongMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZangHongMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZangHongMobileShiRuntime> resolve) => new(resolve);
}

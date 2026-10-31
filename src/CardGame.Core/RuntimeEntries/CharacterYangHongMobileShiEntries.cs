#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangHongMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangHongMobileShiRuntime> _resolve;
    private CharacterYangHongMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangHongMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangHongMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangHongMobileShiRuntime> resolve) => new(resolve);
}

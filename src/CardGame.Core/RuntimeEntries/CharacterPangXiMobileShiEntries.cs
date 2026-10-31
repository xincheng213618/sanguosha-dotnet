#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangXiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangXiMobileShiRuntime> _resolve;
    private CharacterPangXiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangXiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangXiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangXiMobileShiRuntime> resolve) => new(resolve);
}

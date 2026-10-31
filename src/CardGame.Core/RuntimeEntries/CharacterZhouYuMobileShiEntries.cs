#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMobileShiRuntime> _resolve;
    private CharacterZhouYuMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMobileShiRuntime> resolve) => new(resolve);
}

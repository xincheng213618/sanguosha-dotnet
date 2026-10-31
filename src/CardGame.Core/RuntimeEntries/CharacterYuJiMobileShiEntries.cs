#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJiMobileShiRuntime> _resolve;
    private CharacterYuJiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuJiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJiMobileShiRuntime> resolve) => new(resolve);
}

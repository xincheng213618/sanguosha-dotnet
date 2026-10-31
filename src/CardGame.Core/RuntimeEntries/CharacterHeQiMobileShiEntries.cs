#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> _resolve;
    private CharacterHeQiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeQiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> resolve) => new(resolve);
}

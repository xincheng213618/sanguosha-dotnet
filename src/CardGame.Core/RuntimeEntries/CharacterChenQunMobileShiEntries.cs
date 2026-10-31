#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> _resolve;
    private CharacterChenQunMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenQunMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> resolve) => new(resolve);
}

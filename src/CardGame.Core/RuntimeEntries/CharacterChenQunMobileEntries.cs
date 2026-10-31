#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileRuntime> _resolve;
    private CharacterChenQunMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenQunMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileRuntime> resolve) => new(resolve);
}

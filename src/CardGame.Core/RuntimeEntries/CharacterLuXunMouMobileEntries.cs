#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuXunMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> _resolve;
    private CharacterLuXunMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuXunMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouDunMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> _resolve;
    private CharacterXiahouDunMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouDunMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> resolve) => new(resolve);
}

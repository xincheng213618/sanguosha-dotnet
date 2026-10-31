#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouMobileRuntime> _resolve;
    private CharacterZhouYuMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouMobileRuntime> resolve) => new(resolve);
}

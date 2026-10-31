#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianYuMouMobileRuntime> _resolve;
    private CharacterTianYuMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianYuMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianYuMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianYuMouMobileRuntime> resolve) => new(resolve);
}

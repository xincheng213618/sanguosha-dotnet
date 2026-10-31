#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhenJiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime> _resolve;
    private CharacterZhenJiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime> resolve) => new(resolve);
}

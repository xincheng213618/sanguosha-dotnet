#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeTaiHouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeTaiHouMobileRuntime> _resolve;
    private CharacterHeTaiHouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeTaiHouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeTaiHouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeTaiHouMobileRuntime> resolve) => new(resolve);
}

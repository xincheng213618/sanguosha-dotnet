#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeTaiHouMobileEntries
{
    private readonly Func<EngineOwners.CharacterHeTaiHouMobileRuntime> _resolve;
    private CharacterHeTaiHouMobileEntries(Func<EngineOwners.CharacterHeTaiHouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeTaiHouMobileEntries Create(Func<EngineOwners.CharacterHeTaiHouMobileRuntime> resolve) => new(resolve);
}

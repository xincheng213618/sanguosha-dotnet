#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuRanMouMobileRuntime> _resolve;
    private CharacterZhuRanMouMobileEntries(Func<EngineOwners.CharacterZhuRanMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRanMouMobileEntries Create(Func<EngineOwners.CharacterZhuRanMouMobileRuntime> resolve) => new(resolve);
}

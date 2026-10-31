#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRongMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuRongMouMobileRuntime> _resolve;
    private CharacterZhuRongMouMobileEntries(Func<EngineOwners.CharacterZhuRongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRongMouMobileEntries Create(Func<EngineOwners.CharacterZhuRongMouMobileRuntime> resolve) => new(resolve);
}

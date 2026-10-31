#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDingFengMobileEntries
{
    private readonly Func<EngineOwners.CharacterDingFengMobileRuntime> _resolve;
    private CharacterDingFengMobileEntries(Func<EngineOwners.CharacterDingFengMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDingFengMobileEntries Create(Func<EngineOwners.CharacterDingFengMobileRuntime> resolve) => new(resolve);
}

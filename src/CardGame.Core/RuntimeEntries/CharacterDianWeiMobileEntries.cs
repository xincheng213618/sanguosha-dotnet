#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiMobileEntries
{
    private readonly Func<EngineOwners.CharacterDianWeiMobileRuntime> _resolve;
    private CharacterDianWeiMobileEntries(Func<EngineOwners.CharacterDianWeiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiMobileEntries Create(Func<EngineOwners.CharacterDianWeiMobileRuntime> resolve) => new(resolve);
}

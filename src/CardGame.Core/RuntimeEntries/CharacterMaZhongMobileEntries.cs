#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaZhongMobileEntries
{
    private readonly Func<EngineOwners.CharacterMaZhongMobileRuntime> _resolve;
    private CharacterMaZhongMobileEntries(Func<EngineOwners.CharacterMaZhongMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaZhongMobileEntries Create(Func<EngineOwners.CharacterMaZhongMobileRuntime> resolve) => new(resolve);
}

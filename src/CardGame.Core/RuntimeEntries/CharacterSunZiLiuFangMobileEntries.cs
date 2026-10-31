#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunZiLiuFangMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunZiLiuFangMobileRuntime> _resolve;
    private CharacterSunZiLiuFangMobileEntries(Func<EngineOwners.CharacterSunZiLiuFangMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunZiLiuFangMobileEntries Create(Func<EngineOwners.CharacterSunZiLiuFangMobileRuntime> resolve) => new(resolve);
}

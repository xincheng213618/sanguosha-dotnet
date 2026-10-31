#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFuHuangHouMobileEntries
{
    private readonly Func<EngineOwners.CharacterFuHuangHouMobileRuntime> _resolve;
    private CharacterFuHuangHouMobileEntries(Func<EngineOwners.CharacterFuHuangHouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuHuangHouMobileEntries Create(Func<EngineOwners.CharacterFuHuangHouMobileRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJianBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunJianBoundaryMobileRuntime> _resolve;
    private CharacterSunJianBoundaryMobileEntries(Func<EngineOwners.CharacterSunJianBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJianBoundaryMobileEntries Create(Func<EngineOwners.CharacterSunJianBoundaryMobileRuntime> resolve) => new(resolve);
}

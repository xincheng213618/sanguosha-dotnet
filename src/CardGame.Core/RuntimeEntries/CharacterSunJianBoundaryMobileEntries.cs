#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJianBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianBoundaryMobileRuntime> _resolve;
    private CharacterSunJianBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJianBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianBoundaryMobileRuntime> resolve) => new(resolve);
}

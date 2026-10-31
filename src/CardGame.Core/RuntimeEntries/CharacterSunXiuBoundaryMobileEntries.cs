#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunXiuBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunXiuBoundaryMobileRuntime> _resolve;
    private CharacterSunXiuBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunXiuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunXiuBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunXiuBoundaryMobileRuntime> resolve) => new(resolve);
}

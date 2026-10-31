#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengBoundaryMobileRuntime> _resolve;
    private CharacterLuMengBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengBoundaryMobileRuntime> resolve) => new(resolve);
}

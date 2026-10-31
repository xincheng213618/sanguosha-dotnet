#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuMengBoundaryMobileRuntime> _resolve;
    private CharacterLuMengBoundaryMobileEntries(Func<EngineOwners.CharacterLuMengBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengBoundaryMobileEntries Create(Func<EngineOwners.CharacterLuMengBoundaryMobileRuntime> resolve) => new(resolve);
}

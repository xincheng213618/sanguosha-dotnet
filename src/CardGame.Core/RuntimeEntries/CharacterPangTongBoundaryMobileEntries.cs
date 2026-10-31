#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterPangTongBoundaryMobileRuntime> _resolve;
    private CharacterPangTongBoundaryMobileEntries(Func<EngineOwners.CharacterPangTongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongBoundaryMobileEntries Create(Func<EngineOwners.CharacterPangTongBoundaryMobileRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongBoundaryMobileRuntime> _resolve;
    private CharacterPangTongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongBoundaryMobileRuntime> resolve) => new(resolve);
}

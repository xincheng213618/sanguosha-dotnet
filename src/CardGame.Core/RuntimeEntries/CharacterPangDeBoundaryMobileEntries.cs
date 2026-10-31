#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangDeBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> _resolve;
    private CharacterPangDeBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangDeBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> resolve) => new(resolve);
}

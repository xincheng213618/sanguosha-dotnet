#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime))]
internal sealed partial class CharacterPangDeBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> _resolve;
    private CharacterPangDeBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangDeBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> resolve) => new(resolve);
}

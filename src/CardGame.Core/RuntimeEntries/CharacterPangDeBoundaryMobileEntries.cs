#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPangDeBoundaryMobileRuntime))]
internal sealed partial class CharacterPangDeBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterPangDeBoundaryMobileRuntime> _resolve;
    private CharacterPangDeBoundaryMobileEntries(Func<EngineOwners.CharacterPangDeBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangDeBoundaryMobileEntries Create(Func<EngineOwners.CharacterPangDeBoundaryMobileRuntime> resolve) => new(resolve);
}

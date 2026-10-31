#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunCeBoundaryMobileRuntime))]
internal sealed partial class CharacterSunCeBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunCeBoundaryMobileRuntime> _resolve;
    private CharacterSunCeBoundaryMobileEntries(Func<EngineOwners.CharacterSunCeBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunCeBoundaryMobileEntries Create(Func<EngineOwners.CharacterSunCeBoundaryMobileRuntime> resolve) => new(resolve);
}

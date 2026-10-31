#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunXiuBoundaryMobileRuntime))]
internal sealed partial class CharacterSunXiuBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunXiuBoundaryMobileRuntime> _resolve;
    private CharacterSunXiuBoundaryMobileEntries(Func<EngineOwners.CharacterSunXiuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunXiuBoundaryMobileEntries Create(Func<EngineOwners.CharacterSunXiuBoundaryMobileRuntime> resolve) => new(resolve);
}

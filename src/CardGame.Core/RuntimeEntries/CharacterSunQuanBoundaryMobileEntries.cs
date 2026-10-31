#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunQuanBoundaryMobileRuntime))]
internal sealed partial class CharacterSunQuanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunQuanBoundaryMobileRuntime> _resolve;
    private CharacterSunQuanBoundaryMobileEntries(Func<EngineOwners.CharacterSunQuanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanBoundaryMobileEntries Create(Func<EngineOwners.CharacterSunQuanBoundaryMobileRuntime> resolve) => new(resolve);
}

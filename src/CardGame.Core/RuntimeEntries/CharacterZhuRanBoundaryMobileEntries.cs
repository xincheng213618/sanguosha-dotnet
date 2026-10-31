#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhuRanBoundaryMobileRuntime))]
internal sealed partial class CharacterZhuRanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuRanBoundaryMobileRuntime> _resolve;
    private CharacterZhuRanBoundaryMobileEntries(Func<EngineOwners.CharacterZhuRanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRanBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhuRanBoundaryMobileRuntime> resolve) => new(resolve);
}

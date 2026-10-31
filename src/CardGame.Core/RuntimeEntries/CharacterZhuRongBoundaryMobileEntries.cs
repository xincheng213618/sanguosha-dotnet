#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhuRongBoundaryMobileRuntime))]
internal sealed partial class CharacterZhuRongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuRongBoundaryMobileRuntime> _resolve;
    private CharacterZhuRongBoundaryMobileEntries(Func<EngineOwners.CharacterZhuRongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRongBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhuRongBoundaryMobileRuntime> resolve) => new(resolve);
}

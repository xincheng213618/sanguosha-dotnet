#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFaZhengBoundaryMobileRuntime))]
internal sealed partial class CharacterFaZhengBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterFaZhengBoundaryMobileRuntime> _resolve;
    private CharacterFaZhengBoundaryMobileEntries(Func<EngineOwners.CharacterFaZhengBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFaZhengBoundaryMobileEntries Create(Func<EngineOwners.CharacterFaZhengBoundaryMobileRuntime> resolve) => new(resolve);
}

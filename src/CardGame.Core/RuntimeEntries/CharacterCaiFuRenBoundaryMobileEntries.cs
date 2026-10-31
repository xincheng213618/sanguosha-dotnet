#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime))]
internal sealed partial class CharacterCaiFuRenBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime> _resolve;
    private CharacterCaiFuRenBoundaryMobileEntries(Func<EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiFuRenBoundaryMobileEntries Create(Func<EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime> resolve) => new(resolve);
}

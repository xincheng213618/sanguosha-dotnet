#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouCangBoundaryMobileRuntime))]
internal sealed partial class CharacterZhouCangBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhouCangBoundaryMobileRuntime> _resolve;
    private CharacterZhouCangBoundaryMobileEntries(Func<EngineOwners.CharacterZhouCangBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouCangBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhouCangBoundaryMobileRuntime> resolve) => new(resolve);
}

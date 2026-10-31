#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYanYanBoundaryMobileRuntime))]
internal sealed partial class CharacterYanYanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterYanYanBoundaryMobileRuntime> _resolve;
    private CharacterYanYanBoundaryMobileEntries(Func<EngineOwners.CharacterYanYanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanYanBoundaryMobileEntries Create(Func<EngineOwners.CharacterYanYanBoundaryMobileRuntime> resolve) => new(resolve);
}

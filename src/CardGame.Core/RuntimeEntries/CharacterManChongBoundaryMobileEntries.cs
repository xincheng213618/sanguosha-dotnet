#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterManChongBoundaryMobileRuntime))]
internal sealed partial class CharacterManChongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterManChongBoundaryMobileRuntime> _resolve;
    private CharacterManChongBoundaryMobileEntries(Func<EngineOwners.CharacterManChongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterManChongBoundaryMobileEntries Create(Func<EngineOwners.CharacterManChongBoundaryMobileRuntime> resolve) => new(resolve);
}

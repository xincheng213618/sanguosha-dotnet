#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuYongBoundaryMobileRuntime))]
internal sealed partial class CharacterGuYongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterGuYongBoundaryMobileRuntime> _resolve;
    private CharacterGuYongBoundaryMobileEntries(Func<EngineOwners.CharacterGuYongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuYongBoundaryMobileEntries Create(Func<EngineOwners.CharacterGuYongBoundaryMobileRuntime> resolve) => new(resolve);
}

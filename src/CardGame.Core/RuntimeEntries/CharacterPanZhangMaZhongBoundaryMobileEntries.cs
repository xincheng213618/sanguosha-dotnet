#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime))]
internal sealed partial class CharacterPanZhangMaZhongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> _resolve;
    private CharacterPanZhangMaZhongBoundaryMobileEntries(Func<EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPanZhangMaZhongBoundaryMobileEntries Create(Func<EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> resolve) => new(resolve);
}

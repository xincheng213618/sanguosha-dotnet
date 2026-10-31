#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime))]
internal sealed partial class CharacterPanZhangMaZhongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> _resolve;
    private CharacterPanZhangMaZhongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPanZhangMaZhongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> resolve) => new(resolve);
}

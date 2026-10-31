#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryAnjianHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime>> getCharacterPanZhangMaZhongBoundaryMobileRuntime) : IBoundaryAnjianHost
{
    public void Issue(ProgramSkillFrame frame) => getCharacterPanZhangMaZhongBoundaryMobileRuntime().Value.IssueBoundaryAnjianChoice(frame);
}

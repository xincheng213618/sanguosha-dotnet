#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryAnjianHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries> getCharacterPanZhangMaZhongBoundaryMobileRuntime) : IBoundaryAnjianHost
{
    public void Issue(ProgramSkillFrame frame) => getCharacterPanZhangMaZhongBoundaryMobileRuntime().IssueBoundaryAnjianChoice(frame);
}

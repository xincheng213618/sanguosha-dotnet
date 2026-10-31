#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryAnjianHost(Func<RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries> getCharacterPanZhangMaZhongBoundaryMobileRuntime) : IBoundaryAnjianHost
{
    public void Issue(ProgramSkillFrame frame) => getCharacterPanZhangMaZhongBoundaryMobileRuntime().IssueBoundaryAnjianChoice(frame);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXuChuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuChuBoundaryMobileEntries> getCharacterXuChuBoundaryMobileRuntime) : IBoundaryXuChuHost
{
    public void SubstituteBoundaryXuChuDraw(ProgramSkillFrame f) => getCharacterXuChuBoundaryMobileRuntime().SubstituteBoundaryXuChuDraw(f);
}

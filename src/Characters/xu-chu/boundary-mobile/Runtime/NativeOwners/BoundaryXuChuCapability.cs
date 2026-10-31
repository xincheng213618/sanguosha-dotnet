#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXuChuCapability(Func<RuntimeEntries.CharacterXuChuBoundaryMobileEntries> getCharacterXuChuBoundaryMobileRuntime) : IBoundaryXuChuHost
{
    public void SubstituteBoundaryXuChuDraw(ProgramSkillFrame f) => getCharacterXuChuBoundaryMobileRuntime().SubstituteBoundaryXuChuDraw(f);
}
